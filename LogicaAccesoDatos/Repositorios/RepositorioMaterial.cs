using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioMaterial : IRepositorioMaterial
    {
        public ProyectoContext Context { get; set; }

        public RepositorioMaterial()
        {
            this.Context = new ProyectoContext();
        }
        public void Agregar(string nom, int stock, string unidadDeMedida)
        {
            Material material = new Material(nom, stock, unidadDeMedida);
            material.Validar();
            if (this.MaterialPorNombre(material.Nombre) != null)
            {
                throw new ObraException("El nombre del material que esta queriendo ingresar ya se encuentra ingresado.");
            }

            Context.Materiales.Add(material);
            Context.SaveChanges();
        }



        public void Eliminar(int id)
        {
            Material m = this.Buscar(id);

            if (m == null)
            {
                throw new ObraException("No se puede eliminar un material nulo.");
            }
            if (MaterialSeEncuentraEnObra(m))
            {
                throw new ObraException("No se puede eliminar el material. Este se esta utilizando en alguna obra.");
            }
            Context.Materiales.Remove(m);
            Context.SaveChanges();
        }



        public void Modificar(Material m)
        {

            Material material = this.Buscar(m.Id);
            if (material == null)
            {
                throw new MaterialException("No se encontró el material para modificar.");
            }

            material.Nombre = m.Nombre;
            material.Stock = m.Stock;
            material.UnidadDeMedida = m.UnidadDeMedida;
            material.Validar();
            Context.Entry(m).State = EntityState.Modified;
            Context.SaveChanges();


        }


        public Material Buscar(int id)
        {
            return Context.Materiales.Find(id);
        }

        public IEnumerable<Material> TomarTodos()
        {
            return Context.Materiales.ToList();
        }


        public Material MaterialPorNombre(string nombre)
        {
            var mat = Context.Materiales.Where(m => m.Nombre == nombre).FirstOrDefault();
            return mat;
        }

        public bool MaterialSeEncuentraEnObra(Material material)
        {
            foreach (Obra o in Context.Obras)
            {
                foreach (Material m in o.Materiales)
                {
                    if (m == material && o.Finalizada == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }





        //si no esta me tira error pero no va
        public void Agregar(Material item)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(Material item)
        {
            throw new NotImplementedException();
        }
    }
}
