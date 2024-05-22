using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVC.Models;
using System.Numerics;

namespace MVC.Controllers
{
    public class PlanoController : Controller
    {

        private IRepositorioPlano Repositorio = new RepositorioPlano();
        private IRepositorioObra RepoObra = new RepositorioObra();

        // GET: PlanoController
        public ActionResult Index(int idObra)
        {
            try
            {
                Obra obra = RepoObra.Buscar(idObra);
                IEnumerable<Plano> planos = Repositorio.PlanosTotales(obra);
                if (planos == null)
                {
                    planos = new List<LogicaNegocio.Entidades.Plano>();
                }
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Repositorio.BuscarTiposPlanos();
                return View(planos);
            }
            catch (ObraException e) //Solo manda ObraException si no existe obra
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Repositorio.BuscarTiposPlanos();
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
            catch(Exception e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Repositorio.BuscarTiposPlanos();
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult IndexFiltrado(int idObra, string nombre, int idTipoPlano, DateTime? fechaInicio, DateTime? fechaFin)
        {
            Obra obra =RepoObra.Buscar(idObra);
            IEnumerable<Plano> planosFiltrados = Repositorio.PlanosFiltrados(obra,idTipoPlano,nombre,fechaInicio,fechaFin);
            ViewBag.IdObra = idObra;
            ViewBag.TiposdePlano = Repositorio.BuscarTiposPlanos();
            return View(planosFiltrados);
        }


        // GET: PlanoController/Create
        public ActionResult Agregar(int idObra)
        {
            ViewBag.IdObra = idObra;
            ViewBag.TiposdePlano = Repositorio.BuscarTiposPlanos();
            return View();
        }

        // POST: PlanoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Plano aIngresar)
        {
            try
            {
                ViewBag.IdObra = aIngresar.IdObra;
                ViewBag.TiposdePlano = Repositorio.BuscarTiposPlanos();
                Repositorio.Agregar(aIngresar);
                return RedirectToAction("Index", new { idObra = aIngresar.IdObra });
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index", new { idObra = aIngresar.IdObra });
            }
        }

        // GET: PlanoController/Delete/5
        public ActionResult Eliminar(int id)
        {
            Plano plano = Repositorio.Buscar(id);
            return View(plano);
        }

        // POST: PlanoController/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            try
            {
                Plano planoABorrar = Repositorio.Buscar(id);
                Repositorio.Eliminar(planoABorrar);
                return RedirectToAction("Index", new { idObra = planoABorrar.IdObra });
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }
    }
}
