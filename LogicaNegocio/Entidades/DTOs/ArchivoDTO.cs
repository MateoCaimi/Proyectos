namespace LogicaNegocio.Entidades.DTOs
{
    public class ArchivoDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string WebUrl { get; set; }
        public FolderDTO SiteCollection { get; set; } //Simplemente a modo de ver si es carpeta o no
    }
}
