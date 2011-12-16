namespace Upac.CampaignMonitor.ContourExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    using Umbraco.Forms.Core;
    using Umbraco.Forms.Core.Attributes;

    public class TextFieldSetting : FieldSettingType
    {
        #region Fields

        private TextBox tb = new TextBox();

        #endregion Fields

        #region Properties

        public override string Value
        {
            get
            {
                return tb.Text;
            }
            set
            {
                tb.Text = value;
            }
        }

        #endregion Properties

        #region Methods

        public override System.Web.UI.WebControls.WebControl RenderControl(Setting sender, Form form)
        {
            tb.ID = sender.GetName();
            tb.TextMode = TextBoxMode.SingleLine;
            tb.CssClass = "guiInputText guiInputStandardSize";

            if (string.IsNullOrEmpty(tb.Text) && Prevalues.Count > 0)
            {
                tb.Text = Prevalues[0];
            }

            return tb;
        }

        #endregion Methods
    }
}