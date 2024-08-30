using System.Runtime.Serialization;

namespace LogicaNegocio.Excepciones
{
    [Serializable]
    public class ProveedorException : Exception
    {
        public ProveedorException() { }
        public ProveedorException(string message) : base(message) { }
        public ProveedorException(string message, Exception inner) : base(message, inner) { }
        public ProveedorException(SerializationInfo info, StreamingContext context) { }
    }
}
