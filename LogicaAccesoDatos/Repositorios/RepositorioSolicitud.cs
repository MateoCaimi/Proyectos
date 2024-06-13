using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioSolicitud : IRepositorioSolicitud
    {
        public ProyectoContext Context { get; set; }

        public RepositorioSolicitud()
        {
            this.Context = new ProyectoContext();
        }

        public void Agregar(Solicitud item)
        {
            throw new NotImplementedException();
        }

        public Solicitud Buscar(int id)
        {
            return Context.Solicitudes.Find(id); 
        }

        public void Eliminar(Solicitud item)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Solicitud item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Solicitud> TomarTodos()
        {
            return Context.Solicitudes.ToList();
        }

        public IEnumerable<Solicitud> SolicitudesDeObra(int idObra)
        {
            return Context.Solicitudes.Where(s=>s.IdObra == idObra);
        }
    }
}
