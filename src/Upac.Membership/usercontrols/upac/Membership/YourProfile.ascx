<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YourProfile.ascx.cs" Inherits="Upac.Membership.UserControls.YourProfile" %>
<asp:MultiView ID="multiView" runat="server" ActiveViewIndex="0">
    <asp:View ID="viewProfile" runat="server">
        Navn: <%# Member.getProperty("nickname").Value %><br />
        Email: <%# Member.Email %><br />
        <asp:Button ID="btShowEditView" runat="server" Text="Rediger" OnClick="ShowEditView" />
    </asp:View>
    <asp:View ID="viewProfileEdit" runat="server">
        Not implemented
        <asp:Button ID="btShowProfileView" runat="server" Text="Fortryd" OnClick="ShowProfileView" />
    </asp:View>
</asp:MultiView>