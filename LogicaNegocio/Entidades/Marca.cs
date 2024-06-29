using LogicaNegocio.Excepciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades
{
    [PrimaryKey(nameof(IdObra), nameof(IdEmpleado))]
    public class Marca
    {
        public int IdObra { get; set; }
        public int IdEmpleado { get; set; }
        [ForeignKey("IdObra, IdEmpleado")]
        public ObraEmpleado Empleado {  get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public int HorasLluvia { get; set; }

        public int HorasTrabajadas()
        {
            TimeSpan diff = Salida - Entrada; 
            return diff.Hours - 1; //se resta hora de descanso
        }

        public void Validar()
        {
            TimeSpan diff = Entrada - Salida;
            if(diff.Hours > 0)
            {
                throw new EmpleadoException("La hora de salida fue anterior a la hora de entrada.");
            }
        }
    }
}
