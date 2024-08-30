using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        LogicaNegocio.Entidades.Usuario InicioSesion(System.String nombreUsuario, System.String contrasenia);
        LogicaNegocio.Entidades.Usuario UsuarioPorNombreUsuario(System.String nombreUsuario);
        System.Boolean TieneObrasAbiertas(LogicaNegocio.Entidades.Usuario item);
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Usuario> TomarTodosDeObra();
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Usuario> TomarTodosDeOficina();
        System.Collections.Generic.IEnumerable<LogicaNegocio.Entidades.Usuario> TomarTodosDeNormal();
        LogicaNegocio.Entidades.Usuario CastearU(System.String nombre, System.String nomUsuario, System.String pass, System.String tipo);

    }
}
