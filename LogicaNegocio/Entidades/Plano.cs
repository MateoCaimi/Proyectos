using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Interfaces;
using System.Runtime.CompilerServices;

namespace LogicaNegocio.Entidades
{
    public class Plano : IValidable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Ingrese un nombre")] LO COMENTO PARA HACER QUE SI EL NOMBRE ES VACÍO, SE USA EL DEL ARCHIVO.
        public string? Nombre { get; set; }

        [ForeignKey("IdTipoPlano")] 
        public TipoPlano? TipoPlano { get; set; }
        public int IdTipoPlano { get; set; }
        [Required]
        public DateTime FechaPublicado { get; set; }

        [ForeignKey("IdObra")] 
        public Obra? Obra { get; set; }
        public int IdObra { get; set; }
        public string NombrePdf { get; set; }
        public string TipoPdf { get; set; }
        public byte[] Pdf { get; set; }
        public Carpeta CarpetaContenedora { get; set; }


        public Plano()
        {
            this.FechaPublicado = DateTime.Now;
        }

        public void Validar()
        {

        }
    }
}
