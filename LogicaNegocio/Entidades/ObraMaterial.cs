using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicaNegocio.Entidades
{
    [PrimaryKey(nameof(IdObra), nameof(IdMaterial))]
    public class ObraMaterial : IValidable
    {
        [ForeignKey("Obra")]
        public int IdObra { get; set; }
        public Obra Obra { get; set; }
        [ForeignKey("Material")]
        public int IdMaterial { get; set; }
        public Material Material { get; set; }
        public int Stock { get; set; }


        public ObraMaterial() { }
        public void Validar()
        {
            if (Stock < 0)
            {
                throw new MaterialException("El stock no puede ser negativo");
            }
        }
    }
}
