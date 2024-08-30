using LogicaNegocio.Entidades;

namespace LogicaNegocio.ViewModel
{
    [Serializable]
    public class ObraEmpleadoLiquidacionViewModel
    {
        public decimal Liquidacion { get; set; }
        public ObraEmpleado? ObraEmpleado { get; set; }

        public ObraEmpleadoLiquidacionViewModel(ObraEmpleado obraEmp, decimal liq)
        {
            this.Liquidacion = liq;
            this.ObraEmpleado = obraEmp;
        }
        public ObraEmpleadoLiquidacionViewModel()
        {

        }
    }
}
