<%@ Page Title="Tüm Filmlerim" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="TumFilmler.aspx.cs"
    Inherits="Cinescope.TumFilmler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --primary:#ffb3d9;
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

/* SAYFA */
.tumfilmler-wrapper{
    width:94%;
    max-width:1400px;
    margin:35px auto 90px;
}

.tumfilmler-title{
    font-size:1.6rem;
    font-weight:800;
    color:var(--primary);
    border-left:6px solid var(--primary);
    padding-left:14px;
    margin-bottom:25px;
}

/* GRID */
.film-grid{
    display:grid;
    grid-template-columns:repeat(auto-fill, minmax(170px,1fr));
    gap:22px;
}

/* CARD */
.film-card{
    background:var(--card);
    border:1px solid var(--border);
    border-radius:14px;
    padding:10px;
    cursor:pointer;
    transition:all .25s ease;
}

.film-card:hover{
    border-color:var(--primary);
    transform:translateY(-4px);
    box-shadow:0 8px 18px rgba(0,0,0,.45);
}

/* POSTER */
.film-poster{
    width:100%;
    height:250px;
    object-fit:cover;
    border-radius:10px;
    margin-bottom:8px;
}

/* INFO */
.film-name{
    font-size:.95rem;
    font-weight:700;
    color:var(--primary);
    line-height:1.25;
    margin-bottom:4px;
}

.film-date{
    font-size:.78rem;
    color:var(--muted);
}

/* BOŞ DURUM */
.empty-box{
    text-align:center;
    margin-top:70px;
    color:var(--muted);
    font-size:.95rem;
}
.empty-box span{
    font-size:3rem;
    display:block;
    margin-bottom:10px;
}
</style>

<div class="tumfilmler-wrapper">

    <div class="tumfilmler-title">
        İzlediğim Tüm Filmler
    </div>

    <asp:Repeater ID="rptTumFilmler" runat="server">
        <HeaderTemplate>
            <div class="film-grid">
        </HeaderTemplate>

        <ItemTemplate>
            <div class="film-card"
                 onclick="location.href='Filmsayfasi.aspx?id=<%# Eval("FilmID") %>'">

                <img class="film-poster"
                     src="https://image.tmdb.org/t/p/w500<%# Eval("PosterPath") %>"
                     onerror="this.src='https://via.placeholder.com/300x450';" />

                <div class="film-name">
                    <%# Eval("FilmAdi") %>
                </div>

                

            </div>
        </ItemTemplate>

        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlBos" runat="server" Visible="false" CssClass="empty-box">
        <span>🍿</span>
        Henüz izlediğin film yok
    </asp:Panel>

</div>

</asp:Content>
