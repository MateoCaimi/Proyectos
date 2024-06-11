using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioMaterial : IRepositorio<Material>
    {

        public Material MaterialPorNombre(string nombre);
        public bool MaterialSeEncuentraEnObra(Material m);
        public IEnumerable<Material> TomarTodos();


    }
}
