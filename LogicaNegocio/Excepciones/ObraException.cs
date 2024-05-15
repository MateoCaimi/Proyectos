using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.Excepciones
{
    [Serializable]
    public class ObraException : Exception
    {
        public ObraException() { }
        public ObraException(string message) : base(message) { }
        public ObraException(SerializationInfo info, StreamingContext context) { }
        public ObraException(string message, Exception inner) : base(message, inner) { }
    }
}
