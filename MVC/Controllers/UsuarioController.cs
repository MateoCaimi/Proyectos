using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC.Controllers
{

    public class UsuarioController : Controller
    {

        private Fachada Fachada = new Fachada();

        // GET: UsuarioController/LoginQR/1
        public ActionResult LoginQR(int id)
        {
            Fachada.Precarga();

            if(HttpContext.Session.GetString("UsuarioLogueado") != null)
            {
                return RedirectToAction("Index", "Plano", new {idObra = id});
            }
            ViewBag.IdObra = id;
            return View();
        }

        [HttpPost]
        public ActionResult LoginQR(int idObra, string NombreUsuario, string Contrasenia)
        {

            try
            {
                Usuario u = Fachada.InicioSesion(NombreUsuario, Contrasenia);
                if (HttpContext.Session.GetString("UsuarioLogueado") != null)
                {
                    return RedirectToAction("Index", "Plano", new { idObra = idObra });
                }
                if (u.CambioContrasenia)
                {
                    return RedirectToAction("CambiarPass", "Usuario", new { nomU = NombreUsuario });
                }


                HttpContext.Session.SetString("UsuarioLogueado", u.NombreUsuario);
                HttpContext.Session.SetString("UsuarioTipo", u.Tipo);
                Obra obra = Fachada.BuscarObra(idObra);
                if(obra != null)
                {
                    return RedirectToAction("Index", "Plano", new { idObra = idObra });
                }
                else
                {
                    return RedirectToAction("Index", "Obra");
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
            
        }

        // GET: UsuarioController
        public ActionResult Index()
        {
            Fachada.Precarga();

            if (HttpContext.Session.GetString("UsuarioLogueado") != null)
            {
                return RedirectToAction("Index", "Obra");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string NombreUsuario, string Contrasenia)
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") != null)
            {
                return RedirectToAction("Index", "Obra");
            }

            try
            {
                Usuario u = Fachada.InicioSesion(NombreUsuario, Contrasenia);

                if (u.CambioContrasenia)
                {
                    return RedirectToAction("CambiarPass", "Usuario", new { nomU = NombreUsuario });
                }


                HttpContext.Session.SetString("UsuarioLogueado", u.NombreUsuario);
                HttpContext.Session.SetString("UsuarioTipo", u.Tipo);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }


            return RedirectToAction("Index", "Home");
        }


        public ActionResult CambiarPass(string nomU)
        {
            ViewBag.nom = nomU;
            return View();
        }


        [HttpPost]
        public ActionResult CambiarPass(string NombreUsuario, string Contrasenia, string confirmarPass)
        {
            Fachada.CambiarPass(NombreUsuario, Contrasenia, confirmarPass);
            Usuario u = Fachada.BuscarUsuarioXNombreU(NombreUsuario);

            HttpContext.Session.SetString("UsuarioLogueado", u.NombreUsuario);
            HttpContext.Session.SetString("UsuarioTipo", u.Tipo);

            //Si es usuario normal no pasa
            return RedirectToAction("Index", "Obra");
        }


        public ActionResult CerrarSesion()
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            HttpContext.Session.Remove("UsuarioLogueado");
            HttpContext.Session.Remove("UsuarioTipo");

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


            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario administrador")
            {
                return RedirectToAction("Index", "Obra");
            }


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


            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario administrador")
            {
                return RedirectToAction("Index", "Obra");
            }


            try
            {

                Usuario u = Fachada.CastearUsuario(nombre, nombreUsuario, contrasenia, tipo);

                Fachada.AgregarUsuario(u);
                return RedirectToAction(nameof(Listado));
            }

            //Error invalid column name tipo
            catch (Exception e)
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

        //GET: UsuarioController/Edit/5
        public ActionResult Editar(int id)
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario administrador")
            {
                return RedirectToAction("Index", "Home");
            }


            Usuario usuario = Fachada.BuscarUsuario(id);
            var subclassTypes = Assembly
            .GetAssembly(typeof(Usuario))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Usuario)));
            ViewBag.TipoUsuario = subclassTypes;
            return View(usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost, ActionName("Editar")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarConfirmado(int id, string nombre, string nombreUsuario, string contrasenia, string tipo)
        {


            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario administrador")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Usuario nuevoUsuario = Fachada.CastearUsuario(nombre, nombreUsuario, contrasenia, tipo);
                nuevoUsuario.Id = id;
                Fachada.ModificarUsuario(nuevoUsuario);
                return RedirectToAction(nameof(Listado));
            }
            catch (UsuarioException ue)
            {
                var subclassTypes = Assembly
                .GetAssembly(typeof(Usuario))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Usuario)));
                ViewBag.TipoUsuario = subclassTypes;
                ViewBag.Error = ue.Message;
                ViewBag.pass = contrasenia;
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Eliminar(int id)
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario administrador")
            {
                return RedirectToAction("Index", "Home");
            }

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

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario administrador")
            {
                return RedirectToAction("Index", "Home");
            }


            try
            {
                Usuario u = Fachada.BuscarUsuario(id);
                Fachada.EliminarUsuario(u);
                return RedirectToAction(nameof(Listado));
            }
            catch (UsuarioException ue)
            {
                ViewBag.Error = ue.Message;
                return View();
            }
        }

        // GET: UsuarioController/Listado/5
        public ActionResult Listado()
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario administrador")
            {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<Usuario> usuarios = Fachada.ObtenerUsuarios();
            return View(usuarios);
        }
    }
}
