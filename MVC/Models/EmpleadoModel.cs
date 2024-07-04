using LogicaNegocio.Entidades;

namespace MVC.Models
{
    public class EmpleadoModel
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string NumFuncionario { get; set; }
        public string Identificador { get; set; }
        public List<MarcaModel> Marcas { get; set; }
    }
}
