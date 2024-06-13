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


namespace MVC.Controllers
{
    public class PlanoController : Controller
    {

        private Fachada Fachada = new Fachada();


        // GET: PlanoController
        public ActionResult Index(int idObra)
        {

            /*if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Usuario");
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

    }
}


        //private async Task<string> ObtenerTokenDeAccesoGraph()
        //{
        //    var clientId = "dbde2b1a-6c38-46c7-9465-e8f4c3fa7961";
        //    var clientSecret = "72D8Q~vHtGdsR-kcRd~rd4BIPgOpVDHfN6bv6a4.";
        //    var tenantId = "d79720cd-d8c0-4d0c-a404-2dcd025f01e3";
        //    var authority = $"https://login.microsoftonline.com/{tenantId}";

        //    var app = ConfidentialClientApplicationBuilder.Create(clientId)
        //        .WithClientSecret(clientSecret)
        //        .WithAuthority(new Uri(authority))
        //        .Build();

        //    string[] scopes = { "https://graph.microsoft.com/.default" };

        //    AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        //    string accessToken = result.AccessToken;
        //    return accessToken;
        //}

        //private async Task SubirATeams(Plano aIngresar)
        //{
        //    string tokenAcceso = ObtenerTokenDeAccesoGraph().Result;
        //    string groupId = "66f0d73d-1cad-4e6a-9291-c31f775b4937";
        //    MemoryStream aSubir = new MemoryStream(aIngresar.Pdf);
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcceso);


        //        var content = new StreamContent(aSubir);
        //        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        //        var uploadUrl = $"https://graph.microsoft.com/v1.0/groups/{groupId}/drive/items/root:/{aIngresar.NombrePdf}:/content";

        //        HttpResponseMessage response = await httpClient.PutAsync(uploadUrl, content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            string errorResponse = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine($"Error: {response.StatusCode}");
        //            Console.WriteLine(errorResponse);
        //        }
        //        response.EnsureSuccessStatusCode();

        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        JObject jsonResponse = JObject.Parse(responseBody);

        //        Console.WriteLine("File uploaded successfully!");
        //        Console.WriteLine(jsonResponse.ToString());

        //    }
        //}

        //public async Task<DriveItem> ObtenerCarpeta()
        //{

        //    try
        //    {
        //        string tokenAcceso = ObtenerTokenDeAccesoGraph().Result;
                
        //        var scopes = new[] { "https://graph.microsoft.com/.default" };

        //        // Multi-tenant apps can use "common",
        //        // single-tenant apps must use the tenant ID from the Azure portal
        //        var tenantId = "d79720cd-d8c0-4d0c-a404-2dcd025f01e3";

        //        // Value from app registration
        //        var clientId = "dbde2b1a-6c38-46c7-9465-e8f4c3fa7961";

        //        var clientSecret = "72D8Q~vHtGdsR-kcRd~rd4BIPgOpVDHfN6bv6a4.";

        //        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        //        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        //        var result = await graphClient.Drives.GetAsync();

        //        /*using (HttpClient httpClient = new HttpClient())
        //        {
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcceso);
        //            HttpResponseMessage response = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/me/drive/recent");

                    
        //            return null;
        //        }*/

        //        return null;

        //    }
        //    catch (ServiceException ex)
        //    {
        //        Console.WriteLine($"Error getting folder: {ex.Message}");
        //        throw;
        //    }
        //}