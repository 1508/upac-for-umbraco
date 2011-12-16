<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PropertyNameChanger.ascx.cs" Inherits="Upac.Dashboards.PropertyNameChanger" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umb" %>
<umb:Pane Text="Change property names" runat="server">
	<umb:PropertyPanel ID="PropertyPanel2" runat="server" Text="Properties affected">
		<asp:Literal ID="ltrRowsAffected" Text="0" runat="server" />
	</umb:PropertyPanel>
	<umb:PropertyPanel runat="server" Text="Find text">
		<asp:DropDownList ID="ddlFind" runat="server" />
	</umb:PropertyPanel>
	<umb:PropertyPanel runat="server" Text="Replace with dictionary item">
		<asp:DropDownList ID="ddlDictionary" runat="server" />
	</umb:PropertyPanel>
	<umb:PropertyPanel ID="PropertyPanel1" runat="server" Text="">
		<asp:Button ID="bt" Text="Save" runat="server" OnClick="Save" />
	</umb:PropertyPanel>
</umb:Pane>