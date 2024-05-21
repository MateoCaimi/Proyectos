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
        private IRepositorioObra Repositorio = new RepositorioObra();
        // GET: ObraController
        public ActionResult Index()
        {
            IEnumerable<Obra> obras = Repositorio.TomarTodos();
            return View(obras);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string nombre, string direccion, bool finalizada)
        {
            IEnumerable<Obra> listadoObras = Repositorio.ObrasFiltradas(nombre, direccion, finalizada);
            return View(listadoObras);
        }

        // GET: ObraController/Details/5
        public ActionResult Detalles(int id)
        {
            try
            {
                ObraEstadisticasViewModel obra = new ObraEstadisticasViewModel();
                obra.Obra = Repositorio.Buscar(id);
                if (obra.Obra.Finalizada) //Yo sé que esto parece una locura.Usa un viewmodel para pasar todo de una a la vista y no usar muchos viewbags o tempdata, queda feo pero creo que es mejor.
                {
                    obra.MaterialMasSolicitado = Repositorio.MaterialMasSolicitado(obra.Obra.IdObra);
                    obra.MaterialMenosSolicitado = Repositorio.MaterialMenosSolicitado(obra.Obra.IdObra);
                    obra.AprobadorMasComun = Repositorio.AprobadorMasComun(obra.Obra.IdObra);
                    obra.ProveedorMasComun = Repositorio.ProveedorMasComun(obra.Obra.IdObra);
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
                Repositorio.Agregar(aIngresar);
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
                Obra obra = Repositorio.Buscar(id);
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
                Repositorio.Modificar(nuevaObra);
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
                Obra obra = Repositorio.Buscar(id);
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
                Obra obraABorrar = Repositorio.Buscar(id);
                Repositorio.Eliminar(obraABorrar);
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
                Obra obra = Repositorio.Buscar(id);
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
                Obra obraABorrar = Repositorio.Buscar(id);
                Repositorio.FinalizarObra(obraABorrar);
                return RedirectToAction(nameof(Index));
            }
            catch (ObraException e)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            }
        }

        public ActionResult Planos(int id)
        {
            try
            {
                IEnumerable<Plano> planos = Repositorio.PlanosTotales(id);
                if (planos == null) 
                {
                    planos = new List<LogicaNegocio.Entidades.Plano>();
                }
                return View(planos);
            }
            catch(ObraException e) //Solo manda ObraException si no existe obra
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                errorModel.RequestId = e.Message;
                return View("Error", errorModel); //usar shared hasta tener vistas de error para cada coso
            } 

        }
    }
}
