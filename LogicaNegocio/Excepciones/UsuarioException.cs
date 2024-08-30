using System.Runtime.Serialization;

namespace LogicaNegocio.Excepciones
{
    [Serializable]
    public class UsuarioException : Exception
    {
        public UsuarioException() { }
        public UsuarioException(string message) : base(message) { }
        public UsuarioException(string message, Exception inner) : base(message, inner) { }
        public UsuarioException(SerializationInfo info, StreamingContext context) { }
    }
}
