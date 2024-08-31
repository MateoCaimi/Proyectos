using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
    public class Empleado : IValidable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese una cédula")]
        [Display(Name = "Cédula")]

        public string Cedula { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de ingreso")]

        public DateTime? FechaIngreso { get; set; }
        [ForeignKey("TipoEmpleado")] public int? IdTipoEmpleado { get; set; }
        [Required(ErrorMessage = "Ingrese un tipo de empleado")]
        public TipoEmpleado TipoEmpleado { get; set; }
        public string? CuentaBanco { get; set; }
        public string? Banco { get; set; }
        public decimal IncentivoXHora { get; set; }

        public bool Activo { get; set; }


        public Empleado()
        {

        }

        public void Validar()
        {
            ValidarCuentaDeBanco(); //Por banco o algo en especial? Numeros y letras pero no caract?
            ValidarFechaIngreso();
            ValidarCi();
        }

        private bool SoloDigitos(string str)
        {
            return str.All(c => c >= '0' && c <= '9');
        }

        private void ValidarCi()
        {
            if(this.Cedula.Length >= 9 || this.Cedula.Length <= 6)
            {
                throw new EmpleadoException("La cédula debe tener 7 u 8 carácteres.");
            }
            if(!this.SoloDigitos(this.Cedula)){ throw new EmpleadoException("La cédula solo puede contener digitos"); }
            /*Cedula = Cedula.Replace("-", "").Replace(" ", "");

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
            }*/

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

        public void ValidarNulos()
        {
            if (this.FechaIngreso == null)
            {
                throw new EmpleadoException("Debe seleccionar una fecha de ingreso.");
            }
            if (this.CuentaBanco == null)
            {
                throw new EmpleadoException("Debe ingresar una cuenta bancaria.");
            }
            if (this.Banco == null)
            {
                throw new EmpleadoException("Debe ingresar el banco de la cuenta.");
            }
            if (this.IdTipoEmpleado == null || this.IdTipoEmpleado == 0)
            {
                throw new EmpleadoException("Debe ingresar la categoría del empleado.");
            }
        }
    }
}
