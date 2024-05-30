using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class UAdministrador : Usuario
    {
        public UAdministrador(string nombre, string nomUsuario, string pass) : base(nombre, nomUsuario, pass)
        {
        }

        public UAdministrador() : base()
        {
            
        }

        public override string Tipo { get => "Usuario administrador"; }
    }
}
