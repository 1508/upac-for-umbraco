<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipInstaller.ascx.cs" Inherits="Upac.Membership.Installer.UserControl.MembershipInstaller" %>
<asp:Wizard ID="wizard" runat="server" DisplaySideBar="false">
    <WizardSteps>
        <asp:WizardStep AllowReturn="false" ID="stepIntro" StepType="Start" Title="Introduction">
            <p>
                Macroes has been installed.<br />
                To following steps will be done, when you click next.
            </p>
            <ul>
                <li>Set macroes to not render content in rich text editor</li>
                <li>Install settings and content nodes</li>
                <li>Create member group and member type</li>
                <li>Setup membership in web.config</li>
            </ul>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="false" ID="stepInstallSettings" StepType="Step" OnActivate="UpdateMacroes">
            <p>
                Macroes is updated
            </p>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="false" ID="WizardStep5" StepType="Step" OnActivate="InstallSettingsAndContent">
            <p>
                Settings and content installed, next up content
            </p>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="false" ID="WizardStep4" StepType="Step" OnActivate="CreateMemberTypeAndMemberGroup">
            <p>
                Member type and member group created. Next up web.config changes
            </p>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="false" ID="WizardStep6" StepType="Step" OnActivate="UpdateWebConfig">
            <p>
                web.config changed.
            </p>
        </asp:WizardStep>
        <asp:WizardStep AllowReturn="false" ID="WizardStep1" StepType="Complete" Title="Finish">
            <p>
                You are done :-)
                <br />
                Please protect the page /bruger/Din profil
                <br />
                Please setup the Upac.Membership.Navigation. Normaly near the servive navigation in the Main.master file.
                <br />
                &lt;umbraco:Macro Alias="Upac.Membership.Navigation" runat="server"&gt;&lt;/umbraco:Macro&gt;
            </p>
        </asp:WizardStep>
    </WizardSteps>
</asp:Wizard>
