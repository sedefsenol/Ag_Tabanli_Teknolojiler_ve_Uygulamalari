<%@ Page Title="Kayıt Ol" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Kayitol.aspx.cs"
    Inherits="Cinescope.Kayitol" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="giris-card">
    <h2>CineScope Kayıt Ol</h2>

    <asp:Label ID="lblMesaj" runat="server"
        CssClass="text-danger mb-3 d-block text-center"></asp:Label>

    <div class="mb-3">
        <asp:TextBox ID="txtIsim" runat="server"
            CssClass="form-control giris-input"
            Placeholder="Ad Soyad"></asp:TextBox>
    </div>

    <div class="mb-3">
        <asp:TextBox ID="txtEmail" runat="server"
            CssClass="form-control giris-input"
            Placeholder="E-posta"></asp:TextBox>
    </div>

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

    <div class="mb-3">
        <asp:TextBox ID="txtSifreTekrar" runat="server"
            CssClass="form-control giris-input"
            TextMode="Password"
            Placeholder="Şifre Tekrar"></asp:TextBox>
    </div>

    <asp:Button ID="btnKayitOl" runat="server"
        Text="Kayıt Ol"
        CssClass="btn btn-primary giris-btn"
        OnClick="btnKayitOl_Click" />

    <div class="text-center mt-3">
        <small>Zaten hesabın var mı? <a href="Girisyap.aspx">Giriş Yap</a></small>
    </div>
</div>

</asp:Content>
