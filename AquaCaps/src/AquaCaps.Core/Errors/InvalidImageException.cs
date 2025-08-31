using System.Runtime.Serialization;

namespace AquaCaps.Core.Errors;

[Serializable]
public class InvalidImageException : BaseException
{
    public InvalidImageException() { }
    public InvalidImageException(String message) : base(message) { }
    public InvalidImageException(String message, Exception ex) : base(message, ex) { }
    protected InvalidImageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
