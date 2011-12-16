namespace Upac.Core.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;

    public class StandardSender : IMailSender
    {
        #region Methods

        public bool Send(MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Send(message);
            return true;
        }

        #endregion Methods
    }
}