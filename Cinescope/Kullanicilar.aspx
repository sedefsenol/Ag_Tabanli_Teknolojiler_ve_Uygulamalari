<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Kullanicilar.aspx.cs" Inherits="Cinescope.Kullanicilar" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kullanıcı Listesi</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridViewKullanicilar" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="ID" />
                    <asp:BoundField DataField="username" HeaderText="Kullanıcı Adı" />
                    <asp:BoundField DataField="email" HeaderText="E-posta" />
                    <asp:BoundField DataField="role" HeaderText="Rol" />
                    <asp:BoundField DataField="created_at" HeaderText="Oluşturulma Tarihi" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>