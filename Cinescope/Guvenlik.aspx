<%@ Page Title="Güvenlik" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Guvenlik.aspx.cs"
    Inherits="Cinescope.Guvenlik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --primary:#ffb3d9;
    --primary-soft:#ffd9eb;
    --bg:#1f1f1f;
    --card:#262626;
    --border:#3a3a3a;
    --text:#eee;
    --muted:#9a9a9a;
}

body{
    background:var(--bg)!important;
    color:var(--text);
}

.hesapayarlari-container{
    width:95%;
    max-width:1000px;
    margin:40px 0 100px 40px;
    background:var(--card);
    border:1px solid var(--border);
    border-radius:18px;
    padding:30px 34px 36px;
}

.hesapayarlari-title{
    font-size:1.6rem;
    font-weight:800;
    color:var(--primary);
    border-left:6px solid var(--primary);
    padding-left:14px;
    margin-bottom:25px;
}

.hesapayarlari-nav{
    display:flex;
    gap:30px;
    border-bottom:1px solid var(--border);
    padding-bottom:12px;
    margin-bottom:30px;
}

.hesapayarlari-nav a{
    font-size:.95rem;
    font-weight:700;
    color:var(--muted);
    text-decoration:none;
    padding-bottom:8px;
}

.hesapayarlari-nav a.active,
.hesapayarlari-nav a:hover{
    color:var(--primary);
    border-bottom:3px solid var(--primary);
}

.hesapayarlari-field{
    display:flex;
    flex-direction:column;
    gap:8px;
    margin-bottom:22px;
}

.hesapayarlari-field span{
    font-size:.9rem;
    color:var(--muted);
    font-weight:600;
}

.hesapayarlari-input{
    background:#1e1e1e;
    border:1px solid var(--border);
    color:var(--text);
    padding:10px 12px;
    border-radius:10px;
    font-size:.9rem;
    outline:none;
}

.hesapayarlari-input:focus{
    border-color:var(--primary);
}

.hesapayarlari-button{
    background:var(--primary);
    border:none;
    color:#000;
    font-weight:800;
    padding:12px 22px;
    border-radius:30px;
    font-size:.95rem;
    cursor:pointer;
    transition:.2s;
    margin-top:10px;
}

.hesapayarlari-button:hover{
    background:var(--primary-soft);
}

.hesapayarlari-message{
    display:block;
    margin-top:18px;
    font-size:.9rem;
    color:var(--primary);
    font-weight:600;
}
</style>

<div class="hesapayarlari-container">

    <div class="hesapayarlari-title">
        Hesap Ayarları
    </div>

    <div class="hesapayarlari-nav">
        <a href="HesapAyarlari.aspx">Profil</a>
        <a href="Guvenlik.aspx" class="active">Güvenlik</a>
    </div>

    <div class="hesapayarlari-field">
        <span>E-mail</span>
        <asp:TextBox ID="txtEmail" runat="server"
                     CssClass="hesapayarlari-input" />
    </div>

    <div class="hesapayarlari-field">
        <span>Mevcut Şifre</span>
        <asp:TextBox ID="txtMevcut" runat="server"
                     CssClass="hesapayarlari-input"
                     TextMode="Password"
                     placeholder="Mevcut şifrenizi giriniz" />
    </div>

    <div class="hesapayarlari-field">
        <span>Yeni Şifre</span>
        <asp:TextBox ID="txtYeni" runat="server"
                     CssClass="hesapayarlari-input"
                     TextMode="Password"
                     placeholder="Yeni şifrenizi giriniz" />
    </div>

    <div class="hesapayarlari-field">
        <span>Yeni Şifre (Tekrar)</span>
        <asp:TextBox ID="txtYeniTekrar" runat="server"
                     CssClass="hesapayarlari-input"
                     TextMode="Password"
                     placeholder="Yeni şifreyi tekrar giriniz" />
    </div>

    <asp:Button ID="btnKaydet" runat="server"
                Text="Kaydet"
                CssClass="hesapayarlari-button"
                OnClick="btnKaydet_Click" />

    <asp:Label ID="lblMesaj" runat="server"
               CssClass="hesapayarlari-message" />

</div>

</asp:Content>
