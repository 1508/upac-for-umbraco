namespace Upac.Core.Mail.Provider
{
    using System;
    using System.Net.Mail;

    public class StandardMailSenderProvider : MailSenderProviderBase
    {
        #region Methods

        public override bool Send(MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Send(message);
            return true;
        }

        #endregion Methods
    }
}