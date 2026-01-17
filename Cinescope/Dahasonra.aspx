<%@ Page Title="Daha Sonra İzle" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="DahaSonra.aspx.cs"
    Inherits="Cinescope.DahaSonra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --p:#ffb3d9;--bg:#1f1f1f;--card:#262626;--b:#3a3a3a;--t:#eee;--m:#888;
}
body{background:var(--bg)!important;color:var(--t);}

.tab-wrapper{
    width:95%;margin:40px auto 0;
    border-bottom:2px solid var(--b);
    display:flex;gap:45px;
}
.tab-item{
    font-size:1.2rem;font-weight:600;
    color:var(--m);padding-bottom:12px;
    text-decoration:none;position:relative;
}
.tab-item.active{color:var(--p);}
.tab-item.active::after{
    content:"";position:absolute;left:0;bottom:-2px;
    width:100%;height:3px;background:var(--p);
}

.sub-header{
    width:95%;
    margin:25px auto 15px;
    display:flex;
    justify-content:space-between;
    align-items:center;
}
.sub-tabs{display:flex;gap:30px;}
.sub-tab{
    font-size:1.1rem;font-weight:700;
    color:var(--m);cursor:pointer;
}
.sub-tab.active{color:var(--p);}

.edit-btn{
    color:var(--p);
    font-weight:800;
    cursor:pointer;
}

.content-wrapper{width:95%;margin:20px auto 120px;}
.favori-row{display:flex;flex-wrap:wrap;gap:16px;}

.favori-item{
    width:105px;
    height:150px;
    border-radius:10px;
    overflow:hidden;
    border:1px solid var(--b);
    position:relative;
    cursor:pointer;
    transition:.25s;
}
.favori-item img{
    width:100%;
    height:100%;
    object-fit:cover;
}
.favori-item:hover{
    transform:translateY(-6px) scale(1.05);
    border-color:var(--p);
}

.favori-item::after{
    content:"Kaldır";
    position:absolute;
    inset:0;
    background:rgba(255,179,217,.9);
    color:#1f1f1f;
    font-weight:900;
    font-size:1rem;

    display:flex;
    align-items:center;
    justify-content:center;

    opacity:0;
    transition:.25s;
    pointer-events:none;
}
.edit-active .favori-item::after{
    opacity:1;
    pointer-events:auto;
}
</style>

<div class="tab-wrapper">
    <a href="Listeler.aspx" class="tab-item">Listelerim</a>
    <a href="Favoriler.aspx" class="tab-item">Favoriler</a>
    <a href="DahaSonra.aspx" class="tab-item active">Daha Sonra İzle</a>
    <a href="ListeOlustur.aspx" class="tab-item">Liste Oluştur</a>
</div>

<div class="sub-header">
    <div class="sub-tabs">
        <div id="tabFilm" class="sub-tab active" onclick="showFilm()">Film</div>
        <div id="tabDizi" class="sub-tab" onclick="showDizi()">Dizi</div>
    </div>
    <div class="edit-btn" onclick="toggleEdit()">Düzenle</div>
</div>

<div class="content-wrapper" id="laterWrapper">

    <div id="filmArea" class="favori-row">
        <asp:Repeater ID="rptDahaSonraFilmler" runat="server"
            OnItemCommand="rptDahaSonraFilmler_ItemCommand">
            <ItemTemplate>
                <div class="favori-item"
                     onclick="handleClick(this,'film',<%# Eval("FilmID") %>)">
                    <img src="https://image.tmdb.org/t/p/w500<%# Eval("PosterPath") %>" />
                    <asp:LinkButton runat="server"
                        CommandName="Sil"
                        CommandArgument='<%# Eval("ID") %>'
                        Style="display:none" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div id="diziArea" class="favori-row" style="display:none;">
        <asp:Repeater ID="rptDahaSonraDiziler" runat="server"
            OnItemCommand="rptDahaSonraDiziler_ItemCommand">
            <ItemTemplate>
                <div class="favori-item"
                     onclick="handleClick(this,'dizi',<%# Eval("DiziID") %>)">
                    <img src="https://image.tmdb.org/t/p/w500<%# Eval("PosterPath") %>" />
                    <asp:LinkButton runat="server"
                        CommandName="Sil"
                        CommandArgument='<%# Eval("ID") %>'
                        Style="display:none" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</div>

<script>
    let editMode = false;

    function toggleEdit() {
        editMode = !editMode;
        document.getElementById("laterWrapper")
            .classList.toggle("edit-active", editMode);
    }

    function showFilm() {
        filmArea.style.display = "flex";
        diziArea.style.display = "none";
        tabFilm.classList.add("active");
        tabDizi.classList.remove("active");
    }

    function showDizi() {
        filmArea.style.display = "none";
        diziArea.style.display = "flex";
        tabDizi.classList.add("active");
        tabFilm.classList.remove("active");
    }

    function handleClick(card, type, id) {
        if (!editMode) {
            location.href = (type === "film"
                ? "Filmsayfasi.aspx?id=" + id
                : "Dizisayfasi.aspx?id=" + id);
            return;
        }
        const btn = card.querySelector("a,button");
        if (btn) btn.click();
    }
</script>

</asp:Content>
