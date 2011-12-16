<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Create.ascx.cs" Inherits="Upac.Membership.UserControls.Create" %>
<%@ Import Namespace="Upac.Core.Utilities"%>
<script language="C#" runat="server">
    // You can hook in, with custom code here
    void Page_OnCreateUserError(object sender, CreateUserErrorEventArgs e) { OnCreateUserError(sender, e); }
    void Page_OnCreatedUser(object sender, EventArgs e) { OnCreatedUser(sender, e); }
    void Page_OnCreatingUser(object sender, LoginCancelEventArgs e) { OnCreatingUser(sender, e); }
</script>
<asp:CreateUserWizard ID="createUser" runat="server" HeaderText=""
    RequireEmail="false"

    DisableCreatedUser='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/Approval") != "AutoApprove" %>'

    UnknownErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/UnknownErrorMessage") %>'

    CreateUserButtonText='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/CreateUserButtonText") %>'

    DuplicateUserNameErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/DuplicateUserNameErrorMessage") %>'
    DuplicateEmailErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/DuplicateUserNameErrorMessage") %>'

    OnCreateUserError="Page_OnCreateUserError"
    OnCreatedUser="Page_OnCreatedUser"
    OnCreatingUser="Page_OnCreatingUser"
>
<WizardSteps>
    <asp:CreateUserWizardStep Title="" runat="server" ID="CreateUserWizardStep">
        <ContentTemplate>
            <fieldset class="CreateUserWizard">
                <legend></legend>
                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                <br />
                <asp:PlaceHolder ID="placeholderNickname" runat="server" Visible='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/ShowNickname", false) %>'>
                    <asp:Label id="labelForNickname" AssociatedControlID="tbNickname" runat="server">
                        <%# CommonUtil.GetSetting("Membership/CreateUserWizard/NicknameLabelText") %>
                    </asp:Label>
                    <br />
                    <asp:TextBox ID="tbNickname" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNickname"
                        ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/NicknameRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/NicknameRequiredErrorMessage") %>' ValidationGroup="createUser">*</asp:RequiredFieldValidator>
                    <br />
                    <br />
                </asp:PlaceHolder>
                <asp:Label ID="labelForUserName" AssociatedControlID="UserName" runat="server">
                    <%# CommonUtil.GetSetting("Membership/CreateUserWizard/UserNameLabelText") %>
                </asp:Label>
                <br />
                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/EmailRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/EmailRequiredErrorMessage") %>' ValidationGroup="createUser">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="UserName"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/EmailRegularExpressionErrorMessage") %>' ValidationExpression='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/EmailRegularExpression") %>' ValidationGroup="createUser"></asp:RegularExpressionValidator><br />
                <br />
                <asp:label ID="labelPassword1" AssociatedControlID="Password" runat="server">
                    <%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordLabelText") %>
                </asp:label>
                <br />
                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRequiredErrorMessage") %>' ValidationGroup="createUser">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Password"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRegularExpressionErrorMessage") %>' ValidationExpression='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/PasswordRegularExpression") %>' ValidationGroup="createUser"></asp:RegularExpressionValidator><br />
                <br />
                <asp:Label ID="labelPassword2" AssociatedControlID="ConfirmPassword" runat="server">
                    <%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordLabelText") %>
                </asp:Label>
                <br />
                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ConfirmPassword"
                    ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordRequiredErrorMessage") %>' ValidationGroup="createUser">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                    ControlToValidate="ConfirmPassword" ErrorMessage='<%# CommonUtil.GetSetting("Membership/CreateUserWizard/ConfirmPasswordCompareErrorMessage") %>'
                    ValidationGroup="createUser"></asp:CompareValidator>
                 <asp:TextBox ID="Email" runat="server" Visible="false"></asp:TextBox>
            </fieldset>

        </ContentTemplate>
    </asp:CreateUserWizardStep>
    <asp:CompleteWizardStep Title="" runat="server" ID="CompleteWizardStep">
        <ContentTemplate>
            <asp:Literal ID="ltrReceipt" runat="server" />
        </ContentTemplate>
    </asp:CompleteWizardStep>
</WizardSteps>

</asp:CreateUserWizard>