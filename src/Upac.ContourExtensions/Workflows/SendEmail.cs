namespace Upac.ContourExtensions.Workflows
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

    using umbraco.BusinessLogic;

    using Umbraco.Forms.Core;
    using Umbraco.Forms.Core.Enums;

    public class SendEmail : WorkflowType
    {
        #region Constructors

        public SendEmail()
        {
            Id = new Guid("{441380E7-A309-4d99-A23D-AEF85E7A16DD}");
            Name = "Upac.ContourExtensions: Send email";
            Description = "Send email til en valgt email eller/og et email felt fra formularen.";
        }

        #endregion Constructors

        #region Properties

        [Umbraco.Forms.Core.Attributes.Setting("Felter som skal bruges til besked", description = "Alias skal udskrives ud $ - når man referere til et alias i Besked feltet skal man bruge $ eks.: Kære $name tak for...", control = "Umbraco.Forms.Core.FieldSetting.FieldMapper")]
        public string FieldMapper
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Besked", description = "Skriv din besked her. Eventuelt med aliaser tilvalgt under 'Felter som skal bruges til besked'", control = "Umbraco.Forms.Core.FieldSetting.TextArea")]
        public string Message
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Modtager emails", description = "Modtager email(s) semikolonsepareret", control = "Umbraco.Forms.Core.FieldSetting.TextField")]
        public string ReceiverEmails
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Email til afsender", description = "Hvis udfylder af formularen skal modtage en email skal du vælge email feltet fra formularen (Dette felt er kun relevant hvis der er sat hak i 'Send email til afsender')", control = "Umbraco.Forms.Core.FieldSetting.FieldPicker")]
        public string RecipientEmail
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Afsender", description = "Afsender/reply-to email", control = "Umbraco.Forms.Core.FieldSetting.TextField")]
        public string ReplyTo
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Send email til afsender", description = "Send email til den som har udfyldt formularen", control = "Umbraco.Forms.Core.FieldSetting.Checkbox")]
        public string SendToRecipientEmail
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Emne", description = "Emne til email", control = "Umbraco.Forms.Core.FieldSetting.TextField")]
        public string Subject
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public override WorkflowExecutionStatus Execute(Record record, RecordEventArgs e)
        {
            Dictionary<Guid, RecordField> dictionary = record.RecordFields;

            // Get the to emails
            List<string> toEmails = new List<string>();
            if (!string.IsNullOrEmpty(RecipientEmail))
            {
                KeyValuePair<Guid, RecordField> subscriberEmailField = dictionary.SingleOrDefault(d => d.Key == new Guid(RecipientEmail));
                if (!subscriberEmailField.Equals(default(KeyValuePair<Guid, RecordField>)))
                {
                    string email = subscriberEmailField.Value.ValuesAsString().Trim().ToLower();
                    // Some simple validation. The validation should be in the form
                    if (SimpleEmailValidate(email))
                    {
                        toEmails.Add(email);
                    }
                }
            }

            if (!string.IsNullOrEmpty(ReceiverEmails))
            {
                string[] receiverEmails = ReceiverEmails.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string  email in receiverEmails)
                {
                    if (SimpleEmailValidate(email))
                    {
                        toEmails.Add(email.Trim().ToLower());
                    }
                }

            }

            // Shoud we send any emails?
            if (toEmails.Count > 0)
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress(ReplyTo); ;
                message.Subject = Subject;

                message.Body = ParseBody(Message, dictionary, record);

                foreach (string email in toEmails)
                {
                    message.To.Clear();
                    message.To.Add(email);
                    SmtpClient client = new SmtpClient();
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception exception)
                    {
                        Log.Add(LogTypes.Error, -1, string.Format("Upac.ContourExtensions.Workflows.SendEmail could not send email to {0}", email));
                        Log.Add(LogTypes.Error, -1, string.Format("Upac.ContourExtensions.Workflows.SendEmail Exception\n\n {0}", exception.InnerException));
                    }
                }
            }

            return WorkflowExecutionStatus.Completed;
        }

        public override List<Exception> ValidateSettings()
        {
            var exceptions = new List<Exception>();
            if (string.IsNullOrEmpty(ReplyTo))
                exceptions.Add(new Exception("'Afsender' setting not filled out'"));

            if (string.IsNullOrEmpty(ReceiverEmails) && string.IsNullOrEmpty(RecipientEmail))
                exceptions.Add(new Exception("'Modtager emails' or 'Recipient Email' setting should be filled out'"));

            if (string.IsNullOrEmpty(Message))
                exceptions.Add(new Exception("'Besked' setting not filled out'"));

            return exceptions;
        }

        private static string EvaluateVelocity(string template, Hashtable variables)
        {
            string returnValue;
            if (variables.Count == 0)
            {
                returnValue = template;
            }
            else
            {
                VelocityContext context = new VelocityContext(variables);
                Velocity.Init();
                using (StringWriter velocityOutput = new StringWriter())
                {
                    Velocity.Evaluate(context, velocityOutput, "", template);
                    returnValue = velocityOutput.GetStringBuilder().ToString();
                }
            }
            return returnValue;
        }

        private static bool SimpleEmailValidate(string email)
        {
            return (!string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains("."));
        }

        private string ParseBody(string message, Dictionary<Guid, RecordField> dictionary, Record record)
        {
            if (!string.IsNullOrEmpty(FieldMapper))
            {
                Hashtable ht = new Hashtable();

                string[] _fields = FieldMapper.Split(';');
                if (_fields.Length > 0)
                {
                    foreach (string field in _fields)
                    {
                        string[] vals = field.Split(',');
                        if (vals.Length == 3)
                        {

                            string formFieldId = string.Empty;
                            string alias = vals[0];
                            string value = string.Empty;

                            if (!string.IsNullOrEmpty(vals[1]))
                            {
                                formFieldId = vals[1];
                            }

                            if (!string.IsNullOrEmpty(formFieldId))
                            {
                                KeyValuePair<Guid, RecordField> formField = dictionary.SingleOrDefault(d => d.Key == new Guid(formFieldId));
                                if (!formField.Equals(default(KeyValuePair<Guid, RecordField>)))
                                {
                                    value = formField.Value.ValuesAsString();
                                }
                            }

                            if (!ht.ContainsKey(alias))
                            {
                                ht.Add(alias, value);
                            }
                        }
                    }

                    return EvaluateVelocity(message, ht);
                }

                /*
                foreach (KeyValuePair<Guid, RecordField> keyValuePair in dictionary)
                {
                    Guid guid = keyValuePair.Key;
                    RecordField field = keyValuePair.Value;
                    Field field1 = field.Field;
                    Guid keyId = field.Key;
                    Guid recordId = field.Record;
                    List<object> objects = field.Values;
                    string asString = field.ValuesAsString();
                }
                */
            }
            return message;
        }

        #endregion Methods
    }
}