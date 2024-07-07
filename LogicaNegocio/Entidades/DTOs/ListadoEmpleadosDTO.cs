using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades.DTOs
{
    public class ListadoEmpleadosDTO
    {
        public int CantidadEmpleados {  get; set; }
        public List<EmpleadoDTO> Empleados { get; set; }
    }
}
