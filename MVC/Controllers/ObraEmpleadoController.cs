using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ObraEmpleadoController : Controller
    {
        private Fachada Fachada = new Fachada();

        // GET: ObraEmpleadoController/Details/5
        public ActionResult Detalles(int idEmpleado, int idObra)
        {
            ObraEmpleado oEmp = Fachada.BuscarEmpleadoObra(idEmpleado, idObra);
            return View(oEmp);
        }

        // GET: ObraEmpleadoController/Edit/5
        public ActionResult Editar(int idEmpleado, int idObra)
        {
            ObraEmpleado oEmp = Fachada.BuscarEmpleadoObra(idEmpleado, idObra);
            return View(oEmp);
        }

        // POST: ObraEmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(ObraEmpleado obraEmpleadoNuevo)
        {
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
