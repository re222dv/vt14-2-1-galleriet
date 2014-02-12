<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="vt14_2_1_galleriet.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <%-- Message of successful upload --%>
            <asp:Panel ID="Success" runat="server" CssClass="good" Visible="false">
                Uppladdningen lyckades!
                <asp:Button ID="Close" runat="server" Text="Stäng" CausesValidation="False" OnClick="Close_Click"></asp:Button>
            </asp:Panel>
            
            <%-- Big image --%>
            <asp:Image ID="Image" runat="server" />
            
            <%-- Small images --%>
            <aside>
                <asp:Repeater ID="Repeater" runat="server" ItemType="vt14_2_1_galleriet.Model.Picture" SelectMethod="Repeater_GetData">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="<%# Item.Thumb %>" NavigateUrl='<%# "?name=" + Item.Name %>' />
                    </ItemTemplate>
                </asp:Repeater>
            </aside>
        </main>

        <%-- Upload image --%>
        <footer>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="error" />
            <asp:FileUpload ID="FileUpload" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Välj en fil" ControlToValidate="FileUpload" Display="None"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Endast filtyperna gif, jpeg eller png är tillåtna." Display="None" ControlToValidate="FileUpload" ValidationExpression="(?i:^.*\.(gif|jpg|png)$)"></asp:RegularExpressionValidator>
            <asp:Button ID="Button" runat="server" Text="Ladda upp" OnClick="Button_Click" />
        </footer>
    </form>
</body>
</html>
