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
    public class TipoPlano 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese una categoria")]
        public string Categoria { get; set; }
        public string UltimaModificacion { get; set; }

        public TipoPlano() { }

        public TipoPlano(string cat)
        {
            this.Categoria = cat;

        }

    }
}
