using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MVC.Models;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC.Controllers
{
    public class PlanoController : Controller
    {

        private Fachada Fachada = new Fachada();

        // GET: PlanoController
        public ActionResult Index(int idObra)
        {
            try
            {
                if (TempData["Error"] != null) //Para cuando viene de Agregar Plano siendo finalizada
                {
                    ViewBag.Error = TempData["Error"].ToString();
                }
                TempData["Error"] = null;
                Obra obra = Fachada.BuscarObra(idObra);
                IEnumerable<Plano> planos = Fachada.PlanosTotales(obra);
                if (planos == null)
                {
                    planos = new List<LogicaNegocio.Entidades.Plano>();
                }
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View(planos);
            }
            catch (ObraException e) //Solo manda ObraException si no existe obra
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
            catch(Exception e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult IndexFiltrado(int idObra, string nombre, int idTipoPlano, DateTime? fechaInicio, DateTime? fechaFin)
        {
            Obra obra = Fachada.BuscarObra(idObra);
            IEnumerable<Plano> planosFiltrados = Fachada.PlanosFiltrados(obra,idTipoPlano,nombre,fechaInicio,fechaFin);
            ViewBag.IdObra = idObra;
            ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
            return View(planosFiltrados);
        }


        // GET: PlanoController/Create
        public ActionResult Agregar(int idObra)
        {
            TempData["Error"] = null;
            ViewBag.IdObra = idObra;
            if (!Fachada.BuscarObra(idObra).Finalizada)
            {
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View();
            }
            else
            {
                TempData["Error"] = "No se puede agregar planos a una obra finalizada.";
                return RedirectToAction("Index", new { idObra = idObra });
            }
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Agregar(Plano aIngresar, IFormFile archivoImagen)
        {
            try
            {
                if (aIngresar == null || aIngresar.TipoPdf != "application/pdf")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivoImagen.CopyToAsync(memoryStream);
                        aIngresar.NombrePdf = archivoImagen.FileName;
                        aIngresar.TipoPdf = archivoImagen.ContentType;
                        aIngresar.Pdf = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["Error"] = "Debe proporcionar un archivo válido.";
                    return View();
                }
                if(aIngresar.Nombre == null)
                {
                    aIngresar.Nombre = aIngresar.NombrePdf.ToUpper();
                    string path = aIngresar.Nombre;
                    string pattern = @"\.\w+$";
                    Match match = Regex.Match(path, pattern);
                    aIngresar.Nombre = aIngresar.Nombre.Replace(match.Value, "");
                }
                ViewBag.IdObra = aIngresar.IdObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                Fachada.AgregarPlano(aIngresar);
                return RedirectToAction("Index", new { idObra = aIngresar.IdObra });
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Index", new { idObra = aIngresar.IdObra });
            }
        }


        // GET: PlanoController/Delete/5
        public ActionResult Eliminar(int id)
        {
            Plano plano = Fachada.BuscarPlano(id);
            return View(plano);
        }

        // POST: PlanoController/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            try
            {
                Plano planoABorrar = Fachada.BuscarPlano(id);
                Fachada.EliminarPlano(planoABorrar);
                return RedirectToAction("Index", new { idObra = planoABorrar.IdObra });
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }



        [HttpGet("{id}/pdf")]
        public IActionResult ObtenerPDF(int id)
        {
            Plano plano = Fachada.BuscarPlano(id);

            if (plano == null || plano.Pdf == null)
            {
                return NotFound();
            }

            return File(plano.Pdf, plano.TipoPdf, plano.NombrePdf);
        }
    }
}
