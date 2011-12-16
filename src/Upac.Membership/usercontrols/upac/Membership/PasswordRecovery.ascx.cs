namespace Upac.Membership.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using umbraco.cms.businesslogic.member;
    using umbraco.cms.businesslogic.property;

    using Upac.Core.Mail;
    using Upac.Core.Utilities;

    public partial class PasswordRecovery : System.Web.UI.UserControl
    {
        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string username = Request.QueryString["u"];
                string code = Request.QueryString["c"];
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(code))
                {
                    SetupProvideNewPassword();
                }
            }
        }

        protected void RecoverLoginInfo(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbEmail.Text))
            {
                    MembershipUser user = Helper.GetMembershipUserViaEmail(tbEmail.Text);
                    if (user == null)
                    {
                        ltrReceipt.Text = CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/ReceiptEmailNotFound");
                    }
                    else if (user.IsLockedOut)
                    {
                        ltrReceipt.Text = CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/ReceiptProfileIsLocked");
                    }
                    else if (!user.IsApproved)
                    {
                        ltrReceipt.Text = CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/ReceiptNotApproved");
                    }
                    else
                    {
                        umbraco.cms.businesslogic.member.Member member = Helper.GetMember(user);
                        if (member == null)
                        {
                            ltrReceipt.Text = CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/ReceiptEmailNotFound");
                        }
                        else
                        {
                            Property property = member.getProperty("SecretCode");
                            if (property == null || string.IsNullOrEmpty(property.Value.ToString()))
                            {
                                throw new Exception("Could not find SecretCode on email: " + tbEmail.Text);
                            }
                            else
                            {
                                string link = string.Concat(GetBaseUrl(), HttpContext.Current.Request.Url.AbsolutePath, "?c=", HttpUtility.UrlEncode(property.Value.ToString()), "&u=", HttpUtility.UrlEncode(user.UserName));
                                Message message = new Message();
                                message.Body = CommonUtil.GetSetting("Membership/PasswordRecovery/ReceiptEmailSendt/EmailBody");
                                message.Subject = CommonUtil.GetSetting("Membership/PasswordRecovery/ReceiptEmailSendt/EmailSubject");
                                message.To.Add(user.Email);
                                message.TemplateVariables.Add("link", link);
                                message.Send();
                                ltrReceipt.Text = CommonUtil.GetSetting("Membership/PasswordRecovery/ReceiptEmailSendt/Screen");
                            }
                        }
                    }
            }
            multiView.SetActiveView(viewReceipt);
        }

        protected void ResetPassword(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Password.Text == ConfirmPassword.Text)
                {
                    string username = hidddenUsername.Value;
                    string code = hidddenCode.Value;
                    MembershipUser user = Upac.Membership.Helper.GetMembershipUser(username, code);
                    Upac.Core.Diagnostics.Assert.EnsureNotNullReference(user, "MembershipUser");
                    Member member = Helper.GetMember(user);
                    Upac.Core.Diagnostics.Assert.EnsureNotNullReference(member, "Member");
                    member.Password = Password.Text;
                    Helper.UpdateSecretCode(member);
                    multiView.SetActiveView(viewPasswordIsUpdated);
                }
            }
        }

        private string GetBaseUrl()
        {
            string domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string protocol = "http://";
            if (HttpContext.Current.Request.ServerVariables["HTTPS"] != null && HttpContext.Current.Request.ServerVariables["HTTPS"] == "ON")
            {
                protocol = "https://";
            }
            return string.Concat(protocol, domain);
        }

        private void SetupProvideNewPassword()
        {
            string username = Request.QueryString["u"];
            string code = Request.QueryString["c"];

            MembershipUser user = Upac.Membership.Helper.GetMembershipUser(username, code);
            if (user == null)
            {
                ltrReceipt.Text = CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideNewPassword/UrlError");
                multiView.SetActiveView(viewReceipt);
            }
            else
            {
                umbraco.cms.businesslogic.member.Member member = Upac.Membership.Helper.GetMember(user);

                multiView.SetActiveView(viewProvideNewPassword);

                hidddenCode.Value = code;
                hidddenUsername.Value = username;
            }
        }

        #endregion Methods
    }
}