<%@ Page Language="vb" AutoEventWireup="true" CodeFile="default.aspx.vb" Inherits="_default" %>

<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
	TagPrefix="dx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>How to add/remove/edit DataView items on a callback</title>
	<style type="text/css">
		.combo { 
			display: inline-block;
		}
		.btn { 
			border: 0;
		}
	</style>

	<script type="text/javascript" language="javascript">
		function DeleteItem(id) {
			if(confirm('Do you want to delete this item?')) {
				dataView.PerformCallback('delete=' + id);    
			}
		}
		function AddItem(s, e) {
			dataView.PerformCallback('add');
		}
	</script>

</head>
<body>
	<form id="form1" runat="server">
		<div>
			<dx:ASPxDataView ID="dataView" runat="server" Width="100%" AllowPaging="False" ColumnCount="1"
				ItemSpacing="32px" ClientInstanceName="dataView" EnableViewState="false" OnCustomCallback="dataView_CustomCallback">
				<ItemTemplate>
					<dx:ASPxComboBox runat="server" ValueType="System.Int32" ID="comboItemValue" CssClass="combo"
						Value='<%#Eval("ItemValue")%>'>
						<Items>
							<dx:ListEditItem Text="(none)" Value="-1" />
							<dx:ListEditItem Text="One" Value="1" />
							<dx:ListEditItem Text="Two" Value="2" />
							<dx:ListEditItem Text="Three" Value="3" />
						</Items>
					</dx:ASPxComboBox>
					<a runat="server" id="deleteLink" href='<%#GetDeleteLink(Eval("Id"))%>'>
						<img src="delete.gif" alt="Delete Button" title="Delete" width="16" height="16" class="btn" /></a>
				</ItemTemplate>
				<ItemStyle BackColor="Transparent" Height="0px" Width="200px">
					<Paddings Padding="0px" />
					<Border BorderStyle="None" />
				</ItemStyle>
				<Paddings Padding="20px" />
			</dx:ASPxDataView>
			<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Add Item">
				<ClientSideEvents Click="AddItem" />
			</dx:ASPxButton>
			<br />
			<dx:ASPxButton ID="btSave" runat="server" OnClick="btSave_Click" Text="Save">
			</dx:ASPxButton>
		</div>
	</form>
</body>
</html>