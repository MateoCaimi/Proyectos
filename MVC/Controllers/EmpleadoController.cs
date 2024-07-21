using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using MVC.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MVC.Controllers
{
    public class EmpleadoController : Controller
    {

        private Fachada Fachada = new Fachada();

        // GET: EmpleadoController
        public ActionResult Index()
        {
            IEnumerable<Empleado> empleados = Fachada.TomarTodosEmpleados();
            ViewBag.Obras = Fachada.TomarTodasObras();
            try
            {
                Fachada.Precarga();
                // aca es 0 el dia asi q se precarga la ultima semana

                //Fachada.AgregarTodasLasMarcasPorIdEmpleado(44);
                //Liquidar();
                //Fachada.AgregarEmpleadosAObra();

                
                return View(empleados);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View(empleados);
            }
            
        }

        // GET: EmpleadoController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int IdObra)
        {
            IEnumerable<Empleado> empleados;
            if (IdObra != 0)
            {
                Obra obra = Fachada.BuscarObra(IdObra);
                empleados = Fachada.TomarEmpleadosDeObra(obra);
            }
            else
            {
                empleados = Fachada.TomarTodosEmpleados();
            }
            ViewBag.Obras = Fachada.TomarTodasObras();
            return View(empleados);
        }

        // GET: EmpleadoController
        public ActionResult ListaTipos()
        {


            IEnumerable<TipoEmpleado> tipoEmpleados = Fachada.TomarTodosTipoEmpleados();
            //Fachada.AgregarEmpleadosAObraDTO();
            //Fachada.AgregarTodasLasMarcasPorIdEmpleado(44);
            //Fachada.AgregarTodasLasMarcasDTO();
            //Liquidar();
            //Fachada.AgregarEmpleadosAObra();
            return View(tipoEmpleados);
        }

        public ActionResult Marcas(int id)
        {
            ViewBag.Obras = Fachada.TomarTodasObras();
            Empleado empleado = Fachada.BuscarEmpleado(id);
            IEnumerable<Marca> marcasEmp = new List<Marca>(); //No mostrar nada hasta filtrar. Son muchos registros.
            ViewBag.IdEmp = empleado.Id;
            return View(marcasEmp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Marcas(int id, int IdObra, DateTime desde, DateTime hasta)
        {
            Empleado empleado = Fachada.BuscarEmpleado(id);
            List<Marca> marcas = Fachada.TraerTodasMarcas(empleado);
            if (IdObra != 0)
            {
                Obra obra = Fachada.BuscarObra(IdObra);
                marcas = Fachada.MarcasDelEmpleadoEnLaObra(marcas, obra);
            }
            if(desde.Year != 0001 && hasta.Year != 0001)
            {
                marcas = Fachada.MarcasDelRangoDeFechas(marcas, desde, hasta);
            }
            ViewBag.Obras = Fachada.TomarTodasObras();
            ViewBag.IdEmp = empleado.Id;
            ViewBag.HorasTotales = Fachada.HorasTotales(marcas);
            return View(marcas);
        }

        public ActionResult GenerarMarcas()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarMarcas(DateTime desde, DateTime hasta)
        {
            try
            {
                Fachada.Precarga();
                TempData["Anomalia"] = "";
                if (!Fachada.AgregarEmpleadosAObraDTO(desde, hasta))
                {
                    TempData["Anomalia"] = "Se encontraron anomalías generando a los empleados en obra en el sistema. Revisar las marcas del rango especificado.";
                }

                if (!Fachada.AgregarTodasLasMarcasDTO(desde, hasta))
                {
                    TempData["Anomalia"] = "Se generaron marcas anómalas en el sistema. Revisar las marcas del rango especificado.";
                }
                return View();
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
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
            IEnumerable<TipoEmpleado> tipos = Fachada.BuscarTiposEmpleados();
            ViewBag.Tipos = tipos;
            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Empleado empleado)
        {

            //if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            //{
            //    return RedirectToAction("Index", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Administrador")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Obra")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}

            try
            {
                Fachada.AgregarEmpleado(empleado);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                IEnumerable<TipoEmpleado> tipos = Fachada.BuscarTiposEmpleados();
                ViewBag.Tipos = tipos;
                ViewBag.Error = e.Message;
                return View();
            }
        }

        // GET: EmpleadoController/Create
        public ActionResult AgregarTipo()
        {
            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarTipo(TipoEmpleado tipo)
        {
            try
            {
                if(tipo == null)
                {
                    throw new EmpleadoException("El tipo no puede ser nulo");
                }
                Fachada.AgregarTipoEmpleado(tipo);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: EmpleadoController/Edit/5
        public ActionResult Editar(int id)
        {
            Empleado empleado = Fachada.BuscarEmpleado(id);
            IEnumerable<TipoEmpleado> tipos = Fachada.BuscarTiposEmpleados();
            ViewBag.Tipos = tipos;
            return View(empleado);
        }

        // POST: EmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Empleado nuevoEmpleado)
        {
            //if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            //{
            //    return RedirectToAction("Index", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Administrador")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Obra")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}

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

            //if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            //{
            //    return RedirectToAction("Index", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Administrador")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Obra")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Liquidar()
        {

            DateTime desde = new DateTime();
            DateTime hasta = new DateTime();


            if (!Fachada.AgregarEmpleadosAObraDTO(desde, hasta))
            {
                TempData["Anomalia"] = "Se encontraron anomalías generando a los empleados en obra en el sistema. Revisar las marcas de la última semana.";
            }

            if (!Fachada.AgregarTodasLasMarcasDTO(desde, hasta))
            {
                TempData["Anomalia"] = "Se generaron marcas anómalas en el sistema. Revisar las marcas de la última semana.";
            }

            ViewBag.Obras = Fachada.TomarTodasObras();
            ViewBag.Empleados = Fachada.TomarTodosEmpleados();
            List<ObraEmpleadoLiquidacionViewModel> vm = new List<ObraEmpleadoLiquidacionViewModel>();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Liquidar(int IdObra, int IdEmpleado,DateTime desde, DateTime hasta)
        {
            //if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            //{
            //    return RedirectToAction("Index", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Administrador")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Obra")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}

            try
            {
                Empleado empleado = Fachada.BuscarEmpleado(IdEmpleado);
                Obra obra = Fachada.BuscarObra(IdObra);
                List<ObraEmpleadoLiquidacionViewModel> vm = new List<ObraEmpleadoLiquidacionViewModel>();
                Dictionary<ObraEmpleado, double> dic;
                dic = Fachada.Liquidar(desde, hasta, obra, empleado); 
                foreach(KeyValuePair<ObraEmpleado, double> kv in dic) //No es lógica de negocio, es formateo de vista, entonces entiendo que es válido.
                {
                    vm.Add(new ObraEmpleadoLiquidacionViewModel(kv.Key, kv.Value));
                }
                ViewBag.Obras = Fachada.TomarTodasObras();
                ViewBag.Empleados = Fachada.TomarTodosEmpleados();
                return View(vm);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        public ActionResult CambiarListadoLiquidacion(int idObra)
        {
            Obra obra = Fachada.BuscarObra(idObra);
            List<Empleado> empleadosObra = Fachada.TomarEmpleadosDeObra(obra);
            return PartialView("CambiarListadoLiquidacion", empleadosObra);
        }

        // GET: EmpleadoController/Edit/5
        public ActionResult ModificarTipo(int id)
        {
            TipoEmpleado tipoEmpleado = Fachada.BuscarTipo(id);
            return View(tipoEmpleado);
        }

        // POST: EmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarTipo(TipoEmpleado nuevoTipoEmpleado)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario Obra")
            {
                return RedirectToAction("Listado", "Usuario");
            }

            try
            {
                Fachada.ModificarTipo(nuevoTipoEmpleado);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
