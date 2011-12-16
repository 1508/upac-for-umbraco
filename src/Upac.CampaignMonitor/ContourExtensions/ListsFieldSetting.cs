namespace Upac.CampaignMonitor.ContourExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    using CampaignMonitorAPIWrapper.CampaignMonitorAPI;

    using Umbraco.Forms.Core;
    using Umbraco.Forms.Core.Attributes;

    public class ListsFieldSetting : FieldSettingType
    {
        #region Fields

        private readonly CheckBoxList cbl;

        private string selectedValue;

        #endregion Fields

        #region Constructors

        public ListsFieldSetting()
        {
            cbl = new CheckBoxList();
            selectedValue = string.Empty;
        }

        #endregion Constructors

        #region Properties

        public override string Value
        {
            get
            {
                List<string> selectedValues = new List<string>();
                foreach (ListItem listItem in cbl.Items)
                {
                    if (listItem.Selected)
                    {
                        selectedValues.Add(listItem.Value);
                    }
                }
                return string.Join(",", selectedValues.ToArray());
            }
            set
            {
                selectedValue = value;
                foreach (ListItem item in cbl.Items)
                {
                    if (selectedValue.Contains(item.Value))
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        #endregion Properties

        #region Methods

        public override WebControl RenderControl(Setting sender, Form form)
        {
            Api api = new Api();
            cbl.ID = sender.GetName();

            List<List> lists = api.GetLists();
            foreach (List mailinglist in lists)
            {
                ListItem listItem = new ListItem(mailinglist.Name, mailinglist.ListID)
                {
                    Selected = selectedValue.Contains(mailinglist.ListID)
                };
                cbl.Items.Add(listItem);
            }
            return cbl;
        }

        #endregion Methods
    }
}