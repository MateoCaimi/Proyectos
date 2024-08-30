namespace LogicaNegocio.Entidades
{
    public class UDeOficina : Usuario
    {
        public UDeOficina(string nombre, string nomUsuario, string pass) : base(nombre, nomUsuario, pass)
        {

        }

        public UDeOficina() { }

        public override string Tipo { get => "Usuario de oficina"; }
    }
}
