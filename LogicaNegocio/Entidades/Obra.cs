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
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]

        public DateTime FechaInicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaFinalizacion { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre valido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese una direccion")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
        public bool Finalizada { get; set; }
        [Display(Name = "Usuario a cargo")]
        [ForeignKey("UsuarioACargo")] public int IdACargo { get; set; }

        public UDeObra UsuarioACargo { get; set; }
        public string? NombreCronograma { get; set; }
        public string? TipoCronograma { get; set; }
        public byte[]? Cronograma { get; set; }
        public byte[]? QR { get; set; }

        public DateTime? UltimaActualizacion { get; set; }

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

        public Obra(int idObra, DateTime fechaInicio, DateTime fechaFinalizacion, string nombre, string direccion, bool finalizada, int idUACargo)
        {
            IdObra = idObra;
            FechaInicio = fechaInicio;
            FechaFinalizacion = fechaFinalizacion;
            Nombre = nombre;
            Direccion = direccion;
            Finalizada = finalizada;
            IdACargo = idUACargo;
        }

        public void Validar()
        {
            ValidarFechaFinal();
            ValidarNombre();
            ValidarDireccion();
            ValidarUACargo();
        }

        private void ValidarUACargo()
        {
            if (IdACargo == 0)
            {
                throw new ObraException("Se debe asignar un usuario obra a cargo.");

            }
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

        public override string ToString()
        {
            return this.Nombre;
        }
 

    }
}
