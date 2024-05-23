using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVC.Models;

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
            return View();
        }

        // POST: ObraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Obra aIngresar)
        {
            try
            {
                Fachada.AgregarObra(aIngresar);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
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
        public ActionResult EditarConfirmado(Obra nuevaObra)
        {   
            try
            {
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
            try
            {
                Obra obraABorrar = Fachada.BuscarObra(id);
                Fachada.FinalizarObra(obraABorrar);
                return RedirectToAction(nameof(Index));
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }


}
}