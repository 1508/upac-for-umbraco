namespace Upac.Membership
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    using umbraco.cms.businesslogic.member;
    using umbraco.cms.businesslogic.property;

    using Upac.Core.Utilities;

    public static class Helper
    {
        #region Methods

        public static Member GetMember(MembershipUser user)
        {
            return umbraco.cms.businesslogic.member.Member.GetMemberFromLoginName(user.UserName);
        }

        public static MembershipUser GetMembershipUser(string username, string secretCode)
        {
            if (!string.IsNullOrEmpty(username))
            {
                // Is it a email?
                if (username.Contains("@"))
                {
                    username = System.Web.Security.Membership.GetUserNameByEmail(username);
                    if (string.IsNullOrEmpty(username))
                    {
                        return null;
                    }
                }
                MembershipUser user = System.Web.Security.Membership.GetUser(username);
                Member member = GetMember(user);
                if (member != null)
                {
                    Property property = member.getProperty("SecretCode");
                    if (property != null && property.Value.ToString() == secretCode)
                    {
                        return user;
                    }
                }

            }
            return null;
        }

        public static MembershipUser GetMembershipUserViaEmail(string email)
        {
            string username = System.Web.Security.Membership.GetUserNameByEmail(email);
            if (!string.IsNullOrEmpty(username))
            {
                return System.Web.Security.Membership.GetUser(username);
            }
            return null;
        }

        public static bool UpdateSecretCode(string username)
        {
            Member member = umbraco.cms.businesslogic.member.Member.GetMemberFromLoginName(username);
            return UpdateSecretCode(member);
        }

        public static bool UpdateSecretCode(MembershipUser user)
        {
            Member member = GetMember(user);
            return UpdateSecretCode(member);
        }

        public static bool UpdateSecretCode(Member member)
        {
            string secretCode = Upac.Core.Security.RandomPassword.Generate(CommonUtil.GetSetting("Membership/SecretCodeLength", 7));
            if (member != null)
            {
                Property property = member.getProperty("SecretCode");
                if (property != null)
                {
                    property.Value = secretCode;
                    member.Save();
                }
            }
            return false;
        }

        #endregion Methods
    }
}