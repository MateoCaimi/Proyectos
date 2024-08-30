using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioMaterial : IRepositorio<Material>
    {
        LogicaNegocio.Entidades.Material MaterialPorNombre(System.String nombre);
        System.Boolean MaterialSeEncuentraEnObra(LogicaNegocio.Entidades.Material material);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Material> MaterialesFiltados(System.String nombre);
    }
}
