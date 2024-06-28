using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class EmpleadoController : Controller
    {

        private Fachada Fachada = new Fachada();

        // GET: EmpleadoController
        public ActionResult Index()
        {
            IEnumerable<Empleado> empleados = Fachada.TomarTodosEmpleados();
            //Liquidar();
            return View(empleados);
        }

        // GET: EmpleadoController/Details/5
        public ActionResult Detalles(int id)
        {
            Empleado empleado = Fachada.BuscarEmpleado(id);
            return View(empleado);
        }

        // GET: EmpleadoController/Create
        public ActionResult Agregar()
        {
            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(IFormCollection collection)
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

        // GET: EmpleadoController/Edit/5
        public ActionResult Editar(int id)
        {
            Empleado empleado = Fachada.BuscarEmpleado(id);
            return View(empleado);
        }

        // POST: EmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Empleado nuevoEmpleado)
        {
            try
            {
                Fachada.ModificarEmpleado(nuevoEmpleado);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmpleadoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmpleadoController/Delete/5
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

        public async Task<IActionResult> Liquidar()
        {
            try
            {
                var responseData = await Fachada.Liquidar();
                
                return Ok(responseData);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, e.Message);
            }
        }


    }
}
