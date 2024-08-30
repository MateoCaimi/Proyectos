using LogicaNegocio.Excepciones;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
    [PrimaryKey(nameof(IdObra), nameof(IdEmpleado), nameof(Entrada), nameof(Salida))]
    public class Marca
    {
        public int IdObra { get; set; }
        public int IdEmpleado { get; set; }
        [ForeignKey("IdObra, IdEmpleado")]
        public ObraEmpleado Empleado { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public int HorasLluvia { get; set; }
        public int HorasExtra { get; set; }

        public int HorasTrabajadas()
        {
            TimeSpan diff = Salida - Entrada;
            return diff.Hours - 1; //se resta hora de descanso
        }

        public void Validar()
        {
            TimeSpan diff = Entrada - Salida;
            if (diff.Hours > 0)
            {
                throw new EmpleadoException("La hora de salida fue anterior a la hora de entrada.");
            }
        }
    }
}
