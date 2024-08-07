using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioPlano : IRepositorioPlano
    {

        public ProyectoContext Context { get; set; }

        public RepositorioPlano()
        {
            this.Context = new ProyectoContext();
        }

        public void Precarga()
        {
            int cantTipoPlano = this.TomarTodosTipos().Count();
            if(cantTipoPlano == 0)
            {
                TipoPlano tipoGenerico = new TipoPlano();
                tipoGenerico.Categoria = "<<A INGRESAR>>";
                tipoGenerico.UltimaModificacion = "2022-3-1";
                //tipoGenerico.idObra = 1;
                Context.TiposPlanos.Add(tipoGenerico);

                Context.SaveChanges();
            }
        }

        private IEnumerable<TipoPlano> TomarTodosTipos()
        {
            return Context.TiposPlanos;
        }

        public void Agregar(Plano plano)
        {
            if (plano.IdObra == 0)
            {
                throw new ObraException("No se puede ingresar un plano en una obra nula.");
            }
            if (plano == null)
            {
                throw new ObraException("No se puede ingresar un plano nulo.");
            }
            //if (this.BuscarPlanoConTipo(plano.Nombre, plano.IdTipoPlano) != null)
            //{
            //    throw new ObraException("Ya existe un plano con ese nombre y ese tipo. Seleccione otro tipo de plano o cambie el nombre.");
            //}
            try
            {
                Obra obra = Context.Obras.FirstOrDefault(o => o.IdObra == plano.IdObra); 
                plano.Obra = obra;
                plano.FechaPublicado = DateTime.Now;
                plano.Validar();
                Context.Planos.Add(plano);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new ObraException("Error ingresando plano: " + e.Message);
            }
        }

        private Plano BuscarPlanoConTipo(string nombre, int idTipoPlano)
        {
            return Context.Planos.Where(p => p.Nombre == nombre && p.IdTipoPlano == idTipoPlano).FirstOrDefault();
        }

        public IEnumerable<Plano> BuscarPlanosDelTipoEnObra(int idTipoPlano, int idObra)
        {
            return Context.Planos.Where(p => p.IdTipoPlano == idTipoPlano && p.IdObra ==idObra);
        }

        public void Eliminar(Plano plano)
        {
            Obra obra = Context.Obras.FirstOrDefault(o => o.IdObra == plano.IdObra);
            if (obra == null)
            {
                throw new ObraException("No se puede eliminar un plano de una obra nula.");
            }
            if (plano == null)
            {
                throw new ObraException("No se puede eliminar un plano nulo.");
            }
            try
            {
                Context.Planos.Remove(plano);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new ObraException("Error eliminando plano: " + e.Message);
            }
        }

        public IEnumerable<Plano> PlanosFiltrados(Obra obra, int? idTipo, string? nombre, DateTime? fechaDesde, DateTime? fechaHasta)
        {
          
            if (obra == null)
            {
                throw new ObraException("No se pueden buscar planos en una obra nula.");
            }
            IEnumerable<Plano> planos = Context.Planos.Where(p=>p.Obra.IdObra == obra.IdObra); //creo que es descendiente, revisar luego
            if (idTipo != 0)
            {
                planos = planos.Where(p => p.IdTipoPlano == idTipo);
            }
            if (nombre != null)
            {
                planos = planos.Where(p => p.Nombre.ToUpper().Contains(nombre.ToUpper()));
            }
            if (fechaDesde != null && fechaHasta != null)
            {
                if (fechaDesde > fechaHasta)
                {
                    DateTime aux = (DateTime)fechaHasta; //Esto habria que revisar 
                    fechaHasta = fechaDesde;
                    fechaDesde = aux;
                }
                planos = planos.Where(p => p.FechaPublicado >= fechaDesde && p.FechaPublicado <= fechaHasta);
            }
            return planos.ToList();
        }

        public IEnumerable<Plano> PlanosPorAntiguedad(Obra obra)
        {
            if (obra == null)
            {
                throw new ObraException("No se pueden buscar planos en una obra nula.");
            }
            IEnumerable<Plano> planos = Context.Planos.Where(p => p.IdObra == obra.IdObra).OrderByDescending(p => p.FechaPublicado); //creo que es descendiente, revisar luego
            return planos;
        }

        public IEnumerable<Plano> PlanosTotales(Obra obra)
        {
 
            if (obra == null)
            {
                throw new ObraException("No se pueden buscar planos en una obra nula.");
            }
            
            IEnumerable<Plano> planos = Context.Planos.Where(p=> p.IdObra == obra.IdObra);
            return planos;
        }
        public IEnumerable<TipoPlano> BuscarTiposPlanos()
        {
            return Context.TiposPlanos.ToList();
        }

        public TipoPlano BuscarTipoPlano(int idTipo)
        {
            return Context.TiposPlanos.Where(tp=>tp.Id == idTipo).FirstOrDefault();
        }
        public Plano Buscar(int id)
        {
            return Context.Planos.Find(id);
        }

        public void Modificar(Plano item)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Plano> TomarTodos()
        {
            return Context.Planos;
        }

        public List<Plano> CrearPlanosMultiples(int idObra, int idTipoPlano, List<IFormFile> postedFiles)
        {
            List<Plano> list = new List<Plano>();
            foreach (IFormFile f in postedFiles)
            {
                Plano p = new Plano();
                p.IdObra = idObra;
                p.Nombre = f.FileName;
                p.IdTipoPlano = idTipoPlano;
                p.NombrePdf = f.FileName;
                p.TipoPdf = f.ContentType;
                list.Add(p);
            }

            return list;
        }

        public bool ExistePlano(byte[] plano, string planoNombre)
        {
            return Context.Planos.Any(p => p.Pdf == plano || p.NombrePdf == planoNombre);
        }









        //------------------------------------------------------------------------------------



        public TipoPlano TipoPlanoPorNombre(string nombreCarpeta)
        {
            return Context.TiposPlanos.Where(tp => tp.Categoria == nombreCarpeta).FirstOrDefault();
        }

        public TipoPlano CrearTipoPlano(string nombreCarpeta, int idObra)
        {
            TipoPlano nuevoTipo = new TipoPlano(nombreCarpeta);
            nuevoTipo.idObra = idObra;
            nuevoTipo.UltimaModificacion = "2000-01-01";
            Context.TiposPlanos.Add(nuevoTipo);
            Context.SaveChanges();

            return nuevoTipo;
        }

        public void ActualizarFechaUltimaModificacion(TipoPlano tipoPlanoActual, string ultimaModificacion)
        {
            tipoPlanoActual.UltimaModificacion = ultimaModificacion;
            EliminarTipoPlano(tipoPlanoActual);

        }

        public void EliminarTipoPlano(TipoPlano tipoPlanoActual)
        {
            foreach (var plano in Context.Planos)
            {
                if (plano.IdTipoPlano == tipoPlanoActual.Id)
                {
                    Context.Planos.Remove(plano);
                }
            }

            Context.TiposPlanos.Update(tipoPlanoActual);
            Context.SaveChanges();
        }

        public List<TipoPlano> BuscarTipoPlanoPorObra(int idObra)
        {
            List<TipoPlano> listaTP = new List<TipoPlano>();
            foreach (var tipoPlano in Context.TiposPlanos)
            {
                if (tipoPlano.idObra == idObra)
                {
                    listaTP.Add(tipoPlano);
                }

            }

            return listaTP;
        }

        internal int TraerIdPorNombreTipoPlano(string v)
        {   
            TipoPlano tipoplano = Context.TiposPlanos.Where(tp => tp.Categoria == v).FirstOrDefault();
            return tipoplano.Id;
        }

        public void CrearCarpeta(string path, string anterior, Obra obra)
        {
            Carpeta carpetaAnterior = this.BuscarCarpeta(anterior, obra);
            Carpeta carpeta = new Carpeta();
            if (BuscarCarpeta(path, obra) == null)
            {
                carpeta.IdObra = obra.IdObra;
                carpeta.Name = path;
                carpeta.Anterior = carpetaAnterior;
                Context.Carpetas.Add(carpeta);
                Context.SaveChanges();
            }
        }

        private Carpeta BuscarCarpeta(string path, Obra obra)
        {
            return Context.Carpetas.Where(c => c.Name == path && c.IdObra == obra.IdObra).FirstOrDefault();
        }

        public Carpeta ConseguirCarpetaContenedora(string webUrl, Obra obra)
        {
            List<string> paths = webUrl.Split("/").ToList();
            paths.RemoveAt(paths.Count - 1); //el ultimo siempre será el archivo, se elimina para agarrar a la carpeta contenedora
            string ultimo = paths.LastOrDefault();
            string path = Uri.UnescapeDataString(ultimo);
            Carpeta carpetaContenedora = this.BuscarCarpeta(path, obra);
            return carpetaContenedora;
        }
    }
}
