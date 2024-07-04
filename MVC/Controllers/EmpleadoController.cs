using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            
            //Liquidar();
            //Fachada.AgregarEmpleadosAObra();
            ViewBag.Obras = Fachada.TomarTodasObras();
            return View(empleados);
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
            return View(marcas);
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

        public ActionResult Egreso(int IdEmpleado, int IdObra)
        {
            ObraEmpleado obraEmpleado = Fachada.BuscarEmpleadoObra(IdEmpleado, IdObra);
            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Egreso(ObraEmpleado obraEmpleado, DateTime dia)
        {
            try
            {
                Fachada.DarEgreso(obraEmpleado, dia);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        //// GET: EmpleadoController/Create
        //public ActionResult AgregarTipo()
        //{
        //    return View();
        //}

        //// POST: EmpleadoController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AgregarTipo(TipoEmpleado tipo)
        //{
        //    try
        //    {
        //        if(tipo == null)
        //        {
        //            throw new EmpleadoException("El tipo no puede ser nulo");
        //        }
        //        Fachada.AgregarTipoEmpleado(tipo);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = ex.Message;
        //        return View();
        //    }
        //}

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
            ViewBag.Obras = Fachada.TomarTodasObras();
            ViewBag.Empleados = Fachada.TomarTodosEmpleados();
            return View();
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
                //Task<string> responseData = await Fachada.LlamadaClodtimes(desde, hasta);
                //RespuestaApiModel datosApi = JsonConvert.DeserializeObject<RespuestaApiModel>(await responseData);
                //Fachada.AgregarEmpleadosAObra();
                //ObraEmpleado obraEmpleado = Fachada.BuscarEmpleadoObra(2, 2);
                //Fachada.conseguirMarcasEmpleado(obraEmpleado);
                Empleado empleado = Fachada.BuscarEmpleado(IdEmpleado);
                Obra obra = Fachada.BuscarObra(IdObra);
                Fachada.Liquidar(desde,hasta,obra,empleado);
                return Ok();
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
