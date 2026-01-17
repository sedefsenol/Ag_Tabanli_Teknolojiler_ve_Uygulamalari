<%@ Page Title="Anasayfa" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Anasayfa.aspx.cs"
    Inherits="Cinescope.Anasayfa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --p:#ffb3d9;--bg:#1f1f1f;--card:#262626;--b:#3a3a3a;--t:#eee;--tl:#ccc;
}
body{background:var(--bg)!important;color:var(--t);}

.title{
    font-size:1.7rem;font-weight:800;color:var(--p);
    border-left:6px solid var(--p);
    padding-left:12px;margin:40px 0 20px;
}

.grid{
    display:grid;
    grid-template-columns:repeat(auto-fill,minmax(160px,1fr));
    gap:15px;
}

.card{
    height:240px;border:1px solid var(--b);
    border-radius:10px;overflow:hidden;cursor:pointer;
}
.card img{width:100%;height:100%;object-fit:cover;}

.review{
    display:flex;gap:12px;background:var(--card);
    padding:14px;border-radius:12px;border-left:4px solid var(--p);
    margin-bottom:14px;cursor:pointer;
}
.review img{width:80px;height:120px;border-radius:8px;object-fit:cover;}
.review h3{color:var(--p);margin:0 0 4px;font-size:1.05rem;}
.review p{color:var(--tl);font-size:.9rem;margin:0;}

.user-grid{
    display:grid;
    grid-template-columns:repeat(auto-fill,minmax(220px,1fr));
    gap:18px;
}
.user-card{
    background:var(--card);
    border:1px solid var(--b);
    border-radius:14px;
    padding:16px;
    display:flex;
    gap:12px;
    align-items:center;
    cursor:pointer;
}
.user-card img{
    width:60px;height:60px;border-radius:50%;
    object-fit:cover;
}
.user-card h4{margin:0;color:var(--p);}
.user-card span{font-size:.85rem;color:var(--tl);}
</style>

<div class="container mt-4 mb-5">


<div class="title">Popüler Filmler</div>
<div id="popular-films" class="grid"></div>

<div class="title">Popüler Dizi Eleştirileri</div>
<asp:Repeater ID="rptDiziElestiri" runat="server">
<ItemTemplate>
    <div class="review"
         onclick="location.href='Dizisayfasi.aspx?id=<%# Eval("DiziID") %>'">
        <img src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>" />
        <div>
            <h3><%# Eval("DiziAdi") %></h3>
            <p><%# Eval("Yorum") %></p>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>

<div class="title">Popüler Kullanıcılar</div>
<div class="user-grid">
<asp:Repeater ID="rptPopulerKullanicilar" runat="server">
<ItemTemplate>
    <div class="user-card"
         onclick="location.href='<%# GetUserProfileUrl(Eval("ID")) %>'">
        <img src="<%# Eval("ProfilFoto") %>" />
        <div>
            <h4><%# Eval("KullaniciAdi") %></h4>
            <span><%# Eval("TakipciSayisi") %> takipçi</span>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>
</div>

<div class="title">Popüler Diziler</div>
<div id="popular-series" class="grid"></div>

<div class="title">Popüler Film Eleştirileri</div>
<asp:Repeater ID="rptFilmElestiri" runat="server">
<ItemTemplate>
    <div class="review"
         onclick="location.href='Filmsayfasi.aspx?id=<%# Eval("FilmID") %>'">
        <img src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>" />
        <div>
            <h3><%# Eval("FilmAdi") %></h3>
            <p><%# Eval("Yorum") %></p>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>

<div class="title">Önerilen Kullanıcılar</div>
<div class="user-grid">
<asp:Repeater ID="rptOnerilenler" runat="server">
<ItemTemplate>
    <div class="user-card"
         onclick="location.href='Kullanici.aspx?id=<%# Eval("ID") %>'">
        <img src="<%# Eval("ProfilFoto") %>" />
        <div>
            <h4><%# Eval("KullaniciAdi") %></h4>
            <span><%# Eval("Neden") %></span>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>
</div>

<div class="title">Son Çıkan Filmler</div>
<div id="latest-films" class="grid"></div>

</div>

<script>
    const API = "02f7a3e84e6acce9ce0b5583deffb717";
    const IMG = "https://image.tmdb.org/t/p/w500";

    function load(url, id, isFilm = true) {
        fetch(url).then(r => r.json()).then(d => {
            const c = document.getElementById(id); c.innerHTML = "";
            d.results.slice(0, 7).forEach(x => {
                if (!x.poster_path) return;
                c.innerHTML += `
            <div class="card"
             onclick="location.href='${isFilm ? "Filmsayfasi" : "Dizisayfasi"}.aspx?id=${x.id}'">
                <img src="${IMG}${x.poster_path}">
            </div>`;
            });
        });
    }

    load(`https://api.themoviedb.org/3/movie/popular?api_key=${API}&language=tr-TR`,
        "popular-films", true);

    load(`https://api.themoviedb.org/3/tv/popular?api_key=${API}&language=tr-TR`,
        "popular-series", false);

    load(`https://api.themoviedb.org/3/movie/now_playing?api_key=${API}&language=tr-TR`,
        "latest-films", true);
</script>

</asp:Content>
