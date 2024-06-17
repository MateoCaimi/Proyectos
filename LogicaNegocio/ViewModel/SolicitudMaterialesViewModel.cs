using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.ViewModel
{
    public class SolicitudMaterialesViewModel
    {
        public List<SolicitudMaterial> MainMaterials { get; set; }
        public List<SolicitudMaterial> TempMaterials { get; set; }
    }
}
