using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public ActionResult Agregar(string nombre, string nombreUsuario, string contrasenia, string tipo)
        {

            try
            {
                Usuario u = Fachada.CastearUsuario(nombre, nombreUsuario, contrasenia, tipo);

                Fachada.AgregarUsuario(u);
                return RedirectToAction(nameof(Listado));
            }

            //Error invalid column name tipo
            catch(Exception e)
            {
                var subclassTypes = Assembly
            .GetAssembly(typeof(Usuario))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Usuario)));
                ViewBag.TipoUsuario = subclassTypes;
                ViewBag.Error = e.Message;
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        //public ActionResult Editar(int id)
        //{
        //    Usuario usuario = Fachada.BuscarUsuario(id);
        //    return View(usuario);
        //}

        //// POST: UsuarioController/Edit/5
        //[HttpPost, ActionName("Editar")]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditarConfirmado(int id, string nombre, string nombreUsuario, string contrasenia, string tipo)
        //{
        //    try
        //    {
        //        Usuario nuevoUsuario = Fachada.CastearUsuario(nombre, nombreUsuario, contrasenia, tipo);
        //        nuevoUsuario.Id = id;
        //        Fachada.ModificarUsuario(nuevoUsuario);
        //        return RedirectToAction(nameof(Listado));
        //    }
        //    catch(UsuarioException ue)
        //    {
        //        ViewBag.Error(ue.Message);
        //        return RedirectToAction(nameof(Listado));
        //    }
        //}

        // GET: UsuarioController/Delete/5
        public ActionResult Eliminar(int id)
        {
            try
            {
            Usuario usuario = Fachada.BuscarUsuario(id);
            return View(usuario);

            }
            catch (UsuarioException ue)
            {
                ViewBag.Error(ue.Message);
                return RedirectToAction(nameof(Listado));
            }
        }

        // POST: UsuarioController/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        { 
            try
            {
                Usuario u = Fachada.BuscarUsuario(id);
                Fachada.EliminarUsuario(u);
                return RedirectToAction(nameof(Listado));
            }
            catch(UsuarioException ue)
            {
                ViewBag.Error = ue.Message;
                return RedirectToAction(nameof(Listado));
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
