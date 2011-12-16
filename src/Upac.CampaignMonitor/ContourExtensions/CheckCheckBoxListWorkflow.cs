namespace Upac.CampaignMonitor.ContourExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    using log4net;

    using Umbraco.Forms.Core;
    using Umbraco.Forms.Core.Enums;

    using CheckBoxList = Umbraco.Forms.Core.Providers.FieldTypes.CheckBoxList;

    public class CheckCheckBoxListWorkflow : Umbraco.Forms.Core.WorkflowType
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(SaveListsWorkflow));

        #endregion Fields

        #region Constructors

        public CheckCheckBoxListWorkflow()
        {
            this.Name = "Check all checkboxes on a checkboxlist";
            this.Description = "This workflow will check all checkboxes in a checkboxlist fieldtype.";
            this.Id = new Guid("1387A641-3143-4dbd-8DEB-CB0430164E01");
        }

        #endregion Constructors

        #region Properties

        [Umbraco.Forms.Core.Attributes.Setting("Checkboxlist", description = "Please select the checkboxlist field", control = "Umbraco.Forms.Core.FieldSetting.FieldPicker")]
        public string Checkboxlist
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public override WorkflowExecutionStatus Execute(Record record, RecordEventArgs e)
        {
            /*
            if (!string.IsNullOrEmpty(Checkboxlist))
            {
                Dictionary<Guid, RecordField> dictionary = record.RecordFields;
                // Get the checkboxlist field
                KeyValuePair<Guid, RecordField> checkboxlistField = dictionary.SingleOrDefault(d => d.Key == new Guid(Checkboxlist));
                if (!checkboxlistField.Equals(default(KeyValuePair<Guid, RecordField>)))
                {
                    Field field = checkboxlistField.Value.Field;
                    FieldType type = field.FieldType;
                    Guid guid = field.FieldTypeId;
                    if (guid.Equals(new Guid("FAB43F20-A6BF-11DE-A28F-9B5755D89593")))
                    {
                        CheckBoxList cbl = (CheckBoxList) type;
                        WebControl control = cbl.Editor;
                    }
                }
            }
            */
            return WorkflowExecutionStatus.Completed;
        }

        public override List<Exception> ValidateSettings()
        {
            List<Exception> exceptions = new List<Exception>();
            /*
            if (string.IsNullOrEmpty(Checkboxlist))
            {
                exceptions.Add(new Exception("'Checkboxlist' setting not filled out'"));
            }
            */
            return exceptions;
        }

        #endregion Methods
    }
}