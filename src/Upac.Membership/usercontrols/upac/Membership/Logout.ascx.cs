namespace Upac.Membership.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Logout : System.Web.UI.UserControl
    {
        #region Properties

        public bool RedirectBackToPage
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            if (RedirectBackToPage && Request.UrlReferrer != null)
            {
                string url = Request.UrlReferrer.ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    Response.Redirect(url, true);
                }
            }
        }

        #endregion Methods
    }
}