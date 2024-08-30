namespace LogicaNegocio.Interfaces
{
    public interface IRepositorio<T> where T : class
    {
        public void Agregar(T item);
        public void Eliminar(T item);
        public void Modificar(T item);
        public T Buscar(int id);
        public IEnumerable<T> TomarTodos();
    }
}
