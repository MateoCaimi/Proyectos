using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioSolicitud : IRepositorio<Solicitud>
    {
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Solicitud> SolicitudesDeObra(System.Int32 idObra);
    }
}
