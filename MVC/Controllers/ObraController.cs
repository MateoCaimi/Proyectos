using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Drawing;
namespace MVC.Controllers
{
    public class ObraController : Controller
    {
        private Fachada Fachada = new Fachada();
        // GET: ObraController
        public ActionResult Index()
        {
            IEnumerable<Obra> obras = Fachada.TomarTodasObras();
            return View(obras);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string nombre, string direccion, bool finalizada)
        {
            IEnumerable<Obra> listadoObras = Fachada.ObrasFiltradas(nombre, direccion, finalizada);
            return View(listadoObras);
        }

        // GET: ObraController/Details/5
        public ActionResult Detalles(int id)
        {
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
                //Qr(id);
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
            /*Usuario uDeObraTesting = new UDeObra("Federico Ruiz Estévez", "federuiz2729", "Pepepepe123");
            Fachada.AgregarUsuario(uDeObraTesting);*/
            ViewBag.Usuarios = Fachada.ObtenerUsuariosDeObra();
            return View();
        }

        // POST: ObraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Agregar(Obra aIngresar, IFormFile archivoImagen)
        {
            try
            {
                if (aIngresar == null || aIngresar.TipoCronograma != "application/pdf")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivoImagen.CopyToAsync(memoryStream);
                        aIngresar.NombreCronograma = archivoImagen.FileName;
                        aIngresar.TipoCronograma = archivoImagen.ContentType;
                        aIngresar.Cronograma = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["Error"] = "Debe proporcionar un archivo válido.";
                    return View();
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
            try
            {
                if (nuevaObra == null || nuevaObra.TipoCronograma != "application/pdf")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivoImagen.CopyToAsync(memoryStream);
                        nuevaObra.NombreCronograma = archivoImagen.FileName;
                        nuevaObra.TipoCronograma = archivoImagen.ContentType;
                        nuevaObra.Cronograma = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["Error"] = "Debe proporcionar un archivo válido.";
                    return View();
                }
                ViewBag.Usuarios = Fachada.ObtenerUsuariosDeObra();
                Fachada.ModificarObra(nuevaObra);
                return RedirectToAction(nameof(Index));
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        // GET: ObraController/Delete/5
        public ActionResult Eliminar(int id)
        {
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
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            try
            {
                Obra obraABorrar = Fachada.BuscarObra(id);
                Fachada.EliminarObra(obraABorrar);
                return RedirectToAction(nameof(Index));
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        // GET: ObraController/Cerrar/5
        public ActionResult Cerrar(int id)
        {
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
            Obra obra = Fachada.BuscarObra(id);

            if (obra == null || obra.Cronograma == null)
            {
                return NotFound();
            }

            return File(obra.Cronograma, obra.TipoCronograma, obra.NombreCronograma);
        }


        public async Task<IActionResult> ObtenerQr(int id)
        {
            string data = $"HOLAMUNDO"; 
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
                Obra obra = Fachada.BuscarObra(id);

                if (obra == null || obra.QR == null)
                {
                    return NotFound();
                }
                MemoryStream stream = new MemoryStream(obra.QR);
                Bitmap bitmap = new Bitmap(stream);
                Bitmap tempBitmap = new Bitmap(bitmap.Width, bitmap.Height); //Se crea uno vacío y se dibuja sobre ese.
                {
                    // Draw the original bitmap onto the graphics of the new bitmap
                    g.DrawImage(bitmap, 0, 0);
                }
                Graphics graphics = Graphics.FromImage(tempBitmap);
                Font arial = new Font("Arial", 50, FontStyle.Regular);
                Brush brush = new SolidBrush(Color.Black);
                string text = "Obra: " + obra.Nombre;
                Rectangle rectangle = new Rectangle(0, 0, 1000, 200);
                Pen pen = new Pen(Color.White, 2);
                graphics.DrawRectangle(pen, rectangle);
                graphics.DrawString(text, arial, brush, rectangle);
                tempBitmap.Save("C:\\Users\\user\\Desktop\\image.png");
                ImageConverter converter = new ImageConverter();

            return File((byte[])converter.ConvertTo(tempBitmap, typeof(byte[])), "image/png", obra.Nombre + " - QR.png");
            }

    }
}