using LogicaNegocio.Entidades;

namespace LogicaNegocio.ViewModel
{
    public class ObraEstadisticasViewModel
    {
        public Obra Obra { get; set; }
        public Material MaterialMasSolicitado { get; set; }
        public Material MaterialMenosSolicitado { get; set; }
        //public Proveedor ProveedorMasComun {  get; set; }
        public UDeOficina AprobadorMasComun { get; set; }
        public Usuario SolicitanteMasComun { get; set; }
    }
}
