namespace Upac.Membership.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Upac.Core.Utilities;

    public partial class Login : System.Web.UI.UserControl
    {
        #region Methods

        protected void OnLoggedIn(object sender, EventArgs e)
        {
            string returnUrl = Request.Form["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect(returnUrl, true);
            }
        }

        protected void OnLoginError(object sender, EventArgs e)
        {
            login.FailureText = CommonUtil.GetSetting("Membership/Login/LoginErrorTextDefault");
            string userName = login.UserName;
            if (!string.IsNullOrEmpty(userName))
            {
                MembershipUser membershipUser = System.Web.Security.Membership.GetUser(userName);
                if (membershipUser != null)
                {
                    if (!membershipUser.IsApproved)
                    {
                        login.FailureText = CommonUtil.GetSetting("Membership/Login/LoginErrorTextNotApproved");
                    }
                    else if (membershipUser.IsLockedOut)
                    {
                        login.FailureText = CommonUtil.GetSetting("Membership/Login/LoginErrorTextLocked");
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Page.ClientScript.RegisterHiddenField("ReturnUrl", returnUrl);
                }
            }
            else
            {
                string returnUrl = Request.Form["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Page.ClientScript.RegisterHiddenField("ReturnUrl", returnUrl);
                }
            }
        }

        #endregion Methods
    }
}