using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class UDeObra : Usuario
    {
        public List<Obra> ObrasACargo { get; set; }
    }
}
