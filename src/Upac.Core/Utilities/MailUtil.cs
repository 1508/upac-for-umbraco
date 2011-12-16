namespace Upac.Core.Utilities
{
    using Upac.Core.Mail;

    public static class MailUtil
    {
        #region Methods

        public static bool SendMail(string to, string from, string subject, string body, bool isBodyHtml)
        {
            Message message = new Message()
                                  {
                                      Subject = subject,
                                      Body = body,
                                      IsBodyHtml = isBodyHtml,
                                      From = from,
                                      IncludeFooter = false,
                                      IncludeHeader = false
                                  };
            message.To.Add(to);
            return message.Send();
        }

        #endregion Methods
    }
}