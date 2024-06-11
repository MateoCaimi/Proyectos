using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
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
    
        // GET: MaterialController/Details/5
        public ActionResult Detalles(int id)
        {
            //detalles de materiales?
            return View();
        }

        // GET: MaterialController/Create
        public ActionResult Agregar()
        {
            return View();
        }

        // POST: MaterialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(string Nombre, int Stock, string UnidadDeMedida)
        {
            try
            {
                Fachada.AgregarMaterial(Nombre, Stock, UnidadDeMedida);

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
        }

        // GET: MaterialController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MaterialController/Edit/5
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

        // GET: MaterialController/Delete/5
        public ActionResult Eliminar(int id)
        {
            try
            {

                Fachada.EliminarMaterial(id);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index");
            }
                
        }





        //este ni lo usamos
        // POST: MaterialController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
