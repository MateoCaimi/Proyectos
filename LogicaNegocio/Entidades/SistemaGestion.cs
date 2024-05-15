using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class SistemaGestion
    {
        public List<Obra> ObrasTotales { get; set; }
        public List<Obra> ObrasActuales { get; set; }
        public List<Empleado> Empleados { get; set; }
        public List<Proveedor> Proveedores { get; set; }
    }
}
