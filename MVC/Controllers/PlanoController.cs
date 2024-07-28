using Azure.Core;
using LogicaAccesoDatos.EF;
using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MVC.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure.Identity;
using System.Net.Http;
using Microsoft.Graph.Models.TermStore;
using static System.Formats.Asn1.AsnWriter;
using NuGet.Protocol;
using ServiceStack.Web;
using Newtonsoft.Json;
using System.Security.Policy;
using LogicaNegocio.Entidades.DTOs;
using ServiceStack;


namespace MVC.Controllers
{
    public class PlanoController : Controller
    {

        private Fachada Fachada = new Fachada();


        // GET: PlanoController
        public async Task<ActionResult> Index(int idObra)
        {
            //DriveItem carpeta = this.ObtenerCarpeta().Result;
            Fachada.Precarga();
            await ObtenerCarpetaOneDrive();
            /*if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("LoginQR", "Usuario", new {id = idObra});
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }*/
            //Dictionary<string, int> carpetas = Fachada.CarpetasConCantidad();
            try
            {
                if (TempData["Error"] != null) //Para cuando viene de Agregar Plano siendo finalizada
                {
                    ViewBag.Error = TempData["Error"].ToString();
                }
                TempData["Error"] = null;
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View();
            }
            catch (ObraException e) //Solo manda ObraException si no existe obra
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
            catch(Exception e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                ViewBag.IdObra = idObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        //[HttpPost, ActionName("Index")]
        //[ValidateAntiForgeryToken]
        //public ActionResult IndexFiltrado(int idObra, string nombre, int idTipoPlano, DateTime? fechaInicio, DateTime? fechaFin)
        //{

        //    /*if (HttpContext.Session.GetString("UsuarioLogueado") == null)
        //    {
        //        return RedirectToAction("Index", "Usuario");
        //    }
        //    else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
        //    {
        //        return RedirectToAction("Listado", "Usuario");
        //    }*/

        //    Obra obra = Fachada.BuscarObra(idObra);
        //    IEnumerable<Plano> planosFiltrados = Fachada.PlanosFiltrados(obra,idTipoPlano,nombre,fechaInicio,fechaFin);
        //    ViewBag.IdObra = idObra;
        //    ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
        //    return View(planosFiltrados);
        //}


        // GET: PlanoController/Create
        public ActionResult Agregar(int idObra)
        {
            /*if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "UDeOficina")
            {
                return RedirectToAction("Index", "Obra");
            }*/

            TempData["Error"] = null;
            ViewBag.IdObra = idObra;
            if (!Fachada.BuscarObra(idObra).Finalizada)
            {
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View();
            }
            else
            {
                TempData["Error"] = "No se puede agregar planos a una obra finalizada.";
                return RedirectToAction("Index", new { idObra = idObra });
            }
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Agregar(Plano aIngresar, IFormFile archivoImagen)
        {

            /*if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") != "UDeOficina")
            {
                return RedirectToAction("Index", "Obra");
            }*/


            try
            {
                if (aIngresar == null || aIngresar.TipoPdf != "application/pdf")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await archivoImagen.CopyToAsync(memoryStream);
                        aIngresar.NombrePdf = archivoImagen.FileName;
                        aIngresar.TipoPdf = archivoImagen.ContentType;
                        aIngresar.Pdf = memoryStream.ToArray();
                    }
                }
                else
                {
                    TempData["Error"] = "Debe proporcionar un archivo válido.";
                    return View();
                }
                if(aIngresar.Nombre == null)
                {
                    aIngresar.Nombre = aIngresar.NombrePdf.ToUpper();
                    string path = aIngresar.Nombre;
                    string pattern = @"\.\w+$";
                    Match match = Regex.Match(path, pattern);
                    aIngresar.Nombre = aIngresar.Nombre.Replace(match.Value, "");
                }
                ViewBag.IdObra = aIngresar.IdObra;
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                Fachada.AgregarPlano(aIngresar);
          

                return RedirectToAction("Index", new { idObra = aIngresar.IdObra });
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Index", new { idObra = aIngresar.IdObra });
            }
        }


        // GET: PlanoController/Delete/5
        public ActionResult Eliminar(int id)
        {
            //if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            //{
            //    return RedirectToAction("Index", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") != "UDeOficina")
            //{
            //    return RedirectToAction("Index", "Obra");
            //}

            Plano plano = Fachada.BuscarPlano(id);
            return View(plano);
        }

        // POST: PlanoController/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            //if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            //{
            //    return RedirectToAction("Index", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") != "UDeOficina")
            //{
            //    return RedirectToAction("Index", "Obra");
            //}

            try
            {
                Plano planoABorrar = Fachada.BuscarPlano(id);
                Fachada.EliminarPlano(planoABorrar);
                return RedirectToAction("Index", new { idObra = planoABorrar.IdObra });
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }



        [HttpGet("{id}/pdf")]
        public IActionResult ObtenerPDF(int id)
        {
            //if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            //{
            //    return RedirectToAction("Index", "Usuario");
            //}
            //else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            //{
            //    return RedirectToAction("Listado", "Usuario");
            //}

            Plano plano = Fachada.BuscarPlano(id);

            if (plano == null || plano.Pdf == null)
            {
                return NotFound();
            }

            return File(plano.Pdf, plano.TipoPdf, plano.NombrePdf);
        }

        public ActionResult ListarPlanos(int idTipoPlano, int idObra)
        {
            IEnumerable<Plano> planos = Fachada.BuscarPlanosDelTipoEnObra(idTipoPlano, idObra);
            ViewBag.Tipo = Fachada.BuscarTipoPlano(idTipoPlano);
            return View(planos);

        }
        public ActionResult SubidaMultiple(int idObra)
        {
            TempData["Error"] = null;
            ViewBag.IdObra = idObra;
            if (!Fachada.BuscarObra(idObra).Finalizada)
            {
                ViewBag.TiposdePlano = Fachada.BuscarTiposPlanos();
                return View();
            }
            else
            {
                TempData["Error"] = "No se puede agregar planos a una obra finalizada.";
                return RedirectToAction("Index", new { idObra = idObra });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubidaMultiple(int IdObra, int idTipoPlano, List<IFormFile> postedFiles)
        {
            
            TempData["Error"] = null;
            ViewBag.IdObra = IdObra;
            if (!Fachada.BuscarObra(IdObra).Finalizada)
            {
                List<Plano> planos = Fachada.CrearPlanosMultiples(IdObra, idTipoPlano, postedFiles);
                for(int i = 0; i < planos.Count(); i++)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await postedFiles[i].CopyToAsync(memoryStream);
                        planos[i].Pdf = memoryStream.ToArray();
                    }
                }
                foreach(Plano p in planos)
                {
                    Fachada.AgregarPlano(p);
                }
                return View();
            }
            else
            {
                TempData["Error"] = "No se puede agregar planos a una obra finalizada.";
                return RedirectToAction("Index", new { idObra = IdObra });
            }
        }
        private async Task<string> ObtenerTokenDeAccesoGraph()
        {
            var clientId = "ed32de75-de3c-4053-bb7c-488659eda9ad";
            var clientSecret = "ns.8Q~ulrwLxTPhDR8jHeBIs.PlB5m3LIHD3pdoY";
            var tenantId = "20feb869-2f89-4be1-a7ed-fcc4d1579353";
            var authority = $"https://login.microsoftonline.com/{tenantId}";

            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            string[] scopes = { "https://graph.microsoft.com/.default" };

            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            string accessToken = result.AccessToken;
            return accessToken;
        }

        public async Task<DriveItem> ObtenerCarpetaOneDrive()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string tokenAcceso = ObtenerTokenDeAccesoGraph().Result;

                    var scopes = new[] { "https://graph.microsoft.com/.default" };

                    var tenantId = "20feb869-2f89-4be1-a7ed-fcc4d1579353";

                    var clientId = "ed32de75-de3c-4053-bb7c-488659eda9ad";

                    var clientSecret = "ns.8Q~ulrwLxTPhDR8jHeBIs.PlB5m3LIHD3pdoY";

                    var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                    var organizationId = "20feb869-2f89-4be1-a7ed-fcc4d1579353";

                    var driveId = "b!msuOPhxmpkacBgMQlPFHs0GBQhXAt9RDgmQl3jvMs1RdQUZNQ3BeQI8-0DOwkKAW";

                    var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

                    var groups = await graphClient.Groups.GetAsync();
                    var groupId = groups.Value.First().Id;

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcceso);
                    var siteId1 = "b2c703f4-c46f-4fd6-b23a-6976859a82a9";
                    var siteId2 = "150412ee-bfd0-4386-8d59-c7993571ccee";
                    var userId = "27e25a40-12ac-4f7f-95b8-fef55f973bfb";
                    //ESTE ES EL QUE TRAE ARCHIVOS. REVISAR CONTENT
                    var getUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/drive/root/children";
                    HttpResponseMessage response = await httpClient.GetAsync(getUrl);
                    var content = response.Content.ReadAsStringAsync();
                    var contentJson = content.Result; //los values
                    DriveDTO drive = JsonConvert.DeserializeObject<DriveDTO>(contentJson);
                    DriveDTO driveFiltrado = this.FiltrarCarpetas(drive); //filtra por pdfs. ignora carpetas (arreglar)
                    foreach(ArchivoDTO archivo in driveFiltrado.value)
                    {
                        var getUrl2 = $"https://graph.microsoft.com/v1.0/users/{userId}/drive/items/{archivo.Id}?select=id,@microsoft.graph.downloadUrl";
                        HttpResponseMessage response2 = await httpClient.GetAsync(getUrl2);
                        var download = response2.Content.ReadAsStringAsync().Result;
                        string pattern = "@microsoft\\.graph\\.downloadUrl\":\"([^\"]*)\"";
                        Match match = Regex.Match(download, pattern);
                        if (match.Success)
                        {
                            string link = match.Groups[1].Value; //El link de descarga
                            byte[] plano = await httpClient.GetByteArrayAsync(link);
                            string planoNombre = archivo.Name;
                            string tipo = "application/pdf";
                            Plano planoNuevo = new Plano();
                            planoNuevo.NombrePdf = planoNombre;
                            planoNuevo.Pdf = plano;
                            planoNuevo.TipoPdf = tipo;
                            planoNuevo.FechaPublicado = DateTime.Now;
                            planoNuevo.IdTipoPlano = 1; //Tipo genérico
                            planoNuevo.Nombre = planoNombre;
                            planoNuevo.IdObra = 1; //Obra genérica?
                            Fachada.AgregarPlano(planoNuevo);
                        }
                    }
                    

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DriveDTO FiltrarCarpetas(DriveDTO? drive)
        {
            DriveDTO nuevo = new DriveDTO();
            nuevo.value = new List<ArchivoDTO>();
            foreach(ArchivoDTO archivo in drive.value)
            {
                if (archivo.Name.EndsWith(".pdf"))
                {
                    nuevo.value.Add(archivo);
                }
            }
            return nuevo;
        }

