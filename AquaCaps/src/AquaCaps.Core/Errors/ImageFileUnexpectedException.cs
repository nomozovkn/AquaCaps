using System.Runtime.Serialization;

namespace AquaCaps.Core.Errors;

public class ImageFileUnexpectedException : BaseException
{
    public ImageFileUnexpectedException() { }
    public ImageFileUnexpectedException(String message) : base(message) { }
    public ImageFileUnexpectedException(String message, Exception inner) : base(message, inner) { }
    protected ImageFileUnexpectedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
