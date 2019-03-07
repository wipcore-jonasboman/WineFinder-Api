using System;
using System.Runtime.Serialization;

namespace WineFinder.Shared.Services
{
    [Serializable]
    public class CredentialsErrorException : Exception
    {
        public CredentialsErrorException()
        {
        }

        public CredentialsErrorException(string message) : base(message)
        {
        }

        public CredentialsErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CredentialsErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}