using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class UDeObra : Usuario
    {
        public UDeObra(string nombre, string nomUsuario, string pass) : base(nombre, nomUsuario, pass)
        {
        }

        public UDeObra() : base()
        {
            
        }

        public override string Tipo { get => "Usuario de obra"; }
    }
}
