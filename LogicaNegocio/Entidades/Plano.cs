using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class Plano
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [ForeignKey("TipoPlano")] public int IdPlano { get; set; }
        public TipoPlano? TipoPlano { get; set; }
        [Required]
        public DateTime FechaPublicado { get; set; }


        public Plano()
        {
            this.FechaPublicado = DateTime.Now;
            
        }
    }
}
