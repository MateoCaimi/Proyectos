using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
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
    public class Obra : IValidable
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
            ValidarFechaInicio();
        }

        public void ValidarFechaInicio()
        {
            if(this.FechaInicio < DateTime.Now)
            {
                throw new ObraException("La fecha de la obra no puede ser antes de hoy");
            }
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
            if (Solicitudes == null)
            {
                return haySolicitudes;
            }
            foreach(Solicitud s in Solicitudes)
            {
                if(s.Aprovador != null)
                {
                    haySolicitudes = true;  //No me parece esto
                }
            }
            return haySolicitudes;
        }
 

    }
}
