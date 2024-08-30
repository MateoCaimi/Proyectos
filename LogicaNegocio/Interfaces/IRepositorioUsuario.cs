using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        public Usuario InicioSesion(string usu, string pass);
        public Usuario UsuarioPorNombreUsuario(string nombreUsuario);

        public Usuario Buscar(int id);

        public bool TieneObrasAbiertas(Usuario item);

        public IEnumerable<Usuario> TomarTodos();

        public IEnumerable<Usuario> TomarTodosDeObra();

        public IEnumerable<Usuario> TomarTodosDeOficina();

        public IEnumerable<Usuario> TomarTodosDeNormal();

    }
}
