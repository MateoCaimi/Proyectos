using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;

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
            else if (HttpContext.Session.GetString("UsuarioTipo") == "UAdministrador")
            {
                return RedirectToAction("Listado", "Usuario");
            }

            /*Usuario usuarioTest = new UDeOficina("Federico Ruiz", "JorgeJorge123", "JorgeJorge123");
            Fachada.AgregarUsuario(usuarioTest);*/
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
