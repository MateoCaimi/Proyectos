using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.AspNetCore.Http;

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
            List<TipoPlano> cantTipoPlano = this.TomarTodosTipos().ToList();
            if (cantTipoPlano.Count == 0)
            {
                TipoPlano tipoGenerico = new TipoPlano();
                tipoGenerico.Categoria = "<<A INGRESAR>>";
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

        public IEnumerable<Plano> BuscarPlanosDelTipoEnObra(int idTipoPlano, int idObra)
        {
            return Context.Planos.Where(p => p.IdTipoPlano == idTipoPlano && p.IdObra == idObra);
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
            IEnumerable<Plano> planos = Context.Planos.Where(p => p.Obra.IdObra == obra.IdObra); //creo que es descendiente, revisar luego
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

            IEnumerable<Plano> planos = Context.Planos.Where(p => p.IdObra == obra.IdObra);
            return planos;
        }
        public IEnumerable<TipoPlano> BuscarTiposPlanos()
        {
            return Context.TiposPlanos.ToList();
        }

        public TipoPlano BuscarTipoPlano(int idTipo)
        {
            return Context.TiposPlanos.Where(tp => tp.Id == idTipo).FirstOrDefault();
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

        public TipoPlano CrearTipoPlano(string nombreCarpeta, int idObra)
        {
            TipoPlano nuevoTipo = new TipoPlano(nombreCarpeta);
            nuevoTipo.IdObra = idObra;
            List<TipoPlano> tipos = this.BuscarTipoPlanoPorObra(idObra);
            foreach (TipoPlano tipo in tipos)
            {
                if (tipo.Categoria == nuevoTipo.Categoria)
                {
                    return null;
                }
            }
            Context.TiposPlanos.Add(nuevoTipo);
            Context.SaveChanges();

            return nuevoTipo;
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
                if (tipoPlano.IdObra == idObra)
                {
                    listaTP.Add(tipoPlano);
                }

            }

            return listaTP;
        }

        public void CrearCarpeta(string path, string anterior, Obra obra)
        {
            Carpeta carpetaAnterior = this.BuscarCarpeta(anterior, obra);
            Carpeta carpeta = new Carpeta();
            if (BuscarCarpeta(path, obra) == null)
            {
                carpeta.IdObra = obra.IdObra;
                carpeta.Name = path;
                if (carpetaAnterior != null)
                {
                    if (carpetaAnterior.IdTipo != null)
                    {
                        carpeta.NameAnterior = carpetaAnterior.NameAnterior;
                    }
                    else
                    {
                        carpeta.NameAnterior = carpetaAnterior.Name;
                    }

                }
                TipoPlano tipo = this.BuscarTipoPlanoPorNombre(carpeta.Name, obra);
                if (tipo != null)
                {
                    carpeta.IdTipo = tipo.Id;
                }

                Context.Carpetas.Add(carpeta);
                Context.SaveChanges();
            }
        }

        public TipoPlano BuscarTipoPlanoPorNombre(string name, Obra obra)
        {
            TipoPlano tipo = Context.TiposPlanos.Where(tp => tp.Categoria == name && tp.IdObra == obra.IdObra).FirstOrDefault();
            return tipo;
        }

        public Carpeta BuscarCarpeta(string path, Obra obra)
        {
            return Context.Carpetas.Where(c => c.Name == path && c.IdObra == obra.IdObra).FirstOrDefault();
        }
        public void MapearTiposACarpetas()
        {
            List<Plano> planos = this.TomarTodos().ToList();
            List<TipoPlano> tipos = this.TomarTodosTipos().ToList();
            List<Carpeta> carpetas = this.TomarTodasCarpetas().ToList();
            foreach (Carpeta carpeta in carpetas)
            {
                foreach (TipoPlano tipo in tipos)
                {
                    if (carpeta.Name == tipo.Categoria && carpeta.IdObra == tipo.IdObra)
                    {
                        carpeta.IdTipo = tipo.Id;
                        Context.SaveChanges();
                        break;
                    }
                }
            }
        }

        private IEnumerable<Carpeta> TomarTodasCarpetas()
        {
            return Context.Carpetas;
        }

        internal Carpeta BuscarRoot(Obra obra)
        {
            return Context.Carpetas.Where(c => c.IdObra == obra.IdObra && c.Anterior == null).FirstOrDefault();
        }

        internal List<Carpeta> CarpetasPosteriores(Carpeta carpeta)
        {
            return Context.Carpetas.Where(c => c.Anterior.Name == carpeta.Name && c.Obra.IdObra == carpeta.IdObra).ToList();
        }

        internal void LimpiarCarpetasYTipos(int idObra)
        {
            //se hace en orden para no tener dependencias
            List<Plano> planos = Context.Planos.Where(p => p.IdObra == idObra).ToList();
            List<Carpeta> carpetas = Context.Carpetas.Where(p => p.IdObra == idObra).ToList();
            List<TipoPlano> tipos = Context.TiposPlanos.Where(p => p.IdObra == idObra).ToList();
            foreach (Plano p in planos)
            {
                Context.Planos.Remove(p);
                Context.SaveChanges();
            }
            foreach (TipoPlano t in tipos)
            {
                Context.TiposPlanos.Remove(t);
                Context.SaveChanges();
            }
            foreach (Carpeta c in carpetas)
            {
                Context.Carpetas.Remove(c);
                Context.SaveChanges();
            }

        }
    }
}
