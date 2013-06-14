<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="Hash" DataSourceID="SqlDataSource1" EmptyDataText="没有可显示的数据记录。">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="Hash" HeaderText="Hash" ReadOnly="True" SortExpression="Hash" />
                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size" />
                <asp:CheckBoxField DataField="IsFolder" HeaderText="IsFolder" SortExpression="IsFolder" />
                <asp:CheckBoxField DataField="Pass" HeaderText="Pass" SortExpression="Pass" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ShareWareConnectionString1 %>" DeleteCommand="DELETE FROM [FileInfo] WHERE [Hash] = @Hash" InsertCommand="INSERT INTO [FileInfo] ([Hash], [Size], [IsFolder], [Pass]) VALUES (@Hash, @Size, @IsFolder, @Pass)" ProviderName="<%$ ConnectionStrings:ShareWareConnectionString1.ProviderName %>" SelectCommand="SELECT [Hash], [Size], [IsFolder], [Pass] FROM [FileInfo]" UpdateCommand="UPDATE [FileInfo] SET [Size] = @Size, [IsFolder] = @IsFolder, [Pass] = @Pass WHERE [Hash] = @Hash">
            <DeleteParameters>
                <asp:Parameter Name="Hash" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Hash" Type="String" />
                <asp:Parameter Name="Size" Type="Int64" />
                <asp:Parameter Name="IsFolder" Type="Boolean" />
                <asp:Parameter Name="Pass" Type="Boolean" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="Size" Type="Int64" />
                <asp:Parameter Name="IsFolder" Type="Boolean" />
                <asp:Parameter Name="Pass" Type="Boolean" />
                <asp:Parameter Name="Hash" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
