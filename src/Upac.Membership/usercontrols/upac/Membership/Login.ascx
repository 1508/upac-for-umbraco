<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="Upac.Membership.UserControls.Login" %>
<%@ Import Namespace="Upac.Core.Utilities"%>
<asp:LoginView ID="loginView" runat="server">
    <LoggedInTemplate>
        <%# CommonUtil.GetSetting("Membership/Login/YouAreNowLoggedIn") %>
    </LoggedInTemplate>
</asp:LoginView>
<asp:Login
    id="login"
    runat="server"
    FailureText=""
    TitleText=""
    VisibleWhenLoggedIn="false"
    OnLoginError="OnLoginError"
    OnLoggedIn="OnLoggedIn"
>
    <LayoutTemplate>
            <p>
                <%# CommonUtil.GetSetting("Membership/Login/InstructionText") %>
            </p>
            <p>
                <asp:Label ForeColor="Red" ID="FailureText" runat="server" />            
            </p>
            <asp:Label ID="labelForUserName" AssociatedControlID="UserName" runat="server">
                <%# CommonUtil.GetSetting("Membership/Login/UserNameLabelText") %>
            </asp:Label>
            <br />
            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                ErrorMessage='<%# CommonUtil.GetSetting("Membership/Login/UserNameRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/Login/UserNameRequiredErrorMessage") %>' ValidationGroup="login">*</asp:RequiredFieldValidator>
            <br />
            <asp:label ID="labelForPassword" AssociatedControlID="Password" runat="server">
                <%# CommonUtil.GetSetting("Membership/Login/PasswordLabelText") %>
            </asp:label>
            <br />
            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                ErrorMessage='<%# CommonUtil.GetSetting("Membership/Login/PasswordRequiredErrorMessage") %>' ToolTip='<%# CommonUtil.GetSetting("Membership/Login/PasswordRequiredErrorMessage") %>' ValidationGroup="login">*</asp:RequiredFieldValidator>
            <br />
            <asp:CheckBox ID="RememberMe" Visible='<%# CommonUtil.GetSetting("Membership/Login/DisplayRememberMe", false) %>' runat="server" Text='<%# CommonUtil.GetSetting("Membership/Login/RememberMeText") %>' />
            <br />
            <a href="<%# CommonUtil.GetSetting("Membership/CreateUserUrl") %>">
                <%# CommonUtil.GetSetting("Membership/Login/CreateUserText") %>
            </a>
            <br />
            <a href="<%# CommonUtil.GetSetting("Membership/PasswordRecoveryUrl") %>">
                <%# CommonUtil.GetSetting("Membership/Login/PasswordRecoveryText") %>
            </a>
            <br />
            <asp:button id="Login" CommandName="Login" runat="server" Text='<%# CommonUtil.GetSetting("Membership/Login/LoginButtonText") %>' ValidationGroup="login"></asp:button>
    </LayoutTemplate>
</asp:Login>