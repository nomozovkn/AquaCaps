using System.Runtime.Serialization;

namespace AquaCaps.Core.Errors;

[Serializable]
public class MailSendingException : BaseException
{
    public MailSendingException() { }
    public MailSendingException(String message) : base(message) { }
    public MailSendingException(String message, Exception inner) : base(message, inner) { }
    protected MailSendingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}