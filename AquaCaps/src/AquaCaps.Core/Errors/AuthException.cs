using System.Runtime.Serialization;

namespace AquaCaps.Core.Errors;

[Serializable]

public class AuthException : BaseException
{
    public AuthException() { }
    public AuthException(string message) : base(message) { }
    public AuthException(string message, Exception inner) : base(message, inner) { }
    protected AuthException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}