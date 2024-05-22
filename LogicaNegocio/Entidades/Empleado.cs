using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class Empleado : IValidable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
        [ForeignKey("TipoEmpleado")] public int IdEmpleado { get; set; }
        [Required(ErrorMessage = "Ingrese un tipo de empleado")]
        public TipoEmpleado? TipoEmpleado { get; set; }
        public string CuentaBanco { get; set; }
        public string Banco { get; set; }


        //Lista de Obras se va
        public List<Obra> Obras { get; set; }

        public Empleado()
        {
            
        }

        public void Validar()
        {
            ValidarCuentaDeBanco(); //Por banco o algo en especial? Numeros y letras pero no caract?
            ValidarFechaIngreso();
        }

        public void ValidarCuentaDeBanco()
        {

        }

        public void ValidarFechaIngreso()
        {
            if(this.FechaIngreso > DateTime.Now)
            {
                throw new EmpleadoException("La fecha de ingreso no puede ser despues de hoy");
            }

        }
    }
}
