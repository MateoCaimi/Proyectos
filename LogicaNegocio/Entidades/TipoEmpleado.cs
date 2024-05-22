using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using LogicaNegocio.Interfaces;
using LogicaNegocio.Excepciones;

namespace LogicaNegocio.Entidades
{
    public class TipoEmpleado: IValidable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese una categoria")]
        public string Categoría { get; set; }
        [Required(ErrorMessage = "Ingrese un valor por hora")]
        [Range(0,double.MaxValue)]
        public double ValorHora { get; set; }
        [Required(ErrorMessage = "Ingrese un presentismo")]
        [Range(0,double.MaxValue)]
        public double Presentismo { get; set; }
        [Required(ErrorMessage = "Ingrese una compensacion")]
        [Range(0,double.MaxValue)]
        public double Compensacion { get; set; }

        public TipoEmpleado()
        {
            
        }

        public void Validar()
        {
            ValidarValorHora();
            ValidarPresentismo();
            ValidarCompensacion();

        }

        public void ValidarValorHora()
        {
            if (this.ValorHora <= 0)
            {
                throw new EmpleadoException("El valor debe ser mayor a 0");
            }
        }

        public void ValidarPresentismo()
        {
            if (this.ValorHora <= 0)
            {
                throw new EmpleadoException("El valor debe ser mayor a 0");
            }
        }

        public void ValidarCompensacion()
        {
            if (this.Compensacion <= 0)
            {
                throw new EmpleadoException("El valor debe ser mayor a 0");
            }
        }






    }
}
