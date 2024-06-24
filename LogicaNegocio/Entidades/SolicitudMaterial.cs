using LogicaNegocio.Excepciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    [PrimaryKey(nameof(IdSolicitud), nameof(IdMaterial))]
    public class SolicitudMaterial
    {

        [ForeignKey("Solicitud")]
        public int IdSolicitud { get; set; }
        public Solicitud Solicitud { get; set; }

        [ForeignKey("Material")]
        public int IdMaterial { get; set; }
        public Material Material { get; set; }

        [Required]
        public int Cantidad { get; set; }

        public SolicitudMaterial()
        {
            
        }

        public SolicitudMaterial(Solicitud solicitud, Material material, int cantidad)
        {
            this.Solicitud = solicitud;
            this.IdSolicitud = solicitud.Id;
            this.Material = material;
            this.Material.Id = material.Id;
            this.Cantidad = cantidad;

        }

        public void Validar()
        {
            if(this.Cantidad <= 0)
            {
                throw new SolicitudException("La cantidad no puede ser 0.");
            }
        }

    }
}
