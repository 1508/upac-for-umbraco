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

    public partial class YourProfile : System.Web.UI.UserControl
    {
        #region Fields

        private Member member;
        private MembershipUser membershipUser;

        #endregion Fields

        #region Properties

        public Member Member
        {
            get
            {
                if (member == null)
                {
                    member = Upac.Membership.Helper.GetMember(MembershipUser);
                    Upac.Core.Diagnostics.Assert.EnsureNotNullReference(member, "Member");
                }
                return member;
            }
        }

        public MembershipUser MembershipUser
        {
            get
            {
                if (membershipUser == null)
                {
                    membershipUser = System.Web.Security.Membership.GetUser();
                    Upac.Core.Diagnostics.Assert.EnsureNotNullReference(membershipUser, "MembershipUser");
                }
                return membershipUser;
            }
        }

        #endregion Properties

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ShowEditView(object sender, EventArgs e)
        {
            multiView.SetActiveView(viewProfileEdit);
        }

        protected void ShowProfileView(object sender, EventArgs e)
        {
            multiView.SetActiveView(viewProfile);
        }

        #endregion Methods
    }
}