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
        [Required(ErrorMessage = "Ingrese una cedula")]
        public string Cedula { get; set; }
        public DateTime? FechaIngreso { get; set; }
        [ForeignKey("TipoEmpleado")] public int? IdTipoEmpleado { get; set; }
        [Required(ErrorMessage = "Ingrese un tipo de empleado")]
        public TipoEmpleado TipoEmpleado { get; set; }
        public string? CuentaBanco { get; set; }
        public string? Banco { get; set; }
        public double? IncentivoXHora { get; set; }


        public Empleado()
        {

        }

        public void Validar()
        {
            ValidarCuentaDeBanco(); //Por banco o algo en especial? Numeros y letras pero no caract?
            ValidarFechaIngreso();
            ValidarCi();
        }

        private void ValidarCi()
        {
            Cedula = Cedula.Replace("-", "").Replace(" ", "");

            // Verificar que tenga exactamente 8 dígitos
            if (Cedula.Length != 8)
            {
                throw new EmpleadoException("La CI no puede tener mas de 8 digitos");

            }

            // Separar los dígitos de la cédula
            string numCedula = Cedula.Substring(0, 7);
            int digitoVerificador;
            if (!int.TryParse(Cedula.Substring(7, 1), out digitoVerificador))
            {
                throw new EmpleadoException("La CI no es valida");
            }

            // Factores para la multiplicación
            int[] factores = { 2, 9, 8, 7, 6, 3, 4 };

            // Calcular la suma de los productos
            int suma = 0;
            for (int i = 0; i < 7; i++)
            {
                int digito;
                if (!int.TryParse(numCedula[i].ToString(), out digito))
                {
                    throw new EmpleadoException("La CI no es valida");
                }
                suma += digito * factores[i];
            }

            // Calcular el módulo 10 de la suma
            int modulo = suma % 10;

            // Calcular el dígito verificador
            int calculado = 10 - modulo;
            if (calculado == 10)
            {
                calculado = 0;
            }

            // Comparar el dígito verificador calculado con el dígito verificador original
            if (calculado != digitoVerificador)
            {
                throw new EmpleadoException("La CI no es valida");
            }

        }

        public void ValidarCuentaDeBanco()
        {

        }

        public void ValidarFechaIngreso()
        {
            if (this.FechaIngreso > DateTime.Now)
            {
                throw new EmpleadoException("La fecha de ingreso no puede ser despues de hoy");
            }

        }
    }
}
