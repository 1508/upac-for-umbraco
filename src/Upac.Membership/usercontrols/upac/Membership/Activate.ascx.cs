namespace Upac.Membership.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using umbraco.cms.businesslogic.property;

    using Upac.Core.Utilities;

    public partial class Activate : System.Web.UI.UserControl
    {
        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["u"];
            string code = Request.QueryString["c"];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(username) )
            {
                ltrMessage.Text = CommonUtil.GetSetting("Membership/Activate/UrlError");
            }
            else
            {
                MembershipUser user = Upac.Membership.Helper.GetMembershipUser(username, code);
                if (user == null)
                {
                    ltrMessage.Text = CommonUtil.GetSetting("Membership/Activate/UserNotFound");
                }
                else
                {
                    user.IsApproved = true;
                    System.Web.Security.Membership.UpdateUser(user);
                    ltrMessage.Text = CommonUtil.GetSetting("Membership/Activate/UserIsApproved");
                }
            }
        }

        #endregion Methods
    }
}