using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MVC.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
namespace MVC.Controllers
{
    public class ObraController : Controller
    {
        private Fachada Fachada = new Fachada();
        // GET: ObraController
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

            if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de oficina")
            {

                IEnumerable<Obra> obras = Fachada.TomarTodasObras();
                return View(obras);
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de obra")
            {
                string nomUsuarioObra = HttpContext.Session.GetString("UsuarioLogueado");
                IEnumerable<Obra> obrasDeUsuarioObra = Fachada.TomarObrasDeUnUsuarioObra(nomUsuarioObra);
                return View(obrasDeUsuarioObra);


            }
            else return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string nombre, string direccion, bool finalizada)
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




            IEnumerable<Obra> listadoObras = Fachada.ObrasFiltradas(nombre, direccion, finalizada);
            return View(listadoObras);
        }
        public ActionResult Empleados(int IdObra)
        {
            Obra obra = Fachada.BuscarObra(IdObra);
            IEnumerable<ObraEmpleado> empleados = Fachada.GetEmpleadosObra(obra);
            return View(empleados);
        }

        //public ActionResult AgregarEmpleado()
        //{
        //    IEnumerable<Obra> obra = Fachada.TomarTodasObras();
        //    IEnumerable<Empleado> empleados = Fachada.TomarTodosEmpleados();
        //    ViewBag.Obras = obra;
        //    ViewBag.Empleados = empleados;
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AgregarEmpleado(ObraEmpleado oe)
        //{
        //    try
        //    {
        //        Fachada.AgregarEmpleadoAObra(oe);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception e)
        //    {
        //        IEnumerable<Obra> obra = Fachada.TomarTodasObras();
        //        IEnumerable<Empleado> empleados = Fachada.TomarTodosEmpleados();
        //        ViewBag.Obras = obra;
        //        ViewBag.Empleados = empleados;
        //        ViewBag.Error = e.Message;
        //        return View();
        //    }
        //}

        // GET: ObraController/Details/5
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
            //   detalles de obra solo ve el u de ofi o tambien de obra

            try
            {
                ObraEstadisticasViewModel obra = new ObraEstadisticasViewModel();
                obra.Obra = Fachada.BuscarObra(id);
                if (obra.Obra.Finalizada) //Yo sé que esto parece una locura.Usa un viewmodel para pasar todo de una a la vista y no usar muchos viewbags o tempdata, queda feo pero creo que es mejor.
                {
                    obra.MaterialMasSolicitado = Fachada.MaterialMasSolicitado(obra.Obra.IdObra);
                    obra.MaterialMenosSolicitado = Fachada.MaterialMenosSolicitado(obra.Obra.IdObra);
                    obra.AprobadorMasComun = Fachada.AprobadorMasComun(obra.Obra.IdObra);
                    obra.ProveedorMasComun = Fachada.ProveedorMasComun(obra.Obra.IdObra);
                }
                return View(obra);
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        // GET: ObraController/Create
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Usuarios = Fachada.ObtenerUsuariosDeObra();
            return View();
        }

