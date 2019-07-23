using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MMS.Api.Common.Exceptions
{
    /// <summary>
    /// Base exception type for those are thrown by  system for this specific exceptions.
    /// </summary>
    [Serializable]
    public class ExceptionBase : Exception
    {
        /// <summary>
        /// Creates a new <see cref="ExceptionBase"/> object.
        /// </summary>
        public ExceptionBase()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ExceptionBase"/> object.
        /// </summary>
        public ExceptionBase(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="ExceptionBase"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public ExceptionBase(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="ExceptionBase"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
