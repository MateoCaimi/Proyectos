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


        public Usuario InicioSesion(string nombreUsuario, string contrasenia)
        {

            foreach (Usuario unU in Context.Usuarios)
            {
                if (unU.NombreUsuario == nombreUsuario)
                {
                    if (unU.Contrasenia == contrasenia)
                    {

                        if (unU.TiempoDeBloqueo.HasValue && unU.TiempoDeBloqueo.Value > DateTime.UtcNow)
                        {
                            throw new UsuarioException("Cuenta bloqueada. Inténtelo de nuevo más tarde.");
                        }

                        unU.IntentosFallidos = 0;
                        unU.TiempoDeBloqueo = null;
                        unU.UsuarioBloqueado = false;
                        Context.SaveChanges();
                        return unU;
                    }
                    else{

                            unU.IntentosFallidos++;
                            Context.SaveChanges();
                            if (unU.IntentosFallidos >= 3)
                            {
                                unU.TiempoDeBloqueo = DateTime.UtcNow.AddMinutes(1);
                                unU.IntentosFallidos = 0;
                                unU.UsuarioBloqueado = true;
                                Context.SaveChanges();
                                throw new UsuarioException("Cuenta bloqueada por múltiples intentos fallidos. Inténtelo de nuevo en 1 minutos.");

                            }

                           
                            throw new UsuarioException("La contraseña es incorrecta.");
                    }

                }
            }

            throw new UsuarioException("El nombre de usuario ingresado no existe.");
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
                item.Validar();
                Usuario usuario = this.Buscar(item.Id);
                if (usuario == null)
                {
                    throw new UsuarioException("No se encontró el usuario a modificar.");
                }
                Usuario yaExistente = this.UsuarioPorNombreUsuario(item.NombreUsuario);
                if (yaExistente != null && yaExistente.Id != item.Id) 
                {
                    throw new UsuarioException("El nombre de usuario ingresado ya está en uso. Elegir otro.");
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

        public void ModificarOpcionReinstanciar(Usuario item)
        {
            try
            {
                item.Validar();
                Usuario usuario = this.Buscar(item.Id);
                if (usuario == null)
                {
                    throw new UsuarioException("No se encontró el usuario a modificar.");
                }
                Usuario yaExistente = this.UsuarioPorNombreUsuario(item.NombreUsuario);
                if (yaExistente != null && yaExistente.Id != item.Id)
                {
                    throw new UsuarioException("El nombre de usuario ingresado ya está en uso. Elegir otro.");
                }
                //Context.Database.ExecuteSql("SET IDENTITY_INSERT [dbo].[Usuarios] ON");
                Context.Usuarios.Remove(usuario);
                Context.Usuarios.Add(item);
                //Context.Database.ExecuteSql("SET IDENTITY_INSERT [dbo].[Usuarios] OFF");
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

        public Usuario CastearU(string nombre, string nomUsuario, string pass, string tipo)
        {

            Usuario u2;
            switch (tipo)
            {

                case "UDeObra" :
                case "Usuario de obra":
                    u2 = new UDeObra();
                    break;

                case "UDeOficina":
                case "Usuario de oficina":
                     u2 = new UDeOficina();
                    break;

                default :
                   u2 = new UNormal();
                    break;
            }

            u2.NombreUsuario = nomUsuario;
            u2.Nombre = nombre;
            u2.Tipo = tipo;
            u2.Contrasenia = pass;
            u2.CambioContrasenia = true;
            u2.IntentosFallidos = 0;
            u2.UsuarioBloqueado = false;
            u2.TiempoDeBloqueo = null;

            return u2;

        }

        internal void cambiarPass(string nombreUsuario, string contrasenia, string confirmarPass)
        {
            try
            {
                Usuario u = this.UsuarioPorNombreUsuario(nombreUsuario);
                if (contrasenia == confirmarPass)
                {
                    if (u.Contrasenia == contrasenia)
                    {
                        throw new UsuarioException("La contrasenia nueva no puede ser igual a la anterior");
                    }
                    u.Contrasenia = contrasenia;
                    u.Validar();
                    u.CambioContrasenia = false;
                    Context.SaveChanges();
                }
                else
                {
                    throw new UsuarioException("La confirmacion de la contrasenia debe ser la misma");
                }
            }
            catch (Exception e)
            {
                throw new UsuarioException(e.Message);
            }
        }
    }
}
