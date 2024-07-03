using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Fachada Fachada = new Fachada();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            Fachada = new Fachada();
         
             
        }

        public IActionResult Index()
        {
            
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {

            return RedirectToAction("Index", "Usuario");
            }
            else if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario administrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }








            if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de oficina")
            {


                List<Solicitud> solicitudesPendientes = Fachada.BuscarSolicitudPendientesLista();
                var opciones = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,

                };
                HttpContext.Session.SetString("SolicitudesPendientes", System.Text.Json.JsonSerializer.Serialize(solicitudesPendientes, opciones));




                List<Solicitud> solicitudesConfirmadas = Fachada.BuscarSolicitudConfirmadasLista();
                var opciones2 = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,

                };
                HttpContext.Session.SetString("SolicitudesConfirmadas", System.Text.Json.JsonSerializer.Serialize(solicitudesConfirmadas, opciones2));

            }



            if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de obra")
            {
                string nomObrero = HttpContext.Session.GetString("UsuarioLogueado");
                List<Solicitud> solicitudesAprobadas = Fachada.BuscarSolicitudAprobadasParaUnUObra(nomObrero);
                var opciones3 = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,

                };
                HttpContext.Session.SetString("SolicitudesAprobadas", System.Text.Json.JsonSerializer.Serialize(solicitudesAprobadas, opciones3));
            }


            //if (HttpContext.Session.GetString("UsuarioTipo") == "Usuario de oficina")
            //{
            //    List<Solicitud> solicitudesPendientes = Fachada.BuscarSolicitudPendientesLista();
               

            //    var opciones = new JsonSerializerOptions
            //    {
            //        ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //        WriteIndented = true,

            //    };

            //    HttpContext.Session.SetString("SolicitudesPendientes", JsonSerializer.Serialize(solicitudesPendientes, opciones));

            //    if (solicitudesPendientes != null)
            //    {
                    
            //        return View(solicitudesPendientes);
            //    }
            //}
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