        public async Task<DriveItem> ObtenerCarpeta()
        {

            try
            {
                string tokenAcceso = ObtenerTokenDeAccesoGraph().Result;

                var scopes = new[] { "https://graph.microsoft.com/.default" };

                // Multi-tenant apps can use "common",
                // single-tenant apps must use the tenant ID from the Azure portal
                var tenantId = "20feb869-2f89-4be1-a7ed-fcc4d1579353";

                // Value from app registration
                var clientId = "ed32de75-de3c-4053-bb7c-488659eda9ad";


                var clientSecret = "ns.8Q~ulrwLxTPhDR8jHeBIs.PlB5m3LIHD3pdoY";

                var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

                var driveId = "b!msuOPhxmpkacBgMQlPFHs0GBQhXAt9RDgmQl3jvMs1RdQUZNQ3BeQI8-0DOwkKAW";

                var userId = "27e25a40-12ac-4f7f-95b8-fef55f973bfb";
                var resultado = await graphClient.Users[userId].Drive.GetAsync();


                /*using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcceso);
                    HttpResponseMessage response = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/me/drive/recent");

                    
                    return null;
                }*/

                return null;

            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting folder: {ex.Message}");
                throw;
            }
        }

        private async Task<User> ConseguirDrives()
        {
            string tokenAcceso = ObtenerTokenDeAccesoGraph().Result;

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            var tenantId = "20feb869-2f89-4be1-a7ed-fcc4d1579353";

            // Value from app registration
            var clientId = "ed32de75-de3c-4053-bb7c-488659eda9ad";


            var clientSecret = "ns.8Q~ulrwLxTPhDR8jHeBIs.PlB5m3LIHD3pdoY";

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var driveId = "b!msuOPhxmpkacBgMQlPFHs0GBQhXAt9RDgmQl3jvMs1RdQUZNQ3BeQI8-0DOwkKAW";

            var groupId = "01e5d1e0-29d9-440d-832b-0ac27bfc1c5f";

            return await graphClient.Me.GetAsync();
        }
    }
}





