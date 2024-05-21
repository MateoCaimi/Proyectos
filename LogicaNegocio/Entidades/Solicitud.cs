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
    public class Solicitud : IValidable
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
        public List<Material> MaterialesSolicitados { get; set; }

        public Estado Estado { get; set; }
       



        public Solicitud (int id, int idProveedor, Proveedor? proveedor, int idObra, Obra? obra, int idUsuario, Usuario? solicitante, int idUDeOficina, UDeOficina? aprovador, Estado estado)
        {
            Id = id;
            IdProveedor = idProveedor;
            Proveedor = proveedor;
            IdObra = idObra;
            Obra = obra;
            IdUsuario = idUsuario;
            Solicitante = solicitante;
            IdUDeOficina = idUDeOficina;
            Aprovador = aprovador;
            Estado = estado;

        }

        public Solicitud () { }

        public void Validar()
        {

        }
    }
}
