using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Excepciones;

namespace LogicaNegocio.Entidades
{
    public abstract class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        [MinLength(8)]
        public string Contrasenia { get; set; }


        protected Usuario(string nombre, string nomUsuario, string pass)
        {
            this.Nombre = nombre;
            this.NombreUsuario= nomUsuario;
            this.Contrasenia = pass;
        }

        protected Usuario()
        {
            
        }



        public void ValidarContrasena()
        {
            //Verifico si la password tiene mas de 8 caracteres, si tiene una mayuscula y si tiene un numero. Si la password es igual para todos no hay que rescribir en clases hijas.

            if (this.Contrasenia.Length >= 8 && this.Contrasenia.Any(char.IsUpper) && this.Contrasenia.Any(char.IsDigit))
            {
                
            }
            else
            {
                throw new UsuarioException("La contrasenia debe tener almenos 8 caracteres, 1 mayuscula y 1 caracter especial");
            }
        }


    }
}
