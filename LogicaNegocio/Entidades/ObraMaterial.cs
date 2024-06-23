using LogicaNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using LogicaNegocio.Excepciones;

namespace LogicaNegocio.Entidades
{
    [PrimaryKey(nameof(IdObra), nameof(IdMaterial))]
    public class ObraMaterial : IValidable
    {
        [ForeignKey("Obra")]
        public int IdObra { get; set; }
        public Obra Obra {  get; set; }
        [ForeignKey("Material")]
        public int IdMaterial { get; set; }
        public Material Material {  get; set; }
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
