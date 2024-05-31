using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        public ProyectoContext Context { get; set; }
        public RepositorioUsuario()
        {
            this.Context = new ProyectoContext();
        }
        public void Agregar(Usuario item)
        {
            item.Validar();
            if (this.UsuarioPorNombreUsuario(item.NombreUsuario) != null)
            {
                throw new UsuarioException("El nombre de usuario ingresado ya está en uso. Elegir otro.");
            }
            Context.Usuarios.Add(item);
            Context.SaveChanges();
        }

        public void InicioSesion(Usuario u)
        {
            foreach (Usuario unU in Context.Usuarios)
            {
                if (unU.NombreUsuario == u.NombreUsuario)
                {
                    if (unU.Contrasenia == u.Contrasenia)
                    {
                        return;
                    }
                }
            }
            throw new Exception("Usuario o contraseña incorrecto.");
        }


        public Usuario UsuarioPorNombreUsuario(string nombreUsuario)
        {
            return Context.Usuarios.Where(u => u.NombreUsuario == nombreUsuario).FirstOrDefault();
        }

        public Usuario Buscar(int id)
        {
            return Context.Usuarios.Where(u => u.Id == id).FirstOrDefault();
        }

        public void Eliminar(Usuario item)
        {
            if (item == null)
            {
                throw new UsuarioException("No se puede eliminar un usuario nulo.");
            }
            if (TieneObrasAbiertas(item))
            {
                throw new UsuarioException("No se puede eliminar al usuario, tiene obras a cargo aún no finalizadas.");
            }
            Context.Usuarios.Remove(item);
            Context.SaveChanges();
        }
        //estaba en private
        public bool TieneObrasAbiertas(Usuario item)
        {
            return Context.Obras.Where(o => o.UsuarioACargo.Id == item.Id && !o.Finalizada).Any();
        }

        public void Modificar(Usuario item)
        {
            try
            {
                //nuevaObra.Validar();
                Usuario usuario = this.Buscar(item.Id);
                if (usuario == null)
                {
                    throw new UsuarioException("No se encontró el usuario a modificar.");
                }

                usuario.Nombre = item.Nombre;
                usuario.NombreUsuario = item.NombreUsuario;
                usuario.Contrasenia = item.Contrasenia;
                Context.Entry(usuario).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UsuarioException(ex.Message);
            }
        }
        
        public IEnumerable<Usuario> TomarTodos()
        {
            return Context.Usuarios.ToList();
        }

        public IEnumerable<Usuario> TomarTodosDeObra()
        {
            return Context.Usuarios.Where(u => u is UDeObra).ToList();
        }

        public IEnumerable<Usuario> TomarTodosDeOficina()
        {
            return Context.Usuarios.Where(u => u is UDeOficina).ToList();
        }

        public IEnumerable<Usuario> TomarTodosDeNormal()
        {
            return Context.Usuarios.Where(u => u is UNormal).ToList();
        }

    }
}
