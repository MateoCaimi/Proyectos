using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Graph.Models;
using MVC.Models;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;

namespace MVC.Controllers
{
    public class EmpleadoController : Controller
    {

        private Fachada Fachada = new Fachada();

        // GET: EmpleadoController
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
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
            {
                return RedirectToAction("Index", "Plano");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario obra")
            {
                return RedirectToAction("Index", "Home");
            }

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
            ViewBag.Dias = marcas.Count();
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

            Empleado empleado = Fachada.BuscarEmpleado(id);
            return View(empleado);
        }

        // GET: EmpleadoController/Create
        public ActionResult Agregar()
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

            IEnumerable<TipoEmpleado> tipos = Fachada.BuscarTiposEmpleados();
            ViewBag.Tipos = tipos;
            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Empleado empleado)
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
                Fachada.AgregarEmpleado(empleado);
                return RedirectToAction(nameof(Index));
            }
            catch (EmpleadoException ee)
            {
                IEnumerable<TipoEmpleado> tipos = Fachada.BuscarTiposEmpleados();
                ViewBag.Tipos = tipos;
                ViewBag.Error = ee.Message;
                return View();
            }
        }

        // GET: EmpleadoController/Create
        public ActionResult AgregarTipo()
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

            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarTipo(TipoEmpleado tipo)
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
                Fachada.ModificarEmpleado(nuevoEmpleado);
                return RedirectToAction(nameof(Index));
            }
            catch (EmpleadoException ee)
            {
                Empleado empleado = Fachada.BuscarEmpleado(nuevoEmpleado.Id);

                IEnumerable<TipoEmpleado> tipos = Fachada.BuscarTiposEmpleados();
                ViewBag.Tipos = tipos;
                ViewBag.Error = ee.Message;
                return View(empleado);
            }
        }

        // GET: EmpleadoController/Delete/5
        public ActionResult Liquidar()
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
                if (desde.Year == 0001 || hasta.Year == 0001)
                {
                    throw new Exception("Seleccione fechas");
                }
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
                ViewBag.TotalLiquidacion = Fachada.TotalLiquidacion(vm);
                
                return View(vm);
            }
            catch (Exception e)
            {
                ViewBag.Obras = Fachada.TomarTodasObras();
                ViewBag.Empleados = Fachada.TomarTodosEmpleados();
                ViewBag.Error = e.Message;
                List<ObraEmpleadoLiquidacionViewModel> vm = new List<ObraEmpleadoLiquidacionViewModel>();
                return View(vm);
            }
        }

        public ActionResult CambiarListadoLiquidacion(int idObra)
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
                Fachada.ModificarTipo(nuevoTipoEmpleado);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DescargarPDF(List<ObraEmpleadoLiquidacionViewModel> ObraEmpleadoLiquidacionViewModelList)
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
                
           

            

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fontTitle = new XFont("Verdana", 20, XFontStyleEx.Bold);
            XFont fontLineas = new XFont("Verdana", 10, XFontStyleEx.Regular);
            XFont fontHeader = new XFont("Verdana", 15, XFontStyleEx.Italic);
            XFont fontFooter = new XFont("Verdana", 12, XFontStyleEx.BoldItalic);


            double margin = 40;
            double yPos = margin;
            double lineHeight = 20;

            // Título
            gfx.DrawString("Liquidacion", fontTitle, XBrushes.Black,
                new XRect(0, yPos, page.Width, page.Height),
                XStringFormat.TopCenter);

            yPos += 30;  // Espacio después del título

            // Subtítulo
            gfx.DrawString("Bodega & Piedrafita Arquitectos", fontHeader, XBrushes.Black,
                new XRect(0, yPos, page.Width, page.Height),
                XStringFormat.TopCenter);

            yPos += 20;  // Espacio después del subtítulo

            // Línea horizontal
            XPen line = new XPen(XColors.Black, 1);
            gfx.DrawLine(line, margin, yPos, page.Width - margin, yPos);

            yPos += 20;  // Espacio después de la línea

            // Detalles de los materiales
            int num = 0;
            foreach (ObraEmpleadoLiquidacionViewModel oel in ObraEmpleadoLiquidacionViewModelList)
            {
                num++;
                gfx.DrawString($"{num}- Nombre: {oel.ObraEmpleado.Empleado.Nombre} Cedula: {oel.ObraEmpleado.Empleado.Cedula} ", fontLineas, XBrushes.Black,
                    new XRect(margin, yPos, page.Width - 2 * margin, page.Height),
                    XStringFormat.TopLeft);

                yPos += 20;

                gfx.DrawString($"Banco:{oel.ObraEmpleado.Empleado.Banco} Cuenta:{oel.ObraEmpleado.Empleado.CuentaBanco} Total liquidacion: {oel.Liquidacion}", fontLineas, XBrushes.Black,
                  new XRect(margin + 40, yPos, page.Width - 2 * margin, page.Height),
                  XStringFormat.TopLeft);

                yPos += 30;

                gfx.DrawLine(line, margin, yPos, page.Width - margin, yPos);

                yPos += lineHeight + 10;

                if (yPos + lineHeight > page.Height - margin)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPos = margin;
                }

            }

            // Línea horizontal
            gfx.DrawLine(line, margin, yPos, page.Width - margin, yPos);

            yPos += 10;  // Espacio después de la línea



            double footerHeight = 50;  // Altura del pie de página
            double footerYPos = page.Height - margin - footerHeight;

            gfx.DrawLine(new XPen(XColors.Black, 1), margin, footerYPos, page.Width - margin, footerYPos); // Línea superior del pie de página



            //// Pie de página

            gfx.DrawString("Teléfono: 2600 1150", fontFooter, XBrushes.Gray,
                new XRect(0, footerYPos + 5, page.Width, footerHeight / 2),
                XStringFormat.Center);

            gfx.DrawString("Dirección: Formentor 7096", fontFooter, XBrushes.Gray,
                new XRect(0, footerYPos + 20, page.Width, footerHeight / 2),
                XStringFormat.Center);

            gfx.DrawString("Bodega & Piedrafita Arquitectos", fontFooter, XBrushes.Gray,
                new XRect(0, footerYPos + 35, page.Width, footerHeight / 2),
                XStringFormat.Center);



            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream);
                return File(stream.ToArray(), "application/pdf", "Liquidacion.pdf");

            }


        }


    }
}
