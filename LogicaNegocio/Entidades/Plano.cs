using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Interfaces;

namespace LogicaNegocio.Entidades
{
    public class Plano : IValidable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }

        [ForeignKey("IdTipoPlano")] 
        public TipoPlano? TipoPlano { get; set; }
        public int IdTipoPlano { get; set; }
        [Required]// Esto no es automatico?
        public DateTime FechaPublicado { get; set; }

        [ForeignKey("IdObra")] 
        public Obra? Obra { get; set; }
        public int IdObra { get; set; }

        // archivo img o lo que sea IMPORTANTE
        // IMG
        public string NombreImagen { get; set; }
        public string TipoImagen { get; set; }
        public byte[] Imagen { get; set; }

        // IMG
        public Plano()
        {
            this.FechaPublicado = DateTime.Now;
        }

        public void Validar()
        {

        }
    }
}