        // POST: ObraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Agregar(Obra aIngresar, IFormFile archivoImagen)
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {

                if (aIngresar == null || archivoImagen != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivoImagen.CopyToAsync(memoryStream);
                        aIngresar.NombreCronograma = archivoImagen.FileName;
                        aIngresar.TipoCronograma = archivoImagen.ContentType;
                        aIngresar.Cronograma = memoryStream.ToArray();
                    }
                }
                Fachada.AgregarObra(aIngresar);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Usuarios = Fachada.ObtenerUsuariosDeObra();
                ViewBag.Error = e.Message;
                return View();
            }
        }

        // GET: ObraController/Edit/5
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }
           

            try
            {
                Obra obra = Fachada.BuscarObra(id);
                ViewBag.Usuarios = Fachada.ObtenerUsuariosDeObra();
                return View(obra);
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        // POST: ObraController/Edit/5
        [HttpPost, ActionName("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarConfirmado(Obra nuevaObra, IFormFile archivoImagen)
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }


            try
            {
                if (nuevaObra != null)
                {
                    if (archivoImagen != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await archivoImagen.CopyToAsync(memoryStream);
                            nuevaObra.NombreCronograma = archivoImagen.FileName;
                            nuevaObra.TipoCronograma = archivoImagen.ContentType;
                            nuevaObra.Cronograma = memoryStream.ToArray();
                        }
                    }
                }
                else
                {
                    throw new ObraException("No se encuentra la obra a modificar");
                }
               
                ViewBag.Usuarios = Fachada.ObtenerUsuariosDeObra();
                Fachada.ModificarObra(nuevaObra);
                return RedirectToAction(nameof(Index));
            }
            catch (ObraException e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        //// GET: ObraController/Delete/5
        //public ActionResult Eliminar(int id)
        //{

        //    if (HttpContext.Session.GetString("UsuarioLogueado") == null)
        //    {
        //        return RedirectToAction("Index", "Usuario");
        //    }
        //    else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
        //    {
        //        return RedirectToAction("Listado", "Usuario");
        //    }
        //    else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
        //    {
        //        return RedirectToAction("Index", "Plano");
        //    }
        //    else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }



        //    try
        //    {
        //        Obra obra = Fachada.BuscarObra(id);
        //        return View(obra);
        //    }
        //    catch (ObraException e)
        //    {
        //        ErrorViewModel errorModel = new ErrorViewModel();
        //        errorModel.RequestId = e.Message;
        //        return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
        //    }
        //}

        //// POST: ObraController/Delete/5
        //[HttpPost, ActionName("Eliminar")]
        //[ValidateAntiForgeryToken]
        //public ActionResult EliminarConfirmado(int id)
        //{

        //    if (HttpContext.Session.GetString("UsuarioLogueado") == null)
        //    {
        //        return RedirectToAction("Index", "Usuario");
        //    }
        //    else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
        //    {
        //        return RedirectToAction("Listado", "Usuario");
        //    }
        //    else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario normal")
        //    {
        //        return RedirectToAction("Index", "Plano");
        //    }
        //    else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }


        //    try
        //    {
        //        Obra obraABorrar = Fachada.BuscarObra(id);
        //        Fachada.EliminarObra(obraABorrar);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (ObraException e)
        //    {
        //        ErrorViewModel errorModel = new ErrorViewModel();
        //        errorModel.RequestId = e.Message;
        //        return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
        //    }
        //}

        // GET: ObraController/Cerrar/5
        public ActionResult Cerrar(int id)
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Obra obra = Fachada.BuscarObra(id);
                return View(obra);
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        // POST: ObraController/Delete/5
        [HttpPost, ActionName("Cerrar")]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarConfirmado(int id)
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }

            Obra obraACerrar = Fachada.BuscarObra(id);
            try
            {
                Fachada.FinalizarObra(obraACerrar);
                return RedirectToAction(nameof(Index));
            }
            catch (ObraException e)
            {
                ViewBag.Error = e.Message;
                return View(obraACerrar);
            }
        }

        [HttpGet("{id}/cronograma")]
        public IActionResult ObtenerCronograma(int id)
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


            Obra obra = Fachada.BuscarObra(id);

            if (obra == null || obra.Cronograma == null)
            {
                return NotFound();
            }

            return File(obra.Cronograma, obra.TipoCronograma, obra.NombreCronograma);
        }

        public IActionResult Materiales(int id)
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
            Obra obra = Fachada.BuscarObra(id);

            if (obra == null)
            {
                return NotFound();
            }

            IEnumerable<ObraMaterial> materialesObra = Fachada.MaterialesDeObra(id);
            ViewBag.idObra = id;
            return View(materialesObra);
        }


        public async Task<IActionResult> ObtenerQr(int id)
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }




            string data = $"https://localhost:7289/Plano?idObra={id}"; //Se cambia el link una vez hecho el deploy, pero la idea sería esta.
            string url = $"https://api.qrserver.com/v1/create-qr-code/?size=1000x1000&qzone=30&data={data}";

            using (HttpClient cliente = new HttpClient())
            {
                HttpResponseMessage respuesta;
                try
                {
                    respuesta = await cliente.GetAsync(url);
                }
                catch (Exception ex)
                {
                    // Maneja errores de conexión
                    return StatusCode(500, $"Error al conectar con la API: {ex.Message}");
                }

                if (respuesta.IsSuccessStatusCode)
                {
                    byte[] qrCodeImage = await respuesta.Content.ReadAsByteArrayAsync();
                    Obra obra = Fachada.BuscarObra(id);
                    obra.QR = qrCodeImage;
                    return Qr(id);
                }
                else
                {
                    // Maneja errores de la API
                    return StatusCode((int)respuesta.StatusCode, "Error al generar el código QR");
                }
            }


        }
            [HttpGet("{id}/Qr")]
            public IActionResult Qr(int id)
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
            else if (HttpContext.Session.GetString("UsuarioTipo") != "Usuario de oficina")
            {
                return RedirectToAction("Index", "Home");
            }



            Obra obra = Fachada.BuscarObra(id);

                if (obra == null || obra.QR == null)
                {
                    return NotFound();
                }
                MemoryStream stream = new MemoryStream(obra.QR);
                Bitmap bitmap = new Bitmap(stream);
                Bitmap tempBitmap = new Bitmap(bitmap.Width, bitmap.Height); //Se crea uno vacío y se dibuja sobre ese.
                Graphics graphics = Graphics.FromImage(tempBitmap);
                {
                // Draw the original bitmap onto the graphics of the new bitmap
                    graphics.DrawImage(bitmap, 0, 0);
                }
                Font arial = new Font("Arial", 50, FontStyle.Regular);
                Brush brush = new SolidBrush(Color.Black);
                string text = "Obra: " + obra.Nombre;
                Rectangle rectangle = new Rectangle(0, 0, 1000, 200);
                Pen pen = new Pen(Color.White, 2);
                graphics.DrawRectangle(pen, rectangle);
                graphics.DrawString(text, arial, brush, rectangle);
                //tempBitmap.Save("C:\\Users\\user\\Desktop\\image.png");
                ImageConverter converter = new ImageConverter();

            return File((byte[])converter.ConvertTo(tempBitmap, typeof(byte[])), "image/png", obra.Nombre + " - QR.png");
            }


        // GET: ObraController/Consumo
        public ActionResult Consumo(int idObra)
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

            try
            {
                TempData["ListaActualConsumo"] = null;
                if (!Fachada.BuscarObra(idObra).Finalizada)
                {
                    IEnumerable<ObraMaterial> materiales = Fachada.MaterialesDeObra(idObra);
                    ViewBag.Materiales = materiales;
                    ViewBag.IdObra = idObra;
                    ViewBag.NombreObra = Fachada.BuscarObra(idObra).Nombre;

                    List<MaterialConsumoViewModel> tempMaterials = new List<MaterialConsumoViewModel>();
                    return View(tempMaterials);
                }
                else
                {
                    ViewBag.Error = "Esta obra está cerrada, no se pueden consumir materiales";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.IdObra = idObra;
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: ObraController/Consumo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConsumoPost(int IdObra)
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

            try
            {
                if(TempData["ListaActualConsumo"] == null)
                {
                    throw new ObraException("No se pudo consumir ningun material");
                }
                List<MaterialConsumoViewModel> item = JsonConvert.DeserializeObject<List<MaterialConsumoViewModel>>((string)TempData["ListaActualConsumo"]);
                Obra obra = Fachada.BuscarObra(IdObra);
                if(Fachada.MaterialesCheckStock(item, obra))
                {
                    throw new ObraException("No se puede consumir más que el stock de un material en específico");
                }
                Fachada.ConsumirMateriales(item, obra);
                List<ObraMaterial> materialesAlertar;
                if (HttpContext.Session.GetString("UsuarioTipo") == "UDeObra")
                {
                    materialesAlertar = Fachada.AlertarStockDeMaterialesTodasObrasACargo(HttpContext.Session.GetString("UsuarioLogueado"));
                }
                else
                {
                    materialesAlertar = Fachada.AlertarStockDeMaterialesTodasObras();
                }
                if (materialesAlertar.Count > 0)
                {
                    var opciones = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        WriteIndented = true,

                    };
                    HttpContext.Session.SetString("MaterialesAlertar", System.Text.Json.JsonSerializer.Serialize(materialesAlertar, opciones)); //Uso el distinct para no repetir alertas. Ej: se baja de la barrera, y se consume de vuelta
                }
                ViewBag.Mensaje ="Materiales consumidos con exito";
                return RedirectToAction("Consumo", new { idObra = IdObra });
                
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Consumo", new { idObra = IdObra });
            }
        }

        [HttpPost]
        public ActionResult Consumir(MaterialConsumoViewModel consumo)
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

            try
            {
                List<MaterialConsumoViewModel> lista;
                if (TempData["ListaActualConsumo"] != null)
                {
                    lista = JsonConvert.DeserializeObject<List<MaterialConsumoViewModel>>((string)TempData["ListaActualConsumo"]);
                }
                else
                {
                    lista = new List<MaterialConsumoViewModel>();
                }

                Material material = Fachada.BuscarMaterial(consumo.IdMaterial);
                consumo.Material = material;

                var existingMaterial = lista.FirstOrDefault(mc => mc.IdMaterial == consumo.IdMaterial);
                if (existingMaterial != null)
                {
                    existingMaterial.Cantidad += consumo.Cantidad;
                }
                else
                {
                    lista.Add(consumo);
                }

                TempData["ListaActualConsumo"] = JsonConvert.SerializeObject(lista);
                ViewBag.Materiales = Fachada.TodosLosMateriales();

                return PartialView("ListaConsumos", lista);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult HorasLluvia(int IdObra)
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

            try
            {
                Obra obra = Fachada.BuscarObra(IdObra);
                return View(obra);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult HorasLluvia(int IdObra, int horasLluvia, DateTime dia)
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


            try
            {
                Obra obra = Fachada.BuscarObra(IdObra);
                Fachada.AsignacionHoras(obra, horasLluvia, dia);
                ViewBag.Mensaje = "Horas asignadas correctamente.";
                return View(obra);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Obra obra = Fachada.BuscarObra(IdObra);
                return View();
            }
        }

    }
}