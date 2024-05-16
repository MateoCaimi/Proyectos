using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MVC.Controllers
{
    public class ObraController : Controller
    {
        private IRepositorioObra Repositorio = new RepositorioObra();
        // GET: ObraController
        public ActionResult Index()
        {
            IEnumerable<Obra> obras = Repositorio.TomarTodos();
            return View(obras);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string nombre, string direccion, bool finalizada)
        {

            IEnumerable<Obra> listadoObras = Repositorio.ObrasFiltradas(nombre, direccion, finalizada);
            return View(listadoObras);
        }

        // GET: ObraController/Details/5
        public ActionResult Detalles(int id)
        {
            Obra obra = Repositorio.Buscar(id);
            return View(obra);
        }

        // GET: ObraController/Create
        public ActionResult Agregar()
        {
            return View();
        }

        // POST: ObraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Obra aIngresar)
        {
            try
            {
                Repositorio.Agregar(aIngresar);
                return RedirectToAction(nameof(Index));
            }
            catch(ObraException e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        // GET: ObraController/Edit/5
        public ActionResult Modificar(int id)
        {
            return View();
        }

        // POST: ObraController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modificar(int id, Obra nuevaObra)
        {
            try
            {
                Repositorio.Modificar(id, nuevaObra);
                return RedirectToAction(nameof(Index));
            }
            catch(ObraException e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        // GET: ObraController/Delete/5
        public ActionResult Eliminar(int id)
        {
            Obra obra = Repositorio.Buscar(id);
            return View(obra);
        }

        // POST: ObraController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Obra obra)
        {
            try
            {
                Repositorio.Eliminar(obra);
                return RedirectToAction(nameof(Index));
            }
            catch (ObraException e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        public ActionResult Planos(int id)
        {
            IEnumerable<Plano> planos = Repositorio.PlanosTotales(id);
            if (planos == null)
            {

                planos = new List<LogicaNegocio.Entidades.Plano>();
            }

            return View(planos);
        }
    }
}
