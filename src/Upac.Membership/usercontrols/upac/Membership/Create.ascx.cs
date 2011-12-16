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

    using Upac.Core;
    using Upac.Core.Mail;
    using Upac.Core.Utilities;

    public partial class Create : System.Web.UI.UserControl
    {
        #region Methods

        protected void OnCreatedUser(object sender, EventArgs e)
        {
            string strMemberGroups = Upac.Core.Utilities.CommonUtil.GetSetting("Membership/CreateUserWizard/DefaultMemberGroups");
            string[] memberGroups = strMemberGroups.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string memberGroup in memberGroups)
            {
                if (System.Web.Security.Roles.RoleExists(memberGroup))
                {
                    System.Web.Security.Roles.AddUserToRole(createUser.UserName, memberGroup);
                }
            }

            string secretCode = Upac.Core.Security.RandomPassword.Generate(CommonUtil.GetSetting("Membership/SecretCodeLength", 7));
            umbraco.cms.businesslogic.member.Member member =
                umbraco.cms.businesslogic.member.Member.GetMemberFromLoginName(createUser.UserName);
            if (member != null)
            {

                Property propertyCode = member.getProperty("SecretCode");
                if (propertyCode != null)
                {
                    propertyCode.Value = secretCode;
                }

                if (CommonUtil.GetSetting("Membership/CreateUserWizard/ShowNickname", false))
                {
                    TextBox tbNickname = (TextBox)CreateUserWizardStep.ContentTemplateContainer.FindControl("tbNickname");
                    if (tbNickname != null)
                    {
                        Property propertyNickname = member.getProperty("nickname");
                        propertyNickname.Value = tbNickname.Text;
                    }
                }
                member.Save();
            }

            string activateLink = string.Concat(GetBaseUrl(), CommonUtil.GetSetting("Membership/ActivateUrl"), "?c=", HttpUtility.UrlEncode(secretCode), "&u=", HttpUtility.UrlEncode(createUser.UserName));

            string receiptToScreen = string.Empty;
            // AutoApprove, ConfirmEmailFirst eller NeedApproval
            string approval = CommonUtil.GetSetting("Membership/CreateUserWizard/Approval");
            if (approval == "AutoApprove")
            {
                receiptToScreen = CommonUtil.GetSetting("Membership/CreateUserWizard/ReceiptAutoApprove/Screen");
            }
            else if (approval == "ConfirmEmailFirst")
            {
                receiptToScreen = CommonUtil.GetSetting("Membership/CreateUserWizard/ReceiptConfirmEmailFirst/Screen");

                Message message = new Message();
                message.Body = CommonUtil.GetSetting("Membership/CreateUserWizard/ReceiptConfirmEmailFirst/EmailBody");
                message.Subject = CommonUtil.GetSetting("Membership/CreateUserWizard/ReceiptConfirmEmailFirst/EmailSubject");
                message.To.Add(createUser.Email);
                message.TemplateVariables.Add("link", activateLink);
                message.Send();
            }
            else if (approval == "NeedApproval")
            {
                receiptToScreen = CommonUtil.GetSetting("Membership/CreateUserWizard/ReceiptNeedApproval/Screen");
            }

            Literal ltrReceipt = (Literal)CompleteWizardStep.ContentTemplateContainer.FindControl("ltrReceipt");
            if (ltrReceipt != null)
            {
                ltrReceipt.Text = receiptToScreen;
            }
        }

        protected void OnCreateUserError(object sender, CreateUserErrorEventArgs e)
        {
        }

        protected void OnCreatingUser(object sender, LoginCancelEventArgs e)
        {
            createUser.Email = createUser.UserName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Just ensure that the user is not logged in, when trying to create a user profile
            if (!Page.IsPostBack)
            {
                FormsAuthentication.SignOut();
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

        #endregion Methods
    }
}