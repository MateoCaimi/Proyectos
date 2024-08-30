namespace LogicaNegocio.Entidades
{
    public class UDeObra : Usuario
    {
        public UDeObra(string nombre, string nomUsuario, string pass) : base(nombre, nomUsuario, pass)
        {
        }

        public UDeObra() { }

        public override string Tipo { get => "Usuario de obra"; }
    }
}
