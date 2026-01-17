<%@ Page Title="Giriş Yap" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Girisyap.aspx.cs"
    Inherits="Cinescope.Girisyap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<div class="giris-card">
    <h2>CineScope Giriş Yap</h2>

    <asp:Label ID="lblMesaj" runat="server"
        CssClass="text-danger mb-3 d-block"></asp:Label>

    <div class="mb-3">
        <asp:TextBox ID="txtKullaniciAdi" runat="server"
            CssClass="form-control giris-input"
            Placeholder="Kullanıcı Adı"></asp:TextBox>
    </div>

    <div class="mb-3">
        <asp:TextBox ID="txtSifre" runat="server"
            CssClass="form-control giris-input"
            TextMode="Password"
            Placeholder="Şifre"></asp:TextBox>
    </div>

    <asp:Button ID="btnGirisYap" runat="server"
        Text="Giriş Yap"
        CssClass="btn btn-primary giris-btn"
        OnClick="btnGirisYap_Click" />

    <div class="text-center mt-3">
        <small>Hesabınız yok mu? <a href="Kayitol.aspx">Kayıt Ol</a></small>
    </div>
</div>

</asp:Content>
