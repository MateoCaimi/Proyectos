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
        public Obra BuscarPorNombre(string nombre);
        public IEnumerable<Obra> BuscarPorDireccion(string direccion);
        public void FinalizarObra(Obra obra);
        public Material MaterialMenosSolicitado(int IdObra);
        public Material MaterialMasSolicitado(int IdObra);
        public Proveedor ProveedorMasComun(int IdObra);
        public Usuario SolicitanteMasComun(int IdObra);
        public UDeOficina AprobadorMasComun(int IdObra);

    }
}
