using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    public class Solicitud
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [ForeignKey("Proveedor")] public int IdProveedor { get; set; }
        public Proveedor? Proveedor { get; set; }
        [ForeignKey("Obra")] public int IdObra { get; set; }
        [Required]
        public Obra? Obra { get; set; }
        [ForeignKey("Usuario")] public int IdUsuario { get; set; }
        [Required]
        public Usuario? Solicitante { get; set; }
        [ForeignKey("UDeOficina")] public int IdUDeOficina { get; set; }
        public UDeOficina? Aprovador { get; set; }
    }
}
