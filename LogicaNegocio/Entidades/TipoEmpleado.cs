using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace LogicaNegocio.Entidades
{
    public class TipoEmpleado
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Categoría { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        public double ValorHora { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        public double Presentismo { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        public double Compensacion { get; set; }

        public TipoEmpleado()
        {
            
        }

    }
}
