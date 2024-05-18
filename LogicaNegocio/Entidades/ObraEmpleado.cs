using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
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
    [PrimaryKey(nameof(IdObra), nameof(IdEmpleado))]
    public class ObraEmpleado : IValidable
    {
        [ForeignKey("Obra")]
        public int IdObra { get; set; }
        [ForeignKey("Empleado")]
        public int IdEmpleado { get; set; }
        public List<Dia> Dias { get; set; }
        [Required]
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEgreso { get; set; }

        public ObraEmpleado()
        {
            
        }

        public void Validar()
        {
            ValidarFechaIngreso();
            
        }

        public void ValidarFechaIngreso() //Esto podemos cambiarlo capaz
        {
            if (this.FechaIngreso > DateTime.Now)
            {
                throw new EmpleadoException("La fecha de ingreso no puede ser mayor a la de hoy");
            }
        }
    }
}
