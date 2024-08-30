using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
    public class TipoEmpleado : IValidable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese una categoria")]
        public string Categoría { get; set; }
        [Required(ErrorMessage = "Ingrese un valor por hora")]
        [Display(Name = "Valor por Hora")]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "El campo Valor por Hora debe ser un número válido con coma como separador decimal.")]

        [Range(0, double.MaxValue)]
        public decimal ValorHora { get; set; }
        [Range(0, double.MaxValue)]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "El campo Valor por Hora debe ser un número válido con coma como separador decimal.")]

        public decimal Presentismo { get; set; }
        [Range(0, double.MaxValue)]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "El campo Valor por Hora debe ser un número válido con coma como separador decimal.")]


        public decimal Compensacion { get; set; }

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
