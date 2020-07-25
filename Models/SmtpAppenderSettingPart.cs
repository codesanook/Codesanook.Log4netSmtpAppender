using Orchard.ContentManagement;

namespace Codesanook.Log4netSmtpAppender.Models
{
    public class SmtpAppenderSettingPart : ContentPart
    {
        public string ExceptionEmailToAddress
        {
            get => this.Retrieve(x => x.ExceptionEmailToAddress);
            set => this.Store(x => x.ExceptionEmailToAddress, value);
        }
    }
}