namespace Upac.Core.Mail
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;

    using NVelocity;
    using NVelocity.App;

    using umbraco.cms.businesslogic.web;
    using umbraco.presentation.nodeFactory;

    using Upac.Core.Configuration;

    public class Message
    {
        #region Fields

        private MailMessage mailMessage;

        #endregion Fields

        #region Constructors

        public Message()
        {
            IncludeHeader = true;
            IncludeFooter = true;
            SendFromNoReplyAddress = false;
            TemplateVariables = new Hashtable();
            mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
        }

        public Message(Document document)
        {
            // TODO
        }

        public Message(Node node)
        {
            // TODO
        }

        #endregion Constructors

        #region Properties

        public MailAddressCollection Bcc
        {
            get
            {
                return mailMessage.Bcc;
            }
        }

        public string Body
        {
            get; set;
        }

        public MailAddressCollection CC
        {
            get
            {
                return mailMessage.CC;
            }
        }

        public string From
        {
            get
            {
                if (mailMessage.From == null)
                {
                    return string.Empty;
                }
                return mailMessage.From.Address;
            }
            set
            {
                mailMessage.From = new MailAddress(value);
            }
        }

        public bool IncludeFooter
        {
            get; set;
        }

        public bool IncludeHeader
        {
            get; set;
        }

        public bool IsBodyHtml
        {
            get
            {
                return mailMessage.IsBodyHtml;
            }
            set
            {
                mailMessage.IsBodyHtml = value;
            }
        }

        public MailMessage MailMessage
        {
            get
            {
                return mailMessage;
            }
        }

        public bool SendFromNoReplyAddress
        {
            get; set;
        }

        public string Subject
        {
            get
            {
                return mailMessage.Subject;
            }
            set
            {
                mailMessage.Subject = value;
            }
        }

        public Hashtable TemplateVariables
        {
            get; set;
        }

        public MailAddressCollection To
        {
            get
            {
                return mailMessage.To;
            }
        }

        #endregion Properties

        #region Methods

        public string GetFullBody()
        {
            if (!IncludeHeader && !IncludeFooter)
            {
                // return quickly
                return Body;
            }

            StringBuilder sb = new StringBuilder();
            if (IncludeHeader)
            {
                if (IsBodyHtml)
                {
                    // TODO
                    //sb.Append(siteSettings.GetSetting("Email/DefaultHeaderHtml"));
                }
                else
                {
                    // TODO
                    //sb.Append(siteSettings.GetSetting("Email/DefaultHeaderText"));
                }
            }
            sb.Append(Body);
            if (IncludeFooter)
            {
                if (IsBodyHtml)
                {
                    // TODO
                    //sb.Append(siteSettings.GetSetting("Email/DefaultFooterHtml"));
                }
                else
                {
                    // TODO
                    //sb.Append(siteSettings.GetSetting("Email/DefaultFooterText"));
                }
            }
            return sb.ToString();
        }

        public bool Send()
        {
            if (!string.IsNullOrEmpty(mailMessage.Body))
            {
                throw new Exception("You should not set body directly on MailMessage. Use Message!");
            }

            if (string.IsNullOrEmpty(From))
            {
                if (SendFromNoReplyAddress)
                {
                    // TODO
                    //From = siteSettings.GetSetting("Email/NoReplyEmail");
                }
                else
                {
                    // TODO
                    //From = siteSettings.GetSetting("Email/DefaultFromEmail");
                }
            }

            Diagnostics.Assert.EnsureStringValue(mailMessage.From.Address, "From address not specified");

            string fullBody = GetFullBody();
            if (TemplateVariables.Count > 0)
            {
                fullBody = Utilities.VelocityUtil.Evaluate(fullBody, TemplateVariables);
            }

            mailMessage.Body = fullBody;
            StandardSender sender = new StandardSender();
            bool sendt = sender.Send(mailMessage);
            mailMessage.Dispose();
            return sendt;
        }

        #endregion Methods
    }
}