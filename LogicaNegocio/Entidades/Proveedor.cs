using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Excepciones;
using System.Text.RegularExpressions;

namespace LogicaNegocio.Entidades
{
    public class Proveedor
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required] 
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }

        public Proveedor(string nom, string tel, string mail) {
            
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



    }
}
