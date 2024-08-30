namespace LogicaNegocio.Entidades.DTOs
{
    public class DriveDTO
    {
        public List<ArchivoDTO> value { get; set; }

        public DriveDTO()
        {
            value = new List<ArchivoDTO>();
        }
    }
}
