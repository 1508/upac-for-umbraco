namespace Upac.Core.Mail
{
    using System.Net.Mail;

    public interface IMailSender
    {
        #region Methods

        bool Send(MailMessage message);

        #endregion Methods
    }
}