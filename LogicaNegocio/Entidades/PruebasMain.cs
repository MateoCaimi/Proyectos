using LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    class Program
    {
        
        public void Main()
        {

            ProbarValidacionContrasena();

        }

        public void ProbarValidacionContrasena()
        {
            Usuario admin = new UAdministrador("Ricardo", "admin", "Admin1234");
            Usuario oficina = new UDeObra("Ricardo", "oficina", "ofic123");


            // Prueba con contraseña válida
            try
            {
                admin.ValidarContrasena();
                Console.WriteLine("Paso la prueba");
            }
            catch (UsuarioException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Prueba con contraseña inválida
            try
            {
                oficina.ValidarContrasena();
                Console.WriteLine("Paso la prueba");
            }
            catch (UsuarioException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




    }


}
