<%@ Page Title="Listeler" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Listeler.aspx.cs" Inherits="Cinescope.Listeler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    :root { 
        --primary:#ffb3d9; 
        --primary-soft:rgba(255, 179, 217, 0.1); 
        --bg-dark:#1f1f1f; 
        --bg-card:#262626; 
        --border:#3a3a3a; 
        --text:#eee;
        --text-muted: #888;
    }

    body { background: var(--bg-dark) !important; }

    .tab-wrapper {
        width: 95%;
        margin: 40px auto 0 auto;
        border-bottom: 2px solid var(--border); 
        display: flex;
        gap: 45px;
    }

    .tab-item {
        font-size: 1.2rem;
        font-weight: 600;
        color: var(--text-muted);
        padding-bottom: 12px;
        cursor: pointer;
        position: relative;
        transition: all .3s ease;
        text-decoration: none; 
    }

    .tab-item:hover { color: var(--text); }

    .tab-item.active {
        color: var(--primary);
    }

    .tab-item.active::after {
        content: "";
        position: absolute;
        bottom: -2px; 
        left: 0;
        width: 100%;
        height: 3px;
        background: var(--primary);
        border-radius: 10px 10px 0 0;
    }

    .content-wrapper {
        width: 95%;
        margin: 35px auto 120px auto;
    }

    .liste-container {
        background: var(--bg-card);
        padding: 25px;
        border-radius: 16px;
        border: 1px solid var(--border);
        margin-bottom: 25px;
        width: 100%; 
        transition: .3s;
    }

    .liste-container:hover {
        border-color: #444;
        transform: translateY(-2px);
    }

    .liste-header{
        display:flex;
        justify-content:space-between;
        align-items:center;
        margin-bottom:20px;
    }

    .liste-adi {
        color: var(--primary);
        font-size: 1.6rem;
        font-weight: 800;
    }

    .tumunu-gor{
        color: var(--text-muted);
        font-size: .9rem;
        text-decoration: none;
        padding: 6px 12px;
        border: 1px solid var(--border);
        border-radius: 20px;
        transition: .2s;
    }

    .tumunu-gor:hover{
        color: var(--primary);
        border-color: var(--primary);
        background: var(--primary-soft);
    }

    .film-row { 
        display: flex; 
        flex-wrap: wrap; 
        gap: 12px; 
    }

    .film-poster {
        width: 75px; 
        height: 110px; 
        object-fit: cover;
        border-radius: 8px; 
        border: 1px solid var(--border); 
        transition: .2s;
    }

    .film-poster:hover{
        transform: scale(1.1) translateY(-5px);
        border-color: var(--primary);
        box-shadow: 0 5px 15px rgba(0,0,0,0.4);
        z-index: 10;
    }
</style>

<div class="tab-wrapper">
    <a href="Listeler.aspx" class="tab-item active">Listelerim</a>
    <a href="Favoriler.aspx" class="tab-item">Favoriler</a>
    <a href="Dahasonra.aspx" class="tab-item">Daha Sonra İzle</a>
    <a href="Listeolustur.aspx" class="tab-item">Liste Oluştur</a>
</div>

<div class="content-wrapper">
    <asp:Repeater ID="rptListeler" runat="server">
        <ItemTemplate>
            <div class="liste-container">
                <div class="liste-header">
                    <div class="liste-adi">
                        <%# Eval("ListeAdi") %>
                    </div>
                    <a class="tumunu-gor" href="Listesayfasi.aspx?lid=<%# Eval("ListeID") %>">

                        Tümünü Gör <i class="fa-solid fa-arrow-right" style="margin-left:5px; font-size:12px;"></i>
                    </a>
                </div>

                <div class="film-row">
                    <%# GetFilmPosters(Eval("FilmlerJson").ToString()) %>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

</asp:Content>