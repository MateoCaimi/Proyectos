using System.Runtime.Serialization;

namespace LogicaNegocio.Excepciones
{
    [Serializable]
    public class MaterialException : Exception
    {
        public MaterialException() { }
        public MaterialException(string message) : base(message) { }
        public MaterialException(string message, Exception inner) : base(message, inner) { }
        public MaterialException(SerializationInfo info, StreamingContext context) { }
    }
}
