using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace MVC.Controllers
{
    public class UsuarioController : Controller
    {

        private Fachada Fachada = new Fachada();
        // GET: UsuarioController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario u)
        {
            try
            {
                Fachada.InicioSesion(u);
                HttpContext.Session.SetString("UsuarioLogueado", u.NombreUsuario);
                //Me deberia traer tipo usuario para ver que cosas mostrar

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }

            return RedirectToAction("Index", "Obra");
        }


        public ActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("UsuarioLogueado");


            return RedirectToAction("Index", "Usuario");
        }


        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Agregar()
        {
            var subclassTypes = Assembly
            .GetAssembly(typeof(Usuario))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Usuario)));
              ViewBag.TipoUsuario = subclassTypes;
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Usuario u)
        {

            if (u == null)
            {
                return BadRequest();
            }

            try
            {

                //Falta hacer new del usuario especificado en el tipo con un switch o varios if

                Fachada.AgregarUsuario(u);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
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

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Usuario u)
        {
            if (u == null)
            {
                return BadRequest();
            }

            try
            {
                Fachada.EliminarUsuario(u);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Listado/5
        public ActionResult Listado()
        {
            IEnumerable<Usuario> usuarios = Fachada.ObtenerUsuarios();
            return View(usuarios);
        }
    }
}
