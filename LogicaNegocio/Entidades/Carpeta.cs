using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
    [PrimaryKey(nameof(Name), nameof(IdObra))]
    public class Carpeta
    {
        public string? NameAnterior { get; set; }
        [ForeignKey("NameAnterior, IdObra")]
        public Carpeta? Anterior { get; set; }
        [ForeignKey("IdObra")]
        public Obra? Obra { get; set; }
        public int IdObra { get; set; }
        public string Name { get; set; }
        [ForeignKey("Tipo")]
        public int? IdTipo { get; set; }
        public TipoPlano? Tipo { get; set; }

        public string NombreFormateado()
        {
            return Uri.UnescapeDataString(Name);
        }
    }
}
