namespace Upac.CampaignMonitor.ContourExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using log4net;

    using Umbraco.Forms.Core;
    using Umbraco.Forms.Core.Enums;

    public class SaveListsWorkflow : Umbraco.Forms.Core.WorkflowType
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(SaveListsWorkflow));

        #endregion Fields

        #region Constructors

        public SaveListsWorkflow()
        {
            this.Name = "Subscribe to Campaign Monitor";
            this.Description = "This will add a subscription to Campaign Monitor.";
            this.Id = new Guid("D78566DA-91F9-4bb8-A693-70B05C6D1D89");
        }

        #endregion Constructors

        #region Properties

        [Umbraco.Forms.Core.Attributes.Setting("Email", description = "Please select the field containing the subscriber email", control = "Umbraco.Forms.Core.FieldSetting.FieldPicker")]
        public string SubscriberEmail
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("List", description = "Please select the field containing the campaign monitor lists", control = "Umbraco.Forms.Core.FieldSetting.FieldPicker")]
        public string SubscriberLists
        {
            get; set;
        }

        [Umbraco.Forms.Core.Attributes.Setting("Name", description = "Please select the field containing the subscriber name", control = "Umbraco.Forms.Core.FieldSetting.FieldPicker")]
        public string SubscriberName
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public override WorkflowExecutionStatus Execute(Record record, RecordEventArgs e)
        {
            Dictionary<Guid, RecordField> dictionary = record.RecordFields;

            // Get the email
            KeyValuePair<Guid, RecordField> subscriberEmailField = dictionary.SingleOrDefault(d => d.Key == new Guid(SubscriberEmail));
            if (!subscriberEmailField.Equals(default(KeyValuePair<Guid, RecordField>)))
            {
                string email = subscriberEmailField.Value.ValuesAsString();
                if (!string.IsNullOrEmpty(email))
                {
                    // We got an email, lets continue
                    // Get the lists
                    KeyValuePair<Guid, RecordField> subscriberListsField = dictionary.SingleOrDefault(d => d.Key == new Guid(SubscriberLists));
                    if (!subscriberListsField.Equals(default(KeyValuePair<Guid, RecordField>)))
                    {
                        List<object> selectedLists = subscriberListsField.Value.Values;
                        if (selectedLists.Count > 0)
                        {
                            // The user has selected one or more lists
                            // Lets continue
                            // Get the name - it's optional
                            string name = string.Empty;
                            KeyValuePair<Guid, RecordField> subscriberNameField = dictionary.SingleOrDefault(d => d.Key == new Guid(SubscriberName));
                            if (!subscriberNameField.Equals(default(KeyValuePair<Guid, RecordField>)))
                            {
                                name = subscriberNameField.Value.ValuesAsString();
                            }

                            Api api = new Api();
                            foreach (object o in selectedLists)
                            {
                                string listId = (string) o;
                                if (!string.IsNullOrEmpty(listId))
                                {
                                    api.AddSubscriber(email, name, listId);
                                }
                            }

                        }
                    }
                }
            }
            return WorkflowExecutionStatus.Completed;
        }

        public override List<Exception> ValidateSettings()
        {
            List<Exception> exceptions = new List<Exception>();
            if (string.IsNullOrEmpty(SubscriberEmail))
            {
                exceptions.Add(new Exception("'Email' setting not filled out'"));
            }

            if (string.IsNullOrEmpty(SubscriberLists))
            {
                exceptions.Add(new Exception("'List' setting not filled out'"));
            }

            if (string.IsNullOrEmpty(SubscriberName))
            {
                exceptions.Add(new Exception("'Name' setting not filled out'"));
            }
            return exceptions;
        }

        #endregion Methods
    }
}