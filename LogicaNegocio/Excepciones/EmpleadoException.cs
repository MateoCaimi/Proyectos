using System.Runtime.Serialization;

namespace LogicaNegocio.Excepciones
{
    [Serializable]
    public class EmpleadoException : Exception
    {
        public EmpleadoException() { }
        public EmpleadoException(string message) : base(message) { }
        public EmpleadoException(string message, Exception inner) : base(message, inner) { }
        public EmpleadoException(SerializationInfo info, StreamingContext context) { }
    }
}
