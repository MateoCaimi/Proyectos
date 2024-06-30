using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class MaterialController : Controller
    {

        private Fachada Fachada = new Fachada();
     
        public ActionResult Index()
        {

            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
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
            else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            {
                return RedirectToAction("Listado", "Usuario");
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
            else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }

            try
            {
                Fachada.AgregarMaterial(nuevoMaterial);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
        }

        // GET: MaterialController/Edit/5
        public ActionResult Editar(int id)
        {
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
            else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }

            try
            {
                Fachada.ModificarMaterial(material);
                return RedirectToAction(nameof(Index));
            }
            catch(MaterialException me)
            {
                ViewBag.Error = me.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MaterialController/Delete/5
        //public ActionResult Eliminar(int id)
        //{
        //    try
        //    {
        //        Material material = Fachada.BuscarMaterial(id);
        //        return View(material);

        //    }
        //    catch (Exception e)
        //    {
        //        ViewBag.Error = e.Message;
        //        return RedirectToAction("Index");
        //    }
                
        //}

        //// POST: MaterialController/Delete/5
        //[HttpPost, ActionName("Eliminar")]
        //[ValidateAntiForgeryToken]
        //public ActionResult EliminarConfirmado(int id)
        //{
        //    try
        //    {
        //        Material material = Fachada.BuscarMaterial(id);
        //        Fachada.EliminarMaterial(material);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch(MaterialException me)
        //    {
        //        ViewBag.Error = me.Message;
        //        return RedirectToAction("Index");
        //    }
        //}
    }
}
