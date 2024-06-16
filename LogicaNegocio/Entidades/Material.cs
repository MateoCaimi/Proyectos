using LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LogicaNegocio.Entidades
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre para el material")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese una unidad de medida")]
        public string UnidadDeMedida { get; set; }

    
        public Material(string nom, int stock, string unidadDeMedida)
        {
            this.Nombre = nom;
            this.UnidadDeMedida = unidadDeMedida;

        }

        public Material()
        {
        }

        public void Validar()
        {
           
            if(this.Nombre == null)
            {
                throw new MaterialException("Ingrese nombre de material");
            }
            if (this.UnidadDeMedida == null)
            {
                throw new MaterialException("Ingrese la unidad de medida");
            }
            
        }
    }
}
