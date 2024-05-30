using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;

namespace LogicaNegocio.Entidades
{
    public abstract class Usuario : IValidable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre de usuario")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Ingrese una contraseña")]
        [MinLength(8)]
        public string Contrasenia { get; set; }
        public abstract string Tipo { get; }


        public Usuario(string nombre, string nomUsuario, string pass)
        {
            this.Nombre = nombre;
            this.NombreUsuario= nomUsuario;
            this.Contrasenia = pass;
        }

        public Usuario()        
        {
            
        }

        

        public void ValidarContrasena()
        {
            //Verifico si la password tiene mas de 8 caracteres, si tiene una mayuscula y si tiene un numero. Si la password es igual para todos no hay que rescribir en clases hijas.


            //falta caracter especial
            if (this.Contrasenia.Length >= 8 && this.Contrasenia.Any(char.IsUpper) && this.Contrasenia.Any(char.IsDigit))
            {
                
            }
            else
            {
                throw new UsuarioException("La contrasenia debe tener al menos 8 caracteres, 1 mayuscula y 1 numero");
            }
        }
        public void ValidarNombre()
        {
            bool num = true;
            for (var i = 0; i < this.Nombre.Length; i++)
            {
                if (char.IsDigit(Nombre[i]))
                {
                    num = false;
                }
            }
            if (!num)
            {
                throw new UsuarioException("El nombre no puede contener digitos numericos");
            }
        }



        public void Validar()
        {
            ValidarNombre();
            ValidarContrasena();
        }
    }
}
