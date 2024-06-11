using LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Stock { get; set;}
        [Required(ErrorMessage = "Ingrese una unidad de medida")]
        public string UnidadDeMedida { get; set; }

    
        public Material(string nom, int stock, string unidadDeMedida)
        {
            this.Nombre = nom;
            this.Stock = stock;
            this.UnidadDeMedida = unidadDeMedida;

        }

        public Material()
        {
            this.Stock = 0;
        }

        public void Validar()
        {
            if(Stock < 0)
            {
                throw new MaterialException("El stock no puede ser negativo");
            }
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
