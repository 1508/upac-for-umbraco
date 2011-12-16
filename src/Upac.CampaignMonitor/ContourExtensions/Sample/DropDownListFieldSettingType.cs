using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;

namespace Upac.CampaignMonitor.ContourExtensions.Sample
{
    public class DropDownListFieldSettingType : FieldSettingType
    {
        private System.Web.UI.WebControls.CheckBoxList ddl = new System.Web.UI.WebControls.CheckBoxList();
        private string _val = string.Empty;

        public override string Value
        {
            get
            {
                foreach (ListItem item in ddl.Items)
                {
                    if (item.Selected)
                    {
                        return item.Value;
                    }
                }
                return ddl.SelectedValue;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _val = value;
            }
        }

        public override System.Web.UI.WebControls.WebControl RenderControl(Setting sender, Form form)
        {
            ddl.ID = sender.GetName();
            ddl.Items.Clear();
            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("A"));
            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("B"));
            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("C"));
            ddl.SelectedValue = _val;
            return ddl;
        }


    }
}
