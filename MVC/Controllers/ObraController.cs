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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ObraController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ObraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Obra aIngresar)
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ObraController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Obra nuevaObra)
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
        public ActionResult Delete(int id)
        {
            Obra obra = Repositorio.Buscar(id);
            return View(obra);
        }

        // POST: ObraController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Obra obra)
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
    }
}
