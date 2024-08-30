using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ObraEmpleadoController : Controller
    {
        private Fachada Fachada = new Fachada();

        // GET: ObraEmpleadoController/Details/5
        public ActionResult Detalles(int idEmpleado, int idObra)
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

            ObraEmpleado oEmp = Fachada.BuscarEmpleadoObra(idEmpleado, idObra);
            return View(oEmp);
        }

        // GET: ObraEmpleadoController/Edit/5
        public ActionResult Editar(int idEmpleado, int idObra)
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
            ObraEmpleado oEmp = Fachada.BuscarEmpleadoObra(idEmpleado, idObra);
            return View(oEmp);
        }

        // POST: ObraEmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(ObraEmpleado obraEmpleadoNuevo)
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
                Fachada.ModificarObraEmpleado(obraEmpleadoNuevo);
                return RedirectToAction("Empleados", "Obra", new { idObra = obraEmpleadoNuevo.IdObra });
            }
            catch
            {
                return View();
            }
        }
    }
}
