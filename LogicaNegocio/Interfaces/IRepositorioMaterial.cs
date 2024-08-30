using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioMaterial : IRepositorio<Material>
    {

        public Material MaterialPorNombre(string nombre);
        public bool MaterialSeEncuentraEnObra(Material m);
        public IEnumerable<Material> TomarTodos();


    }
}
