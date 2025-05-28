using System.Runtime.Serialization;

namespace Service
{
    [Serializable]
    internal class RefreshTokenBadRequest : Exception
    {
        public RefreshTokenBadRequest()
        {
        }

        public RefreshTokenBadRequest(string? message) : base(message)
        {
        }

        public RefreshTokenBadRequest(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RefreshTokenBadRequest(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}