using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using PdfSharp.Pdf.Advanced;
using System.IO;
using LogicaNegocio.Excepciones;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MVC.Controllers
{
    public class SolicitudController : Controller
    {

        Fachada Fachada = new Fachada();

        // GET: SolicitudController
        public ActionResult Index(int idObra)
        {
            Fachada.Precarga();
            if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de oficina")
            {


                List<Solicitud> solicitudesPendientes = Fachada.BuscarSolicitudPendientesLista();
                var opciones = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,

                };
                HttpContext.Session.SetString("SolicitudesPendientes", System.Text.Json.JsonSerializer.Serialize(solicitudesPendientes, opciones));




                List<Solicitud> solicitudesConfirmadas = Fachada.BuscarSolicitudConfirmadasLista();
                var opciones2 = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,

                };
                HttpContext.Session.SetString("SolicitudesConfirmadas", System.Text.Json.JsonSerializer.Serialize(solicitudesConfirmadas, opciones2));

            }



            if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de obra")
            {
                string nomObrero = HttpContext.Session.GetString("UsuarioLogueado");
                List<Solicitud> solicitudesAprobadas = Fachada.BuscarSolicitudAprobadasParaUnUObra(nomObrero);
                var opciones3 = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,

                };
                HttpContext.Session.SetString("SolicitudesAprobadas", System.Text.Json.JsonSerializer.Serialize(solicitudesAprobadas, opciones3));
            }




            IEnumerable<Solicitud> solicitudesObra = Fachada.SolicitudesDeObra(idObra);
            ViewBag.IdObra = idObra;
            return View(solicitudesObra);
        }

        // GET: SolicitudController/Details/5
        public ActionResult Detalles(int idSolicitud)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }

            IEnumerable<SolicitudMaterial> solicitudMateriales = Fachada.BuscarMaterialesSolicitud(idSolicitud);
            Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
            if (solicitud.Estado == Estado.Recibido)
            {
                Fachada.CambiarEstadoAVisto(solicitud);

            }
            ViewBag.Materiales = solicitudMateriales;
            ViewBag.IdObra = solicitud.IdObra;

            if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de obra")
            {
                return RedirectToAction("Confirmar", new { idSolicitud = solicitud.Id });
            }

            return View(solicitud);

        }

        // GET: SolicitudController/Create
        public ActionResult Crear(int idObra)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }

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
            catch (SolicitudException e)
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
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }

            try
            {
                if (TempData["ListaActual"] == null)
                {
                    TempData["ErrorNoMateriales"] = "No se puede crear una solicitud sin materiales";  //Sin temp data dificil porque es un redirect no un return view. el view bag no funciona
                    throw new SolicitudException("No se puede crear una solicitud sin materiales");
                }
                List<SolicitudMaterial> item = JsonConvert.DeserializeObject<List<SolicitudMaterial>>((string)TempData["ListaActual"]);
                Usuario solicitante = Fachada.BuscarUsuarioXNombreU(HttpContext.Session.GetString("UsuarioLogueado"));
                Solicitud solicitud = Fachada.CrearSolicitud(IdObra, solicitante.Id);
                Fachada.AgregarSolicitud(solicitud);
                Fachada.AgregarSolicitudMateriales(solicitud, item);
                return RedirectToAction("Index", new { idObra = IdObra });
            }
            catch (SolicitudException e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Crear", new { idObra = IdObra });
            }
        }

        [HttpPost]
        public ActionResult Agregar(SolicitudMaterial solicitudMaterial)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }

            try
            {
                //solicitudMaterial.Validar();
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
                if (lista != null)
                {
                    foreach (SolicitudMaterial sm in lista)
                    {
                        if (solicitudMaterial.IdMaterial == sm.IdMaterial)
                        {
                            sm.Cantidad += solicitudMaterial.Cantidad;
                            TempData["ListaActual"] = JsonConvert.SerializeObject(lista);
                            ViewBag.Materiales = Fachada.TodosLosMateriales();
                            return PartialView("ListaMateriales", lista);

                        }
                    }

                }
                lista.Add(solicitudMaterial);
                TempData["ListaActual"] = JsonConvert.SerializeObject(lista);
                ViewBag.Materiales = Fachada.TodosLosMateriales();
                return PartialView("ListaMateriales", lista); // Aquí se usa ListaMateriales
            }
            catch (SolicitudException se)
            {
                return View(se);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Aprobar(int idSolicitud, List<int> materialesSeleccionados, IFormCollection form)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario obra")
            {
                return RedirectToAction("Index");
            }

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



                    IEnumerable<SolicitudMaterial> laSolicitudConMateriales = Fachada.BuscarMaterialesSolicitud(idSolicitud);
                    Fachada.ConfigurarSolicitud(laSolicitudConMateriales, materialesSeleccionadosConCantidad);
                    Usuario aprobador = Fachada.BuscarUsuarioXNombreU(HttpContext.Session.GetString("UsuarioLogueado"));
                    Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
                    Fachada.AceptarSolicitud(solicitud, (UDeOficina)aprobador);
                    return GenerarPdf(solicitud);
                }
                else
                {
                    ViewBag.Error = "No se ha seleccionado ningun material";
                    Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
                    ViewBag.Materiales = Fachada.BuscarMaterialesSolicitud(idSolicitud);
                    ViewBag.IdObra = solicitud.IdObra;
                    return View("Detalles", solicitud);
                }

            }
            catch (SolicitudException se)
            {
                ViewBag.Error(se.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Rechazar(int idSolicitud)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario obra")
            {
                return RedirectToAction("Index");
            }

            try
            {
                Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
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
                Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
                ViewBag.Error = $"Error al procesar la solicitud: {ex.Message}";
                return RedirectToAction("Index", new { idObra = solicitud.IdObra });
            }
        }

        public IActionResult Confirmar(int idSolicitud)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }
            

            Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
            IEnumerable<SolicitudMaterial> solicitudMateriales = Fachada.BuscarMaterialesSolicitud(idSolicitud);
            ViewBag.Materiales = solicitudMateriales;
            return View(solicitud);
        }


        [HttpPost, ActionName("Confirmar")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarPost(int idSolicitud)
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }
           
            try
            {
                Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
                if (solicitud == null)
                {
                    ViewBag.Error = "La solicitud no existe.";
                    return View();
                }
                Usuario confirmador = Fachada.BuscarUsuarioXNombreU(HttpContext.Session.GetString("UsuarioLogueado"));
                Fachada.ConfirmarSolicitud(solicitud, confirmador);
                ViewBag.Mensaje = "Solicitud confirmada correctamente.";
                //Dejar notificacion de solicitud confirmada al udeoficina 

                return RedirectToAction("Index", new { idObra = solicitud.IdObra });
            }
            catch (Exception ex)
            {
                Solicitud solicitud = Fachada.BuscarSolicitud(idSolicitud);
                ViewBag.Error = $"Error al procesar la solicitud: {ex.Message}";
                return RedirectToAction("Index", new { idObra = solicitud.IdObra });
            }
        }


        private ActionResult GenerarPdf(Solicitud solicitud)
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }

            IEnumerable<SolicitudMaterial> materialesSolicitud = Fachada.BuscarMaterialesSolicitud(solicitud.Id);

            //PdfDocument document = new PdfDocument();
            //PdfPage page = document.AddPage();
            //XGraphics gfx = XGraphics.FromPdfPage(page);
            //XFont font = new XFont("Verdana", 20, XFontStyleEx.Bold);
            //XFont fontLineas = new XFont("Verdana", 12, XFontStyleEx.Regular);
            //XFont fontHeader = new XFont("Verdana", 15, XFontStyleEx.Italic);
            //XFont fontFooter = new XFont("Verdana", 12, XFontStyleEx.BoldItalic);
            //gfx.DrawString("Solicitud de materiales", font, XBrushes.Black,
            //new XRect(0, 0, page.Width, page.Height),
            //XStringFormat.TopCenter);
            //gfx.DrawString("Bodega&Piedrafita Arquitectos", fontHeader, XBrushes.Black,
            //new XRect(0, 25, page.Width, page.Height),
            //XStringFormat.TopCenter);


            //XPen line = new XPen(XColors.Black, 2);
            //gfx.DrawLine(line, 0, 80, page.Width, 80);

            //int i = 90;
            //foreach (SolicitudMaterial sm in materialesSolicitud)
            //{
            //    gfx.DrawString(sm.Material.Nombre + " - " + sm.Cantidad + " - " + sm.Material.UnidadDeMedida, fontLineas, XBrushes.Black,
            //    new XRect(0, i, page.Width, page.Height),
            //    XStringFormat.TopLeft);
            //    i += 25;
            //}

            //gfx.DrawLine(line, 0, 800, page.Width, 800);
            //gfx.DrawString("Teléfono: 2600 1150 - Dirección: Formentor 7096 - Bodega&Piedrafita Arquitectos", fontFooter, XBrushes.Black,
            //new XRect(0, 400, page.Width, page.Height),
            //XStringFormat.Center);

            //string filename = $"{solicitud.Obra.Nombre} - {solicitud.Solicitante.Nombre}.pdf";




            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fontTitle = new XFont("Verdana", 20, XFontStyleEx.Bold);
            XFont fontLineas = new XFont("Verdana", 12, XFontStyleEx.Regular);
            XFont fontHeader = new XFont("Verdana", 15, XFontStyleEx.Italic);
            XFont fontFooter = new XFont("Verdana", 12, XFontStyleEx.BoldItalic);
          
           

            double margin = 40;
            double yPos = margin;

            // Título
            gfx.DrawString("Solicitud de materiales", fontTitle, XBrushes.Black,
                new XRect(0, yPos, page.Width, page.Height),
                XStringFormat.TopCenter);

            yPos += 30;  // Espacio después del título

            // Subtítulo
            gfx.DrawString("Bodega & Piedrafita Arquitectos", fontHeader, XBrushes.Black,
                new XRect(0, yPos, page.Width, page.Height),
                XStringFormat.TopCenter);

            yPos += 20;  // Espacio después del subtítulo

            // Línea horizontal
            XPen line = new XPen(XColors.Black, 1);
            gfx.DrawLine(line, margin, yPos, page.Width - margin, yPos);

            yPos += 20;  // Espacio después de la línea

            // Detalles de los materiales
            foreach (SolicitudMaterial sm in materialesSolicitud)
            {
                gfx.DrawString($"{sm.Material.Nombre}: {sm.Cantidad} {sm.Material.UnidadDeMedida}", fontLineas, XBrushes.Black,
                    new XRect(margin, yPos, page.Width - 2 * margin, page.Height),
                    XStringFormat.TopLeft);
                yPos += 20;
            }

            // Línea horizontal
            gfx.DrawLine(line, margin, yPos, page.Width - margin, yPos);

            yPos += 10;  // Espacio después de la línea



            double footerHeight = 50;  // Altura del pie de página
            double footerYPos = page.Height - margin - footerHeight;

            gfx.DrawLine(new XPen(XColors.Black, 1), margin, footerYPos, page.Width - margin, footerYPos); // Línea superior del pie de página



            //// Pie de página

            gfx.DrawString("Teléfono: 2600 1150", fontFooter, XBrushes.Gray,
                new XRect(0, footerYPos + 5, page.Width, footerHeight / 2),
                XStringFormat.Center);

            gfx.DrawString("Dirección: Formentor 7096", fontFooter, XBrushes.Gray,
                new XRect(0, footerYPos + 20, page.Width, footerHeight / 2),
                XStringFormat.Center);

            gfx.DrawString("Bodega & Piedrafita Arquitectos", fontFooter, XBrushes.Gray,
                new XRect(0, footerYPos + 35, page.Width, footerHeight / 2),
                XStringFormat.Center);


            //gfx.DrawLine(line, 0, 800, page.Width, 800);

            //gfx.DrawString("Teléfono: 2600 1150 - Dirección: Formentor 7096 - Bodega&Piedrafita Arquitectos", fontFooter, XBrushes.Black,
            //new XRect(0, 400, page.Width, page.Height),
            //XStringFormat.Center);


            //gfx.DrawString("Teléfono: 2600 1150 - Dirección: Formentor 7096 - Bodega & Piedrafita Arquitectos", fontFooter, XBrushes.Black,
            //new XRect(0, page.Height - margin, page.Width, page.Height), XStringFormat.BottomCenter);

            // Guardar el documento
            string filename = $"{solicitud.Obra.Nombre} - {solicitud.Solicitante.Nombre}.pdf";
           




            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream);
                return File(stream.ToArray(), "application/pdf", "Solicitud.pdf");
                /*Response.HttpContext.Response.Clear();
                Response.HttpContext.Response.ContentType = "application/pdf";
                Response.HttpContext.Response.Headers.Add("Content-Disposition", String.Format("attachment;filename={0}", filename));
                Response.HttpContext.Response.Headers.Expires = DateTime.Now.AddDays(30).ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
                Response.HttpContext.Response.Headers.Add("content-length", stream.Length.ToString());
                await Response.HttpContext.Response.Body.WriteAsync(stream.ToArray());
                await Response.HttpContext.Response.Body.FlushAsync();*/
            }



        }
    }
}
