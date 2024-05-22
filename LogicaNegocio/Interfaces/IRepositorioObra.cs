using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Interfaces
{
    
    public interface IRepositorioObra : IRepositorio<Obra>
    {
        public Obra ObraPorDireccion(string direccion);
        public Obra ObraPorNombre(string nombre);
        public IEnumerable<Obra> ObrasFiltradas(string? nombre, string? direccion, bool? finalizada);
        public IEnumerable<Obra> BuscarPorNombre(string nombre);
        public IEnumerable<Obra> BuscarPorDireccion(string direccion);
        public void FinalizarObra(Obra obra);

    }
}
