using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Details(int id)
        {
            IEnumerable<SolicitudMaterial> solicitudMateriales = Fachada.BuscarMaterialesSolicitud(id);
            Solicitud solicitud = Fachada.BuscarSolicitud(id);
            ViewBag.Materiales = solicitudMateriales;
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
            catch(Exception e)
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
                Obra obra = Fachada.BuscarObra(IdObra);
                Solicitud solicitud = new Solicitud();
                solicitud.IdObra = IdObra;
                Usuario solicitante = Fachada.BuscarUsuarioXNombreU(HttpContext.Session.GetString("UsuarioLogueado"));
                solicitud.IdUsuario = solicitante.Id;
                solicitud.Estado = Estado.Solicitado;
                Fachada.AgregarSolicitud(solicitud);
                item = Fachada.DarIdAMaterialesSolicitud(solicitud, item);
                Fachada.AgregarSolicitudMateriales(item);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Agregar(SolicitudMaterial solicitudMaterial)
        {
            try
            {
                List<SolicitudMaterial> item;
                if (TempData["ListaActual"] != null)
                {
                    item = JsonConvert.DeserializeObject<List<SolicitudMaterial>>((string)TempData["ListaActual"]);
                }
                else
                {
                    item = new List<SolicitudMaterial>();
                }

                item.Add(solicitudMaterial);
                TempData["ListaActual"] = JsonConvert.SerializeObject(item);
                ViewBag.Materiales = Fachada.TodosLosMateriales();
                return PartialView("ListaMateriales", item); // Aquí se usa ListaMateriales
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        // GET: SolicitudController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SolicitudController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SolicitudController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SolicitudController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
