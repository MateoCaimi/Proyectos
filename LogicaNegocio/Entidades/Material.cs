using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre para el material")]
        public string Nombre { get; set; }
        public int Stock { get; set;}
        [Required(ErrorMessage = "Ingrese una unidad de medida")]
        public string UnidadDeMedida { get; set; }

        public Material()
        {
            this.Stock = 0;
        }

    }
}
