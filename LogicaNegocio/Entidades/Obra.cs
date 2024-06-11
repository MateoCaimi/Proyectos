using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
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

        [Required(ErrorMessage = "Ingrese una fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre valido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese una direccion")]
        public string Direccion { get; set; }
        public bool Finalizada { get; set; }
        [ForeignKey("UsuarioACargo")] public int IdACargo { get; set; }
        public UDeObra UsuarioACargo { get; set; }
        public string NombreCronograma { get; set; }
        public string TipoCronograma { get; set; }
        public byte[]? Cronograma { get; set; }
        public byte[]? QR { get; set; }

        //Pruebas de listas
        public List<Material> Materiales { get; set; } = new List<Material>();
        public Obra()
        {

        }

        public Obra(int idObra, DateTime fechaInicio, DateTime fechaFinalizacion, string nombre, string direccion, bool finalizada)
        {
            IdObra = idObra;
            FechaInicio = fechaInicio;
            FechaFinalizacion = fechaFinalizacion;
            Nombre = nombre;
            Direccion = direccion;
            Finalizada = finalizada;
        }

        public void Validar()
        {
            ValidarFechaFinal();
            ValidarNombre();
            ValidarDireccion();
        }


      

        public void ValidarFechaFinal()
        {
            if(this.FechaFinalizacion != null)
            {
                if (this.FechaFinalizacion < this.FechaInicio)
                {
                    throw new ObraException("La fecha de finalizacion no puede ser previa a la de inicio");
                }
            }
        }

        public void ValidarNombre()
        {
            if (this.Nombre.Any(char.IsDigit))
            {
                throw new ObraException("El nombre no puede contener numeros");
                //Podriamos preguntar por las dudas
            }

        }

        public void ValidarDireccion()
        {
            
            if (this.Direccion.All(char.IsDigit))
            {
                throw new ObraException("Debe escribir una direccion");
            }
        }

        public void FinalizarObra()
        {
            if (this.Finalizada)
            {
                throw new ObraException("La obra ya ha sido finalizada.");
            }
            this.Finalizada = true;
            this.FechaFinalizacion = DateTime.Now; //Hacer esto? Y solo a las finalizadas poder setear manualmente la finalización.
        }


 

    }
}
