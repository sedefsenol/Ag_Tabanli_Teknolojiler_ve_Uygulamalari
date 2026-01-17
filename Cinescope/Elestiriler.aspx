<%@ Page Title="Eleştiriler" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Elestiriler.aspx.cs"
    Inherits="Cinescope.Elestiriler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --p:#ffb3d9;--bg:#1f1f1f;--card:#262626;--b:#3a3a3a;--t:#eee;--tl:#ccc;
}
body{background:var(--bg)!important;color:var(--t);}

.page{width:90%;margin:40px auto 80px;}

.title{
    font-size:1.7rem;font-weight:800;color:var(--p);
    border-left:6px solid var(--p);padding-left:12px;
    margin:45px 0 25px;
}

.review{
    display:flex;gap:14px;background:var(--card);
    padding:16px;border-radius:14px;border-left:4px solid var(--p);
    margin-bottom:16px;cursor:pointer;
    transition:.2s;
}
.review:hover{transform:translateY(-3px);}

.review img{
    width:80px;height:120px;border-radius:8px;object-fit:cover;
}
.review h3{color:var(--p);margin:0 0 4px;font-size:1.05rem;}
.review p{color:var(--tl);font-size:.9rem;margin:0;}
.review small{color:#777;font-size:.8rem;}

.users{
    display:flex;
    justify-content:space-between;
    gap:12px;
    overflow:hidden;
}
.user{
    flex:1;max-width:19%;
    background:var(--card);
    border:1px solid var(--b);
    border-radius:12px;
    padding:12px;
    display:flex;gap:10px;align-items:center;
    cursor:pointer;
}
.user img{
    width:38px;height:38px;border-radius:50%;
}
.user div b{font-size:.9rem;}
.user div span{font-size:.75rem;color:var(--tl);}
</style>

<div class="page">

<div class="title">Son Eleştiriler</div>
<asp:Repeater ID="rptSonElestiriler" runat="server">
<ItemTemplate>
    <div class="review"
         onclick="location.href='<%# Eval("Tur").ToString()=="Film" 
             ? "Filmsayfasi.aspx?id="+Eval("FilmID")
             : "Dizisayfasi.aspx?id="+Eval("DiziID") %>'">
        <img src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>">
        <div>
            <h3><%# Eval("Baslik") %></h3>
            <p><%# Eval("Yorum") %></p>
            <small><%# Eval("Tarih","{0:dd.MM.yyyy}") %></small>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>


<div class="title">Popüler Kullanıcılar</div>
<div class="users">
<asp:Repeater ID="rptPopulerKullanicilar" runat="server">
<ItemTemplate>
    <div class="user"
         onclick="location.href='Kullanici.aspx?id=<%# Eval("ID") %>'">
        <img src="<%# Eval("ProfilFoto") %>" onerror="this.src='Gorsel/pp.png'">
        <div>
            <b><%# Eval("KullaniciAdi") %></b><br>
            <span>Profiline göz at</span>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>
</div>
<div class="title">Popüler Eleştiriler</div>
<asp:Repeater ID="rptPopulerElestiriler" runat="server">
<ItemTemplate>
    <div class="review"
         onclick="location.href='<%# Eval("Tur").ToString()=="Film" 
             ? "Filmsayfasi.aspx?id="+Eval("FilmID")
             : "Dizisayfasi.aspx?id="+Eval("DiziID") %>'">
        <img src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>">
        <div>
            <h3><%# Eval("Baslik") %></h3>
            <p><%# Eval("Yorum") %></p>
            
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>
<div class="title">Popüler Film Eleştirileri</div>
<asp:Repeater ID="rptPopulerFilmElestiri" runat="server">
<ItemTemplate>
    <div class="review"
         onclick="location.href='Filmsayfasi.aspx?id=<%# Eval("FilmID") %>'">
        <img src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>">
        <div>
            <h3><%# Eval("FilmAdi") %></h3>
            <p><%# Eval("Yorum") %></p>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>

<div class="title">Popüler Dizi Eleştirileri</div>
<asp:Repeater ID="rptPopulerDiziElestiri" runat="server">
<ItemTemplate>
    <div class="review"
         onclick="location.href='Dizisayfasi.aspx?id=<%# Eval("DiziID") %>'">
        <img src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>">
        <div>
            <h3><%# Eval("DiziAdi") %></h3>
            <p><%# Eval("Yorum") %></p>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>



</div>

</asp:Content>
