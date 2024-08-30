using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
    public class TipoPlano
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese una categoria")]
        public string Categoria { get; set; }
        public int IdObra { get; set; }

        public TipoPlano() { }

        public TipoPlano(string cat)
        {
            this.Categoria = cat;

        }

        public TipoPlano(string cat, int idobra)
        {
            this.Categoria = cat;

        }

    }
}
