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
        public void IngresarPlano(Plano plano, Obra obra);
        public void EliminarPlano(Plano plano, Obra obra);
        public Obra ObraPorDireccion(string direccion);
        public Obra ObraPorNombre(string nombre);
        public IEnumerable<Plano> PlanosTotales(Obra obra);
        public IEnumerable<Plano> PlanosFiltrados(Obra obra, TipoPlano? tipo, string? nombre, DateTime? fechaDesde, DateTime? fechaHasta);
        public IEnumerable<Plano> PlanosPorAntiguedad(Obra obra);
        public IEnumerable<Obra> ObrasFiltradas(string? nombre, string? direccion, bool? finalizada);
        public IEnumerable<Obra> BuscarPorNombre(string nombre);
        public IEnumerable<Obra> BuscarPorDireccion(string direccion);
        public void FinalizarObra(Obra obra);

    }
}
