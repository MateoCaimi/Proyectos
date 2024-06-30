using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioMaterial : IRepositorioMaterial
    {
        public ProyectoContext Context { get; set; }

        public RepositorioMaterial()
        {
            this.Context = new ProyectoContext();
        }
        public void Agregar(Material nuevoMaterial)
        {

            nuevoMaterial.Validar();
            
            foreach (Material mat in Context.Materiales.ToList())
            {
                 if (mat.Nombre == nuevoMaterial.Nombre && mat.UnidadDeMedida == nuevoMaterial.UnidadDeMedida)
                {

                    throw new MaterialException("Ya existe un material con ese nombre y esa unidad de medida.");
                }

            }

            Context.Materiales.Add(nuevoMaterial);
            Context.SaveChanges();
        }



        public void Eliminar(Material material)
        {

            if (material == null)
            {
                throw new MaterialException("No se puede eliminar un material nulo.");
            }
            if (MaterialSeEncuentraEnObra(material))
            {
                throw new MaterialException("No se puede eliminar el material. Este se esta utilizando en alguna obra.");
            }
            Context.Materiales.Remove(material);
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
            material.UnidadDeMedida = m.UnidadDeMedida;
            material.Validar();
            Context.Materiales.Update(material);
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

        public IEnumerable<Material> MaterialesFiltados(string nombre)
        {
            if (nombre == null) throw new MaterialException("Ingrese un nombre para buscar");
            IEnumerable<Material> materiales = Context.Materiales.ToList();
            materiales = materiales.Where(m => m.Nombre.ToUpper().StartsWith(nombre.ToUpper()));
            return materiales;
        }
    }
}
