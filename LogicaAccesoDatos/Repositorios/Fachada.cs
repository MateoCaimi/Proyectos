using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class Fachada
    {
        RepositorioObra RepositorioObra { get; set; }
        RepositorioPlano RepositorioPlano { get; set; }
        RepositorioUsuario RepositorioUsuario { get; set; }
        

        public Fachada()
        {
            RepositorioObra = new RepositorioObra();
            RepositorioPlano = new RepositorioPlano();
            RepositorioUsuario = new RepositorioUsuario();
        }
        public Obra ObraPorDireccion(string direccion)
        {
            return RepositorioObra.ObraPorDireccion(direccion);
        }
        public Obra ObraPorNombre(string nombre)
        {
            return RepositorioObra.ObraPorNombre(nombre);
        }
        public IEnumerable<Obra> ObrasFiltradas(string? nombre, string? direccion, bool? finalizada)
        {
            return RepositorioObra.ObrasFiltradas(nombre, direccion, finalizada);
        }
        public IEnumerable<Obra> BuscarPorNombre(string nombre)
        {
            return RepositorioObra.BuscarPorNombre(nombre);
        }
        public IEnumerable<Obra> BuscarPorDireccion(string direccion)
        {
            return RepositorioObra.BuscarPorDireccion(direccion);
        }
        public void FinalizarObra(Obra obra)
        {
            RepositorioObra.FinalizarObra(obra);
        }
        public Material MaterialMenosSolicitado(int IdObra)
        {
            return RepositorioObra.MaterialMenosSolicitado(IdObra);
        }
        public Material MaterialMasSolicitado(int IdObra)
        {
            return RepositorioObra.MaterialMasSolicitado(IdObra);
        }
        public Proveedor ProveedorMasComun(int IdObra)
        {
            return RepositorioObra.ProveedorMasComun(IdObra);
        }
        public Usuario SolicitanteMasComun(int IdObra)
        {
            return RepositorioObra.SolicitanteMasComun(IdObra);
        }
        public UDeOficina AprobadorMasComun(int IdObra)
        {
            return RepositorioObra.AprobadorMasComun(IdObra);
        }
        public void AgregarObra(Obra item)
        {
           RepositorioObra.Agregar(item);
        }
        public void EliminarObra(Obra item)
        {
            RepositorioObra.Eliminar(item);
        }
        public void ModificarObra(Obra item)
        {
            RepositorioObra.Modificar(item);
        }
        public Obra BuscarObra(int id)
        {
            return RepositorioObra.Buscar(id);
        }
        public IEnumerable<Obra> TomarTodasObras()
        {
            return RepositorioObra.TomarTodos();
        }
        public IEnumerable<Plano> PlanosTotales(Obra obra)
        {
            return RepositorioPlano.PlanosTotales(obra);
        }
        public IEnumerable<Plano> PlanosFiltrados(Obra obra, int? tipo, string? nombre, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return RepositorioPlano.PlanosFiltrados(obra, tipo, nombre, fechaDesde, fechaHasta);
        }
        public IEnumerable<Plano> PlanosPorAntiguedad(Obra obra)
        {
            return RepositorioPlano.PlanosPorAntiguedad(obra);
        }
        public IEnumerable<TipoPlano> BuscarTiposPlanos()
        {
            return RepositorioPlano.BuscarTiposPlanos();
        }
        public void AgregarPlano(Plano item)
        {
            RepositorioPlano.Agregar(item);
        }
        public void EliminarPlano(Plano item)
        {
            RepositorioPlano.Eliminar(item);
        }
        public void ModificarPlano(Plano item)
        {
            RepositorioPlano.Modificar(item);
        }
        public Plano BuscarPlano(int id)
        {
            return RepositorioPlano.Buscar(id);
        }
        public IEnumerable<Plano> TomarTodosPlanos()
        {
            return RepositorioPlano.TomarTodos();
        }
        public void InicioSesion(Usuario u)
        {
            RepositorioUsuario.InicioSesion(u);
        }
        public IEnumerable<Usuario> ObtenerUsuariosDeObra()
        {
            return RepositorioUsuario.TomarTodosDeObra();
        }

        public IEnumerable<Usuario> ObtenerUsuarios()
        {
            return RepositorioUsuario.TomarTodos();
        }

        public void AgregarUsuario(Usuario usuario)
        {
            RepositorioUsuario.Agregar(usuario);
        }

        public UDeObra BuscarUsuarioObra(int idACargo)
        {
            return (UDeObra)RepositorioUsuario.Buscar(idACargo);
        }
        public void EliminarUsuario (Usuario usuario)
        {
            RepositorioUsuario.Eliminar(usuario);
        }

        public Usuario CastearUsuario(string nombre, string nomUsuario, string pass, string tipo)
        {
            return RepositorioUsuario.CastearU( nombre,  nomUsuario,  pass,  tipo);
        }

        public Usuario BuscarUsuario(int id)
        {
            return RepositorioUsuario.Buscar(id);
        }

        public void ModificarUsuario(Usuario u)
        {
            RepositorioUsuario.Modificar(u);
        }

        public IEnumerable<Plano> BuscarCarpeta(string carpeta)
        {
           return RepositorioPlano.BuscarCarpeta(carpeta);
        }

        public IEnumerable<string> CrearCarpetas()
        {
            return RepositorioPlano.CrearCarpetas();
        }

        public IEnumerable<string> CrearCarpetasPlanillas()
        {
            return RepositorioPlano.CrearCarpetasPlanillas();
        }

        public Dictionary<string, int> CarpetasConCantidad()
        {
            return RepositorioPlano.CarpetasConCantidad();
        }




    }
}
