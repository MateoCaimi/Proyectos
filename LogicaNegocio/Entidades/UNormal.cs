using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class UNormal : Usuario
    {
        public override string Tipo { get => "Usuario normal"; }

        public UNormal(string nombre, string nomUsuario, string pass) : base(nombre, nomUsuario, pass)
        {
        }

        public UNormal() : base()
        {
            
        }


    }
}
