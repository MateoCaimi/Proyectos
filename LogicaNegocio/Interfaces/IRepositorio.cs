using LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
