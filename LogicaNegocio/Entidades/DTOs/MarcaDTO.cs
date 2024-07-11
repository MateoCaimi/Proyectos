using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Entidades.DTOs
{
    public class MarcaDTO
    {
        public DateTime HoraMarcaje {  get; set; }
        public string SN {  get; set; }
        public string NombreLector {  get; set; }
        public string? Comentario { get; set; }
    }
}
