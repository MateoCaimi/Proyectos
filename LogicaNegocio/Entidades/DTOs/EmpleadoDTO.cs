namespace LogicaNegocio.Entidades.DTOs
{
    public class EmpleadoDTO
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public int? Num_Funcionario { get; set; }
        public string Identificador { get; set; }
        public List<MarcaDTO> Marcas { get; set; }


    }
}
