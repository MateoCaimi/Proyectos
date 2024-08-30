using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class MaterialController : Controller
    {

        private Fachada Fachada = new Fachada();

        public ActionResult Index()
        {
            Fachada.Precarga();
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
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Material> materiales = Fachada.TomarTodosMateriales();
            return View(materiales);

        }



        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult IndexFiltrado(string nombre)
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
                return RedirectToAction("Index", "Home");
            }

            try
            {
                IEnumerable<Material> materialesFiltrados = Fachada.MaterialesFiltrados(nombre);
                return View(materialesFiltrados);

            }
            catch (MaterialException me)
            {
                ViewBag.Error = me.Message;
                IEnumerable<Material> materiales = Fachada.TomarTodosMateriales();
                return View(materiales);
            }
        }


        // GET: MaterialController/Create
        public ActionResult Agregar()
        {
            return View();
        }

        // POST: MaterialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Material nuevoMaterial)
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
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Fachada.AgregarMaterial(nuevoMaterial);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
        }

        // GET: MaterialController/Edit/5
        public ActionResult Editar(int id)
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
                return RedirectToAction("Index", "Home");
            }

            Material material = Fachada.BuscarMaterial(id);
            return View(material);
        }

        // POST: MaterialController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Material material)
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
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Fachada.ModificarMaterial(material);
                return RedirectToAction(nameof(Index));
            }
            catch (MaterialException me)
            {
                ViewBag.Error = me.Message;
                return View();
            }
        }
    }
}
