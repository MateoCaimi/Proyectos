using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioPlano : IRepositorio<Plano>
    {
        public IEnumerable<Plano> PlanosTotales(Obra obra);
        public IEnumerable<Plano> PlanosFiltrados(Obra obra, int? tipo, string? nombre, DateTime? fechaDesde, DateTime? fechaHasta);
        public IEnumerable<Plano> PlanosPorAntiguedad(Obra obra);
        public IEnumerable<TipoPlano> BuscarTiposPlanos();
        public IEnumerable<Plano> BuscarCarpeta(string carpeta);

    }
}
