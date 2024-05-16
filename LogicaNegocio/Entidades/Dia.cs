using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class Dia
    {
        [Key]
        public DateTime Fecha { get; set; }
        public int Horas { get; set; }

        public Dia()
        {
            
        }
    }
}
