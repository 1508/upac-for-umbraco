<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TracDashboard.ascx.cs" Inherits="Upac.Trac.TracDashboard" %>
<br />
<div style="color: red; font-weight:bold;">
	<asp:Literal ID="ltrMessage" runat="server" />&nbsp;
</div>
<br />
<fieldset>
	<asp:Label ID="Label1" runat="server" AssociatedControlID="tbUsername">
		Trac Username
	</asp:Label><br />
	<asp:TextBox ID="tbUsername" runat="server" Width="200" />
	<br /><br />
	<asp:Label ID="Label2" runat="server" AssociatedControlID="tbUsername">
		Trac Password
	</asp:Label><br />
	<asp:TextBox ID="tbPassword" runat="server" TextMode="Password" Width="200" />
	<asp:Button ID="btDeleteSessionPassword" Text="Delete password in session" runat="server" OnClick="DeleteSessionPassword" Visible="false" />
	<asp:Button ID="btSetSessionPassword" Text="Save password in session"  runat="server" OnClick="SetSessionPassword" Visible="true" />
	<br /><br />
	
	<asp:Label ID="Label3" runat="server" AssociatedControlID="ddlMilestone">
		Milestone
	</asp:Label><br />
	<asp:DropDownList ID="ddlMilestone" runat="server">
		<asp:ListItem Value="UPAC 4.0.3.1" Text="UPAC 4.0.3.1" Selected="True"/>
		<asp:ListItem Value="UPAC 4.0.3.2" Text="UPAC 4.0.3.2"/>
		<asp:ListItem Value="UPAC 4.0.3.3" Text="UPAC 4.0.3.3"/>
		<asp:ListItem Value="UPAC Next" Text="UPAC Next"/>
	</asp:DropDownList>
	<br /><br />
	
	<asp:Label ID="Label6" runat="server" AssociatedControlID="ddlPriority">
		Priority
	</asp:Label><br />
	<asp:DropDownList ID="ddlPriority" runat="server">
		<asp:ListItem Value="blocker" Text="Blocker" />
		<asp:ListItem Value="critical" Text="Critical" />
		<asp:ListItem Value="major" Text="Major" Selected="True" />
		<asp:ListItem Value="minor" Text="Minor" />
		<asp:ListItem Value="trivial" Text="Trivial" />
	</asp:DropDownList>
	<br /><br />
	
	<asp:Label ID="Label7" runat="server" AssociatedControlID="tbEstimatedNumberOfHours">
		Estimated Number of hours
	</asp:Label><br />
	<asp:TextBox ID="tbEstimatedNumberOfHours" runat="server" Width="500" Text="1" />
	<br /><br />
	
	<asp:Label ID="Label4" runat="server" AssociatedControlID="tbSummery">
		Summery
	</asp:Label><br />
	<asp:TextBox ID="tbSummery" runat="server" Width="500" />
	<br /><br />
	<asp:Label ID="Label5" runat="server" AssociatedControlID="tbContent">
		Content
	</asp:Label><br />
	<asp:TextBox ID="tbContent" runat="server" TextMode="MultiLine" Width="500" Height="200" />
	<br /><br />
	<asp:Button ID="btSave" runat="server" Text="Create ticket page" OnClick="CreateWikiPage" />
</fieldset>