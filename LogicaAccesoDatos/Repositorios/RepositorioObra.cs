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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioObra : IRepositorioObra
    {
        public ProyectoContext Context { get; set; }

        public RepositorioObra()
        {
            this.Context = new ProyectoContext();
        }
        public void Agregar(Obra item)
        {
            item.Validar(); //En el validar fijarse que la fecha no sea posterior a hoy.
            if(this.ObraPorNombre(item.Nombre) != null)
            {
                throw new ObraException("El nombre de obra ingresado ya está en uso. Elegir otro.");
            }
            if (this.ObraPorDireccion(item.Direccion) != null)
            {
                throw new ObraException("La ubicación de obra ingresada coincide con una existente. Elegir otra.");
            }
            Context.Obras.Add(item);
            Context.SaveChanges();
        }

        public Obra ObraPorNombre(string nombre)
        {
            var Retorno = Context.Obras.Where(o => o.Nombre == nombre).FirstOrDefault();
            return Retorno;
        }

        public Obra ObraPorDireccion(string direccion)
        {
            var Retorno = Context.Obras.Where(o => o.Direccion == direccion).FirstOrDefault();
            return Retorno;
        }

        public IEnumerable<Obra> BuscarPorDireccion(string direccion)
        {
            var Retorno = Context.Obras.Where(o => o.Direccion == direccion).ToList();
            return Retorno;
        }

        public IEnumerable<Obra> BuscarPorNombre(string nombre)
        {
            var Retorno = Context.Obras.Where(o => o.Nombre == nombre).ToList();
            return Retorno;
        }

        public void Eliminar(Obra obra)
        {
            if(obra == null)
            {
                throw new ObraException("No se puede eliminar una obra nula.");
            }
            if (obra.TieneSolicitudesPendientes())
            {
                throw new ObraException("No se puede eliminar la obra, tiene solicitudes de material en estado pendiente.");
            }
            Context.Obras.Remove(obra);
            Context.SaveChanges();
        }

        public void EliminarPlano(Plano plano, Obra obra)
        {
            if (obra == null)
            {
                throw new ObraException("No se puede eliminar un plano de una obra nula.");
            }
            if (plano == null)
            {
                throw new ObraException("No se puede eliminar un plano nulo.");
            }
            try
            {   //No me convence. Repositorio de planos????? No entiendo bien como hacerlo, el tema es que no tenemos referencia de obra en plano.
                obra.Planos.Remove(plano);
                Context.Planos.Remove(plano);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new ObraException("Error eliminando plano: " + e.Message);
            }
        }

        public void FinalizarObra(Obra obra)
        {
            if (obra == null)
            {
                throw new ObraException("No se puede finalizar una obra nula.");
            }
            try
            {
                obra.FinalizarObra();
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new ObraException($"No se puede finalizar la obra {obra.Nombre}: ", e);
            }
        }

        public void IngresarPlano(Plano plano, Obra obra)
        {
            if (obra == null)
            {
                throw new ObraException("No se puede ingresar un plano en una obra nula.");
            }
            if (plano == null)
            {
                throw new ObraException("No se puede ingresar un plano nulo.");
            }
            try
            {   //No me convence. Repositorio de planos????? No entiendo bien como hacerlo, el tema es que no tenemos referencia de obra en plano.
                plano.Validar();
                obra.Planos.Add(plano);
                Context.Planos.Add(plano);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new ObraException("Error ingresando plano: " + e.Message);
            }
        }

        public void Modificar(Obra nuevaObra)
        {
            try
            {
                nuevaObra.Validar();
                Obra obra = this.Buscar(nuevaObra.IdObra);
                if (obra == null)
                {
                    throw new ObraException("No se encontró la obra a modificar.");
                }

                obra.Nombre = nuevaObra.Nombre;
                obra.Direccion = nuevaObra.Direccion;
                obra.FechaInicio = nuevaObra.FechaInicio;
                obra.FechaFinalizacion = nuevaObra.FechaFinalizacion;
                Context.Entry(obra).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ObraException(ex.Message);
            }
        }

        public IEnumerable<Obra> ObrasFiltradas(string? nombre, string? direccion, bool? finalizada)
        {
            IEnumerable<Obra> obras = this.TomarTodos();
            if(nombre != null)
            {
                obras = obras.Where(o => o.Nombre.Contains(nombre));
            }
            if(direccion != null)
            {
                obras = obras.Where(o => o.Nombre.Contains(nombre));
            }
            if(finalizada != null)
            {
                obras = obras.Where(o => o.Finalizada == finalizada);
            }
            return obras.ToList();
        }

        public IEnumerable<Plano> PlanosFiltrados(Obra obra, TipoPlano? tipo, string? nombre, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            if (obra == null)
            {
                throw new ObraException("No se pueden buscar planos en una obra nula.");
            }
            IEnumerable<Plano> planos = obra.Planos; //creo que es descendiente, revisar luego
            if(tipo != null)
            {
                planos = planos.Where(p => p.TipoPlano.Id == tipo.Id);
            }
            if(nombre != null)
            {
                planos = planos.Where(p => p.Nombre.Contains(nombre));
            }
            if(fechaDesde != null && fechaHasta != null)
            {
                if(fechaDesde > fechaHasta)
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

        public IEnumerable<Plano> PlanosTotales(int id)
        {
            Obra obra = Buscar(id);
            if (obra == null)
            {
                throw new ObraException("No se pueden buscar planos en una obra nula.");
            }
            IEnumerable<Plano> planos = obra.Planos;
            return planos;
        }


        public IEnumerable<Obra> TomarTodos()
        {
            return Context.Obras.ToList();
        }

        public Obra Buscar(int id)
        {
            return Context.Obras.Find(id);
        }
    }
}
