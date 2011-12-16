namespace Upac.Core.Webcontrols
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;

    using Upac.Core.Utilities;

    [DefaultProperty("Path")]
    [ToolboxData("<{0}:Setting runat=server></{0}:Setting>")]
    public class Setting : Control
    {
        #region Properties

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Path
        {
            get
            {
                String s = (String)ViewState["Path"];
                return (s ?? String.Empty);
            }

            set
            {
                ViewState["Path"] = value;
            }
        }

        #endregion Properties

        #region Methods

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(CommonUtil.GetSetting(Path));
        }

        #endregion Methods
    }
}