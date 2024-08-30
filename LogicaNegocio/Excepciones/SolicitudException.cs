using System.Runtime.Serialization;

namespace LogicaNegocio.Excepciones
{
    [Serializable]
    public class SolicitudException : Exception
    {
        public SolicitudException() { }
        public SolicitudException(string message) : base(message) { }
        public SolicitudException(SerializationInfo info, StreamingContext context) { }
        public SolicitudException(string message, Exception inner) : base(message, inner) { }
    }

}
