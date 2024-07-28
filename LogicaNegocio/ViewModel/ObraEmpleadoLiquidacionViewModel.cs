using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LogicaNegocio.ViewModel
{
    [Serializable]
    public class ObraEmpleadoLiquidacionViewModel
    {
        public double Liquidacion {  get; set; }
        public ObraEmpleado? ObraEmpleado {  get; set; }

        public ObraEmpleadoLiquidacionViewModel(ObraEmpleado obraEmp, double liq)
        {
            this.Liquidacion = liq;
            this.ObraEmpleado = obraEmp;
        }
        public ObraEmpleadoLiquidacionViewModel()
        {

        }
    }
}
