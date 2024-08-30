using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace LogicaNegocio.Entidades
{
    public class Proveedor : IValidable
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }

        public Proveedor(string nom, string tel, string mail)
        {

            this.Nombre = nom;
            this.Telefono = tel;
            this.Mail = mail;

        }

        public Proveedor() { }

        public void ValidarMail()
        {

            string regla = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(this.Mail, regla))
            {
                throw new ProveedorException("El correo electrónico no tiene un formato válido.");
            }
        }

        public void ValidarTelefono()
        {
            //Validar celular y telefono para montevideo?
        }

        public void Validar()
        {
            ValidarMail();
            ValidarTelefono();
        }

    }
}
