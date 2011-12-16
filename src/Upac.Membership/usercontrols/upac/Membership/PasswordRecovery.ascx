<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.ascx.cs" Inherits="Upac.Membership.UserControls.PasswordRecovery" %>
<%@ Import Namespace="Upac.Core.Utilities"%>
<asp:MultiView ID="multiView" runat="server" ActiveViewIndex="0">
    <asp:View ID="viewProvideDetails" runat="server">
        <p>
            <%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/InstructionText")%>
        </p>
        <asp:Label ID="labelFor" runat="server">
            <%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/EmailLabelText")%>
        </asp:Label>
        <br />
        <asp:TextBox ID="tbEmail" runat="server" />
        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="tbEmail"
            ErrorMessage='<%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/EmailRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/EmailRequiredErrorMessage") %>' ValidationGroup="recover">*</asp:RequiredFieldValidator>
        <br />
        <asp:Button Text='<%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideDetails/PasswordRecoveryButtonText")%>' runat="server" OnClick="RecoverLoginInfo" ValidationGroup="recover" />
    </asp:View>
    <asp:View ID="viewReceipt" runat="server">
        <p>
            <asp:Literal ID="ltrReceipt" runat="server" />
        </p>
    </asp:View>
    <asp:View ID="viewProvideNewPassword" runat="server">
        <asp:HiddenField ID="hidddenCode" runat="server" />
        <asp:HiddenField ID="hidddenUsername" runat="server" />
        <p>
            <%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideNewPassword/InstructionText")%>
        </p>
                <asp:label ID="labelPassword1" AssociatedControlID="Password" runat="server">
                    <%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordLabelText") %>
                </asp:label>
                <br />
                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRequiredErrorMessage") %>' ValidationGroup="ResetPassword">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Password"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRegularExpressionErrorMessage") %>' ValidationExpression='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRegularExpression") %>' ValidationGroup="ResetPassword"></asp:RegularExpressionValidator><br />
                <br />
                <asp:Label ID="labelPassword2" AssociatedControlID="ConfirmPassword" runat="server">
                    <%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordLabelText") %>
                </asp:Label>
                <br />
                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ConfirmPassword"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordRequiredErrorMessage") %>' ValidationGroup="ResetPassword">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                    ControlToValidate="ConfirmPassword" ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordCompareErrorMessage") %>'
                    ValidationGroup="ResetPassword"></asp:CompareValidator>
        <br />
        <asp:Button ID="Button1" Text='<%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewProvideNewPassword/ResetButtonText")%>' runat="server" OnClick="ResetPassword" ValidationGroup="ResetPassword" />
    </asp:View>
    <asp:View ID="viewPasswordIsUpdated" runat="server">
        <p>
            <%# CommonUtil.GetSetting("Membership/PasswordRecovery/ViewPasswordIsUpdated/PasswordIsUpdated")%>
        </p>
    </asp:View>
</asp:MultiView>