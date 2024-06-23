using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.ViewModel
{
    public class MaterialConsumoViewModel
    {
        public Material Material { get; set; }
        public int IdMaterial {  get; set; }
        public int Cantidad {  get; set; }
    }
}
