<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PhotoAlbum._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PhotoAlbum</title>
    <style type="text/css">
        body {
            font-family: Tahoma, Verdana;
            font-size: 10pt;
        }
        h1 { font-size: 18pt; color: #666}
        h2 { font-size: 12pt; margin-bottom:0.4em}
        .newdrive { float: right}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>PhotoAlbum</h1>
        <asp:Panel ID="SelectDrive" runat="server" Visible="false">
          <asp:LinkButton ID="NewDrive" runat="server" Text="New Drive" onclick="NewDrive_Click" CssClass="newdrive" />
          Mounted Drives: 
          <asp:DropDownList ID="MountedDrives" runat="server" AutoPostBack="true"
                            DataTextField="Name" DataValueField="Value"
                            OnSelectedIndexChanged="MountedDrives_SelectedIndexChanged" />
        </asp:Panel>
        <h2>Image Store Drive: (<%=this.CurrentPath%>)</h2>
        <asp:GridView DataSourceID="LinqDataSource1" AutoGenerateColumns="False" 
            ID="GridView1" runat="server" CellPadding="8" EnableModelValidation="True" 
            ForeColor="#333333" GridLines="None" Width="100%" DataKeyNames="FullName"
            EmptyDataText="No files available" onrowcommand="GridView1_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Length"  HeaderText="Length" ReadOnly="True" SortExpression="Length">
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="CreationTime" HeaderText="Date Created" ReadOnly="True" SortExpression="CreationTime" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="LastWriteTime" HeaderText="Last Updated" SortExpression="LastWriteTime">
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:ButtonField ButtonType="Link" CommandName="Delete" Text="Delete" />
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
            OnContextCreating="LinqDataSource1_ContextCreating"
            TableName="Files" Select="new (Name, Length, CreationTime, LastWriteTime, FullName)">
        </asp:LinqDataSource>
    </div>
    </form>
</body>
</html>
