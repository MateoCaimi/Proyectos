using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class Empleado
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
        [ForeignKey("TipoEmpleado")] public int IdEmpleado { get; set; }
        [Required]
        public TipoEmpleado? TipoEmpleado { get; set; }
        public string CuentaBanco { get; set; }
        public string Banco { get; set; }
        public List<Obra> Obras { get; set; }

        public Empleado()
        {
            
        }

    }
}
