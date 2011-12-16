namespace Upac.Core.Webcontrols
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ContentTypeChanger : WebControl
    {
        #region Properties

        public string ContentType
        {
            set;
            get;
        }

        public string Encoding
        {
            set;
            get;
        }

        public string Filename
        {
            set;
            get;
        }

        public string FilenameExtension
        {
            set;
            get;
        }

        public string FilenameWithOutExtension
        {
            set;
            get;
        }

        #endregion Properties

        #region Methods

        protected override void OnPreRender(EventArgs e)
        {
            Page.Response.ClearHeaders();
            Page.Response.Clear();
            Page.Response.Charset = "";

            if (!string.IsNullOrEmpty(ContentType))
            {
                Page.Response.ContentType = ContentType;
            }

            if (!string.IsNullOrEmpty(Encoding))
            {
                Page.ResponseEncoding = Encoding;
            }

            if (!string.IsNullOrEmpty(Filename))
            {
                Page.Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
            }
            else if (!string.IsNullOrEmpty(FilenameWithOutExtension))
            {
                Page.Response.AddHeader("content-disposition", "attachment; filename=" + FilenameWithOutExtension + "." + FilenameExtension);
            }
        }

        // We need to overdire Render, otherwice we will get a span from the base class
        protected override void Render(HtmlTextWriter writer)
        {
        }

        #endregion Methods
    }
}