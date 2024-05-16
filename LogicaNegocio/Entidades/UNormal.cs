using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class UNormal : Usuario
    {
        public UNormal(string nombre, string nomUsuario, string pass) : base(nombre, nomUsuario, pass)
        {
        }

        public UNormal() : base()
        {
            
        }
    }
}
