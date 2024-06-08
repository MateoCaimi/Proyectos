using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        public Usuario InicioSesion(string nombreUsuario, string contrasenia);
        public Usuario UsuarioPorNombreUsuario(string nombreUsuario);

        public Usuario Buscar(int id);

        public bool TieneObrasAbiertas(Usuario item);

        public IEnumerable<Usuario> TomarTodos();

        public IEnumerable<Usuario> TomarTodosDeObra();

        public IEnumerable<Usuario> TomarTodosDeOficina();

        public IEnumerable<Usuario> TomarTodosDeNormal();

    }
}
