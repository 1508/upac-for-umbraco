namespace Upac.CampaignMonitor.ContourExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    using CampaignMonitorAPIWrapper.CampaignMonitorAPI;

    using Umbraco.Forms.Core;
    using Umbraco.Forms.Core.Controls;

    public class ListsFieldType : FieldType
    {
        #region Fields

        private ValidateableCheckBoxList cbl;
        private string defaultValueTest;
        private string defaultValueTest2;
        private string defaultValueTest3;
        private string listsToInclude;
        private List<object> value;

        #endregion Fields

        #region Constructors

        public ListsFieldType()
        {
            base.Id = new Guid("{46172748-DF31-4c14-B4F3-300E355606A8}");
            base.Name = "Campaign Monitor Lists";
            base.Description = "Renders a checkbox list with mailinglists";
            this.Icon = "checkboxlist.png";
            this.DataType = FieldDataType.String;
            this.value = new List<object>();
            listsToInclude = string.Empty;
        }

        #endregion Constructors

        #region Properties

        public override WebControl Editor
        {
            get
            {
                cbl = new ValidateableCheckBoxList();
                cbl.CssClass = "checkboxlist";
                cbl.RepeatLayout = RepeatLayout.Flow;

                ListItem[] items = GetListItems();
                cbl.Items.AddRange(items);
                return cbl;
            }
            set
            {
                base.Editor = value;
            }
        }

        public override List<object> Values
        {
            get
            {
                if (cbl != null && cbl.SelectedIndex > -1)
                {
                    value.Clear();
                    foreach (ListItem item in cbl.Items)
                    {
                        if (item.Selected)
                        {
                            value.Add(item.Value);
                        }
                    }
                }
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        #endregion Properties

        #region Methods

        public override string RenderPreview()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"checkboxlist\">");

            ListItem[] items = GetListItems();
            foreach (ListItem item in items)
            {
                sb.Append(string.Format("<p><input type='checkbox' /> <label>{0}</label></p>", item.Text));
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        public override string RenderPreviewWithPrevalues(List<object> prevalues)
        {
            return RenderPreview();
        }

        private ListItem[] GetListItems()
        {
            List<ListItem> items = new List<ListItem>();

            return items.ToArray();
        }

        #endregion Methods
    }
}