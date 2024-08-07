using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogicaNegocio.Entidades
{
    public class Carpeta
    {
        public Carpeta Anterior { get; set; }
        [ForeignKey("IdObra")]
        public Obra? Obra { get; set; }
        public int IdObra { get; set; }
        [Key]
        public string Name { get; set; }
    }
}
