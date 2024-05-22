using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioPlano : IRepositorioPlano
    {

        public ProyectoContext Context { get; set; }
        private IRepositorioObra repositorioObra;

        public RepositorioPlano()
        {
            this.Context = new ProyectoContext();
            repositorioObra = new RepositorioObra();
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
            try
            {
                Obra obra = repositorioObra.Buscar(plano.IdObra);
                plano.Validar();
                Context.Planos.Add(plano);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new ObraException("Error ingresando plano: " + e.Message);
            }
        }

        public void Eliminar(Plano plano)
        {
            Obra obra = repositorioObra.Buscar(plano.IdObra);
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
                planos = planos.Where(p => p.Nombre.Contains(nombre));
            }
            if (fechaDesde != null && fechaHasta != null)
            {
                if (fechaDesde > fechaHasta)
                {
                    DateTime aux = (DateTime)fechaHasta;
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
            IEnumerable<Plano> planos = obra.Planos.OrderByDescending(p => p.FechaPublicado); //creo que es descendiente, revisar luego
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
            throw new NotImplementedException();
        }

    }
}
