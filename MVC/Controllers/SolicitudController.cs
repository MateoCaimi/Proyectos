using LogicaAccesoDatos.Repositorios;
using LogicaNegocio.Entidades;
using LogicaNegocio.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class SolicitudController : Controller
    {

        Fachada Fachada = new Fachada();
        // GET: SolicitudController
        public ActionResult Index(int idObra)
        {
            IEnumerable<Solicitud> solicitudesObra = Fachada.SolicitudesDeObra(idObra);
            ViewBag.IdObra = idObra;
            return View(solicitudesObra);
        }

        // GET: SolicitudController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SolicitudController/Create
        public ActionResult Crear(int idObra)
        {
            try
            {
                if (!Fachada.BuscarObra(idObra).Finalizada)
                {
                    ViewBag.Materiales = Fachada.TodosLosMateriales();
                    ViewBag.IdObra = idObra;

                    SolicitudMaterialesViewModel vm = new SolicitudMaterialesViewModel();
                    vm.TempMaterials = new List<SolicitudMaterial>();

                    return View(vm);
                }
                else
                {
                    ViewBag.Error = "Esta obra está cerrada, no se pueden hacer solicitudes";
                    return View();
                }
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.IdObra = idObra;
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: SolicitudController/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Solicitud solicitud)
        {
            try
            {
                //// Guardar la solicitud en la base de datos
                //using (var context = new YourDbContext())
                //{
                //    context.Solicitudes.Add(solicitud);
                //    context.SaveChanges();

                //    // Recuperar los materiales desde TempData y guardarlos en la base de datos
                //    var tempMaterials = TempData["TempMaterials"] as List<SolicitudMaterial>;
                //    foreach (var material in tempMaterials)
                //    {
                //        material.SolicitudId = solicitud.Id;
                //        context.SolicitudMateriales.Add(material);
                //    }
                //    context.SaveChanges();
                //}

                //TempData["TempMaterials"] = null;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Agregar(SolicitudMaterial solicitudMaterial)
        {
            try
            {
                var tempMaterials = TempData["TempMaterials"] as List<SolicitudMaterial>;
                tempMaterials.Add(solicitudMaterial);

                TempData["TempMaterials"] = tempMaterials;
                TempData.Keep("TempMaterials");

                return PartialView("ListaMateriales", tempMaterials); // Aquí se usa ListaMateriales
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        // GET: SolicitudController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SolicitudController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SolicitudController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SolicitudController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
