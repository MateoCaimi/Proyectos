using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class SolicitudController : Controller
    {

        Fachada Fachada = new Fachada();
        // GET: SolicitudController
        public ActionResult Index(int idObra)
        {
            IEnumerable<Solicitud> solicitudesObra = Fachada.SolicitudesDeObra(idObra);
            ViewBag.IdObra = idObra;
            return View(solicitudesObra);
        }

        // GET: SolicitudController/Details/5
        public ActionResult Detalles(int id, int idObra)
        {

            IEnumerable<SolicitudMaterial> solicitudMateriales = Fachada.BuscarMaterialesSolicitud(id);
            Solicitud solicitud = Fachada.BuscarSolicitud(id);
            ViewBag.Materiales = solicitudMateriales;
            ViewBag.IdObra = idObra;
            return View(solicitud);

        }

        // GET: SolicitudController/Create
        public ActionResult Crear(int idObra)
        {
            try
            {
                TempData["ListaActual"] = null;
                if (!Fachada.BuscarObra(idObra).Finalizada)
                {
                    ViewBag.Materiales = Fachada.TodosLosMateriales();
                    ViewBag.IdObra = idObra;

                    List<SolicitudMaterial> tempMaterials = new List<SolicitudMaterial>();
                    return View(tempMaterials);
                }
                else
                {
                    ViewBag.Error = "Esta obra está cerrada, no se pueden hacer solicitudes";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.IdObra = idObra;
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: SolicitudController/Crear
        [HttpPost, ActionName("Crear")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearPost(int IdObra)
        {
            try
            {
                List<SolicitudMaterial> item = JsonConvert.DeserializeObject<List<SolicitudMaterial>>((string)TempData["ListaActual"]);
                Usuario solicitante = Fachada.BuscarUsuarioXNombreU(HttpContext.Session.GetString("UsuarioLogueado"));
                Solicitud solicitud = Fachada.CrearSolicitud(IdObra, solicitante.Id);
                Fachada.AgregarSolicitud(solicitud);
                Fachada.AgregarSolicitudMateriales(solicitud, item);
                return RedirectToAction("Index", new { idObra = IdObra });
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index", new { idObra = IdObra });
            }
        }

        [HttpPost]
        public ActionResult Agregar(SolicitudMaterial solicitudMaterial)
        {
            try
            {
                List<SolicitudMaterial> lista;
                if (TempData["ListaActual"] != null)
                {
                    lista = JsonConvert.DeserializeObject<List<SolicitudMaterial>>((string)TempData["ListaActual"]);
                }
                else
                {
                    lista = new List<SolicitudMaterial>();
                }
                Material material = Fachada.BuscarMaterial(solicitudMaterial.IdMaterial);
                solicitudMaterial.Material = material;
                lista.Add(solicitudMaterial);
                TempData["ListaActual"] = JsonConvert.SerializeObject(lista);
                ViewBag.Materiales = Fachada.TodosLosMateriales();
                return PartialView("ListaMateriales", lista); // Aquí se usa ListaMateriales
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult Aprobar(int solicitudId, List<int> materialesSeleccionados, IFormCollection form)
        {
            try
            {
                Dictionary<int, int> materialesSeleccionadosConCantidad = new Dictionary<int, int>();

                if (materialesSeleccionados.Count != 0)
                {
                    foreach (int materialId in materialesSeleccionados)
                    {
                        // Obtener la cantidad correspondiente de los materiales seleccionados
                        int cantidad = int.Parse(form[$"Cantidad_{materialId}"]);
                        materialesSeleccionadosConCantidad.Add(materialId, cantidad);
                    }


                    IEnumerable<SolicitudMaterial> laSolicitudConMateriales = Fachada.BuscarMaterialesSolicitud(solicitudId);
                    Fachada.ConfigurarSolicitud(laSolicitudConMateriales, materialesSeleccionadosConCantidad);
                    Usuario aprobador = Fachada.BuscarUsuarioXNombreU(HttpContext.Session.GetString("UsuarioLogueado"));
                    Solicitud solicitud = Fachada.BuscarSolicitud(solicitudId);
                    Fachada.AceptarSolicitud(solicitud, (UDeOficina)aprobador);
                    //Dejar notificacion de solicitud aprovada al solicitante y tarea de confirmar que llegue el material
                    //hacer pdf orden de compra
                    //problema si el solicitante es el mismo que el aprobador
                    return RedirectToAction("Index", new { idObra = solicitud.IdObra });
                }
                else
                {
                    ViewBag.Error("No se selecciono ningun material");
                    Solicitud solicitud = Fachada.BuscarSolicitud(solicitudId);
                    return RedirectToAction("Index", new { idObra = solicitud.IdObra });
                }

            }
            catch (Exception e)
            {
                ViewBag.Error(e.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Rechazar(int SolicitudId)
        {
            try
            {
                Solicitud solicitud = Fachada.BuscarSolicitud(SolicitudId);
                if (solicitud == null)
                {
                    ViewBag.Error = "La solicitud no existe.";
                    return View();
                }
                Usuario rechazador = Fachada.BuscarUsuarioXNombreU(HttpContext.Session.GetString("UsuarioLogueado"));
                Fachada.RechazarSolicitud(solicitud, (UDeOficina)rechazador);
                ViewBag.Mensaje = "Solicitud rechazada correctamente.";
                //Dejar notificacion de solicitud rechazada al solicitante 

                return RedirectToAction("Index", new { idObra = solicitud.IdObra });
            }
            catch (Exception ex)
            {
                Solicitud solicitud = Fachada.BuscarSolicitud(SolicitudId);
                ViewBag.Error = $"Error al procesar la solicitud: {ex.Message}";
                return RedirectToAction("Index", new { idObra = solicitud.IdObra });
            }
        }

    }
}
