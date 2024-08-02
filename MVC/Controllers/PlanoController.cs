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
using Microsoft.Graph.Authentication;
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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using NuGet.Common;


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
            string siteId = "bodegapiedrafita.sharepoint.com,95552c4f-844e-44a0-b73d-7b7f3cda8e39,f5ebb529-4b04-4838-9fa8-73750fa93b26";
            string path = "1.%20PROYECTO/02.APROBADO";
            List<JObject> list = new List<JObject>();
            //await ObtenerCarpetaOneDrive();
            
            list = await GraphRecursivoParalelizado();//await TomarPdfRecursivo(siteId, path);
            List<Plano> planos = await this.FormateoDePlanos(list);
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
            catch (Exception e)
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
                if (aIngresar.Nombre == null)
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
                for (int i = 0; i < planos.Count(); i++)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await postedFiles[i].CopyToAsync(memoryStream);
                        planos[i].Pdf = memoryStream.ToArray();
                    }
                }
                foreach (Plano p in planos)
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

                    var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcceso);
                    var userId = "27e25a40-12ac-4f7f-95b8-fef55f973bfb";
                    var idXigna = "bodegapiedrafita.sharepoint.com,95552c4f-844e-44a0-b73d-7b7f3cda8e39,f5ebb529-4b04-4838-9fa8-73750fa93b26";
                    var idIsleny = "bodegapiedrafita.sharepoint.com,fb1d4765-2750-4024-9f2a-400fdb36d6f6,0f8e81ca-2380-4e1a-88aa-ba6eae565975";
                    var idListDocumentos = "0e38f59a-b64e-47df-8116-d6c9a8d2945d";
                    var idFedeBodega = "bodegapiedrafita-my.sharepoint.com,b2c703f4-c46f-4fd6-b23a-6976859a82a9,150412ee-bfd0-4386-8d59-c7993571ccee";
                    var driveIdDocuments = "b!9APHsm_E1k - yOml2hZqCqe4SBBXQv4ZDjVnHmTVxzO5tRwh6gJMjTKJ5BiA3oFZM";
                    var idAprobado = "01BAYPMGGZZP5UUSW6M5GLC3BZYEC7262A";
                    var idproceso = "01BAYPMGHHW6XSAA3IUVB2BH6YXY3G4YHV";
                    //ESTE ES EL QUE TRAE ARCHIVOS. REVISAR CONTENT
                    //var getUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/drive/root/children";  Se puede traer el site id con el noombre del site osea de la obra, usar esto mas adelnate para ver si funciona
                    //var getUrl = $"https://graph.microsoft.com/v1.0/sites/{idXigna}/lists/{idListDocumentos}/items";
                    var getUrl = $"https://graph.microsoft.com/v1.0/sites";

                    //var getUrl = $"https://graph.microsoft.com/v1.0/sites/{idIsleny}/drive/root:/1.%20PROYECTO/02.APROBADO";
                    //var getUrl = $"https://graph.microsoft.com/v1.0/drives/b!TyxVlU6EoES3PXt_PNqOOSm16_UESzhIn6hzdQ-pOyaa9TgOTrbfR4EW1smo0pRd/items/01BAYPMGGZZP5UUSW6M5GLC3BZYEC7262A/children";
                    //var getUrl = $"https://graph.microsoft.com/v1.0/sites/{idXigna}/drive/root:/1.%20PROYECTO/02.APROBADO/01.ALBA%C3%91ILERIA/AL1-IMPLANTACION:/children";
                    HttpResponseMessage response = await httpClient.GetAsync(getUrl);
                    // var folderAttachmentsId = "01TXBKQWRZF2PDJIHD7BD2NSNQDMK7T4RF";
                    var content = response.Content.ReadAsStringAsync();
                    var archivosRoot = content.Result; //los values
                    DriveDTO driveRoot = JsonConvert.DeserializeObject<DriveDTO>(archivosRoot); 
                    ArchivoDTO archivo1 = driveRoot.value.First(); //El primer sitio va a traer lo mismo que los demás.
                    string driveId = "b!8nhpxotooUOpTxumD-yUZZftdxCsZtRDrtbWUXMo4Wtrc_pqCXxFQb6jFYOVPZwf"; //Es el mismo para cada sitio. Con hardcodear uno alcanza.
                    var getUrl3 = $"https://graph.microsoft.com/v1.0/sites/{archivo1.Id}/drives/{driveId}/root/search(q='')";
                    HttpResponseMessage response3 = await httpClient.GetAsync(getUrl3);
                    var download = response3.Content.ReadAsStringAsync().Result;
                    //DriveDTO driveNuevo = await AgregaArchivosCarpetasAsync(driveRoot); 
                    //DriveDTO driveFiltrado = this.FiltrarCarpetas(driveRoot); //filtra por pdfs. ignora carpetas (arreglar)
                    foreach (ArchivoDTO archivo in driveRoot.value)
                    {


                        /*

                        string pattern = "@microsoft\\.graph\\.downloadUrl\":\"([^\"]*)\"";
                        Match match = Regex.Match(download, pattern);
                        if (match.Success)
                        {
                            string link = match.Groups[1].Value; //El link de descarga
                            byte[] plano = await httpClient.GetByteArrayAsync(link);
                            string planoNombre = archivo.Name;
                            int idObra = 2;// Fachada.TraerIdPorNombreObra(archivo.Name);
                            string tipo = "application/pdf";
                            Plano planoNuevo = new Plano();
                            planoNuevo.NombrePdf = planoNombre;
                            planoNuevo.Pdf = plano;
                            planoNuevo.TipoPdf = tipo;
                            planoNuevo.FechaPublicado = DateTime.Now;
                            planoNuevo.IdTipoPlano = 1; //Tipo genérico
                            planoNuevo.Nombre = planoNombre;
                            planoNuevo.IdObra = idObra;
                            if(idObra != 0) //Si no encuentra obra, no es un plano para agregar. Es o un pdf cualquiera o de una obra no subida
                            {
                                Fachada.AgregarPlano(planoNuevo);
                            }
                        
                        }*/
                    }


                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //private async Task<DriveDTO> AgregaArchivosCarpetasAsync(DriveDTO drive)
        //{
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        string tokenAcceso = ObtenerTokenDeAccesoGraph().Result;

        //        var scopes = new[] { "https://graph.microsoft.com/.default" };

        //        var tenantId = "20feb869-2f89-4be1-a7ed-fcc4d1579353";

        //        var clientId = "ed32de75-de3c-4053-bb7c-488659eda9ad";

        //        var clientSecret = "ns.8Q~ulrwLxTPhDR8jHeBIs.PlB5m3LIHD3pdoY";

        //        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        //        var organizationId = "20feb869-2f89-4be1-a7ed-fcc4d1579353";

        //        var driveId = "b!msuOPhxmpkacBgMQlPFHs0GBQhXAt9RDgmQl3jvMs1RdQUZNQ3BeQI8-0DOwkKAW";

        //        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcceso);

        //        var userId = "27e25a40-12ac-4f7f-95b8-fef55f973bfb";




        //        DriveDTO nuevoDrive = new DriveDTO();
        //        nuevoDrive.value.AddRange(drive.value);

        //        foreach (ArchivoDTO posibleCarpeta in drive.value)
        //        {
        //            if (posibleCarpeta.SiteCollection.HostName != "0")
        //            {
        //                var getUrl2 = $"https://graph.microsoft.com/v1.0/users/{userId}/drive/items/{posibleCarpeta.Id}/children";
        //                HttpResponseMessage response2 = await httpClient.GetAsync(getUrl2);
        //                var content2 = response2.Content.ReadAsStringAsync();
        //                var archivosCarpeta = content2.Result; //los values
        //                DriveDTO driveCarpeta = JsonConvert.DeserializeObject<DriveDTO>(archivosCarpeta);
        //                nuevoDrive.value.AddRange(driveCarpeta.value);
        //                nuevoDrive.value = nuevoDrive.value.Distinct().ToList();
        //                nuevoDrive = await AgregaArchivosCarpetasAsync(nuevoDrive);
        //                nuevoDrive.value = nuevoDrive.value.Distinct().ToList();
        //            }
        //        }

        //        return nuevoDrive;
        //    }

        //}

        private async Task<List<Plano>> FormateoDePlanos(List<JObject> list)
        {
            string token = await ObtenerTokenDeAccesoGraph();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                foreach (JObject obj in list)
                {
                    string input = obj.ToString();
                    string inicio = "\"@microsoft.graph.downloadUrl\": \"";
                    string final = "\"";
                    string pattern = $@"{Regex.Escape(inicio)}(.*?){Regex.Escape(final)}";

                    Match match = Regex.Match(input, pattern);
                    if (match.Success)
                    {
                        string webUrl = obj["webUrl"].ToString();
                        string result = match.Groups[1].Value;
                        byte[] plano = await httpClient.GetByteArrayAsync(result);
                        string planoNombre = obj["name"].ToString();
                        bool checkExistencia = Fachada.ExistePlano(plano, planoNombre);
                        if (!checkExistencia)
                        {
                            int idObra = Fachada.TraerIdPorNombreObra(webUrl);
                            if (idObra == 0)
                            {
                                idObra = Fachada.TraerIdPorNombreObra(planoNombre);
                            }
                            string tipo = "";
                            if (planoNombre.Contains(".pdf"))
                            {
                                tipo = "application/pdf";
                            }
                            else if (planoNombre.Contains(".png"))
                            {
                                tipo = "image/png";
                            }
                            if (tipo != "")
                            {
                                TipoPlano tipoPlano = this.TraerTipoPlanoPorRuta(webUrl);
                                Plano planoNuevo = new Plano();
                                planoNuevo.NombrePdf = planoNombre;
                                planoNuevo.Pdf = plano;
                                planoNuevo.TipoPdf = tipo;
                                planoNuevo.FechaPublicado = DateTime.Now;
                                planoNuevo.IdTipoPlano = 1; //Tipo genérico
                                planoNuevo.Nombre = planoNombre;
                                planoNuevo.IdObra = idObra;
                                planoNuevo.IdTipoPlano = tipoPlano.Id;
                                if (idObra != 0) //Si no encuentra obra, no es un plano para agregar. Es o un pdf cualquiera o de una obra no subida
                                {
                                    Fachada.AgregarPlano(planoNuevo);
                                }
                            }
                        }
                        
                    }
                }

            }
            return null;
        }

        private TipoPlano TraerTipoPlanoPorRuta(string ruta)
        {
            ruta = ruta.Replace("%20", " ");
            IEnumerable<TipoPlano> tipos = Fachada.BuscarTiposPlanos();
            foreach(TipoPlano tipo in tipos)
            {
                if (ruta.ToLower().Contains(tipo.Categoria.ToLower()))
                {
                    return tipo;
                }
            }
            return tipos.First(); //<<A INGRESAR>>
        }

        private async Task<List<JObject>> GraphRecursivoParalelizado()
        {
            string path = "1.%20PROYECTO/02.APROBADO";
            string token = await ObtenerTokenDeAccesoGraph();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tasks = new List<Task<List<JObject>>>();
                var requestUri = $"https://graph.microsoft.com/v1.0/sites/";
                var response = await httpClient.GetStringAsync(requestUri);

                // Parseo respuesta
                var json = JObject.Parse(response);
                var items = json["value"].ToList(); // Convierto a lista para facilitar el manejo

                foreach (var item in items)
                {
                    if (this.EstaEnObra(item["webUrl"].ToString()))
                    {
                        string url = item["id"].ToString();
                        tasks.Add(this.TomarPdfRecursivo(url, path, token));
                    }
                }
                await Task.WhenAll(tasks);

                var postResponses = new List<JObject>();

                foreach (var t in tasks)
                {
                    var postResponse = t.Result; //t.Result would be okay too.
                    postResponses.AddRange(postResponse);
                }

                return postResponses;
            }


        }

        private bool EstaEnObra(string url)
        {
            IEnumerable<Obra> obras = Fachada.TomarTodasObras();
            foreach(Obra obra in obras)
            {
                if (url.ToLower().Contains(obra.Nombre.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<List<JObject>> TomarPdfRecursivo(string siteId, string path, string token)
        {
            var pdfs = new List<JObject>();
            await TomarPdfsInterno(siteId, path, pdfs, token);
            return pdfs;
        }

        private async Task<List<JObject>> TomarPdfsInterno2(string siteId, string path, List<JObject> pdfs)
        {
            string token = await ObtenerTokenDeAccesoGraph();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tasks = new List<Task>();

                var requestUri = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drive/root:/{path}:/children";
                var response = await httpClient.GetStringAsync(requestUri);

                // Parseo respuesta
                var json = JObject.Parse(response);
                var items = json["value"].ToList(); // Convierto a lista para facilitar el manejo

                foreach (var item in items)
                {
                    if (item["folder"] != null)
                    {
                        // Es un folder, necesitamos llamar al método recursivamente
                        var subPath = $"{path}/{item["name"]}";
                        // Lanzar tarea asíncrona para procesamiento paralelo
                        tasks.Add(TomarPdfsInterno(siteId, subPath, pdfs, token));
                    }
                    else
                    {
                        // Es un archivo PDF, agregarlo a la lista
                        pdfs.Add(item as JObject);
                    }
                }

                // Esperar a que todas las tareas se completen
                await Task.WhenAll(tasks);

                return pdfs;
            }
        }

        private async Task<List<JObject>> TomarPdfsInterno(string siteId, string path, List<JObject> pdfs, string token)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tasks = new List<Task<List<JObject>>>();
                var requestUri = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drive/root:/{path}:/children";
                try
                {
                    var response = await httpClient.GetStringAsync(requestUri); //si tira 404 seguí de largo

                    // Parseo respuesta
                    var json = JObject.Parse(response);
                    var items = json["value"].ToList(); // Convierto a lista para facilitar el manejo
                    foreach (var item in items)
                    {
                        if (item["folder"] != null)
                        {

                            //FEDE TE DEJO LA IDEA DE LAS FECHAS. 

                            //Es folder entonces es tipo plano. Tomar el substring del name del folder y buscar el tipo plano.
                            //string nombreCarpeta = "name del folder";
                            //if(fachada.ExisteTipoPlano(nombreCarpeta)){
                            //TipoPlano TipoPlanoActual = Fachada.BuscarTipoPlanoPorNombre(string );
                            //}
                            //Aca deberiamos tener el if del lastTimeodify
                            //if(item.lasttimemodufy != TipoPlanoActual.LastTimeModify){

                            //Aca iria la llamada recursiva sino sale del if y sigue sin entrar a la carpeta

                            //}
                            // es un folder, necesitamos llamar al método recursivamente
                            var subPath = $"{path}/{item["name"]}";
                            // Lanzar tarea asíncrona para procesamiento paralelo
                            tasks.Add(TomarPdfsInterno(siteId, subPath, pdfs, token));

                            /*TipoPlano tipo = this.TraerTipoPlanoPorRuta(item["webUrl"].ToString());
                            if (tipo != null)
                            {
                                if(tipo.UltimaModificacion != item["lastTimeModified"].ToString())
                                {

                                }
                            }*/
                        }
                        else
                        {
                            pdfs.Add(item as JObject);
                        }
                    }

                    // Esperar a que todas las tareas se completen
                    await Task.WhenAll(tasks);

                    return pdfs;
                }
                catch (Exception ex)
                {
                    return null;
                }




            }
        }


        private DriveDTO FiltrarCarpetas(DriveDTO? drive)
        {
            var userId = "27e25a40-12ac-4f7f-95b8-fef55f973bfb";
            DriveDTO nuevo = new DriveDTO();
            nuevo.value = new List<ArchivoDTO>();
            foreach (ArchivoDTO archivo in drive.value)
            {
                if (archivo.Name.EndsWith(".pdf") || archivo.Name.EndsWith(".png"))
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





