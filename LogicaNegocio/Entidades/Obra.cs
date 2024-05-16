using LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    //Documentar cambios en requerimientos: Listado de Obras, Vista de Obras, Filtro de Obras
    public class Obra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdObra { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Direccion { get; set; }
        public bool Finalizada { get; set; }
        public List<Material> MaterialesSolicitados { get; set; }
        public List<Material> MaterialesObra { get; set; }
        public List<Plano> Planos { get; set; }
        public List<Solicitud> Solicitudes { get; set; }

        public Obra()
        {
            
        }

        public void Validar()
        {
            throw new NotImplementedException();
        }

        public void FinalizarObra()
        {
            if (!this.Finalizada)
            {
                throw new ObraException("La obra ya ha sido finalizada.");
            }
            if (this.TieneSolicitudesPendientes())
            {
                throw new ObraException("No se puede cerrar la obra, tiene solicitudes de material en estado pendiente.");
            }
            this.Finalizada = true;
        }

        public bool TieneSolicitudesPendientes()
        {
            bool haySolicitudes = false;
            foreach(Solicitud s in Solicitudes)
            {
                if(s.Aprovador != null)
                {
                    haySolicitudes = true;
                }
            }
            return haySolicitudes;
        }
    }
}
