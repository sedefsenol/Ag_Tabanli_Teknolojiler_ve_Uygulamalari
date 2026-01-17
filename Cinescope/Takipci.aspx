<%@ Page Title="Takipçiler" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Takipciler.aspx.cs"
    Inherits="Cinescope.Takipciler" %>

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

/* SOLA YASLI */
.takipci-wrapper{
    width:95%;
    max-width:1400px;
    margin:35px 0 90px 40px;
}

.takipci-title{
    font-size:1.5rem;
    font-weight:800;
    color:var(--primary);
    border-left:6px solid var(--primary);
    padding-left:14px;
    margin-bottom:25px;
}

/* ITEM */
.takipci-item{
    display:flex;
    align-items:center;
    gap:16px;
    background:var(--card);
    border:1px solid var(--border);
    border-radius:14px;
    padding:14px 18px;
    margin-bottom:14px;
    transition:.2s;
    max-width:700px;
    cursor:pointer;
}

.takipci-item:hover{
    border-color:var(--primary);
    box-shadow:0 6px 14px rgba(0,0,0,.45);
}

.takipci-avatar{
    width:50px;
    height:50px;
    border-radius:50%;
    object-fit:cover;
    border:2px solid var(--primary);
}

.takipci-info{ flex:1; }

.takipci-username{
    font-size:.95rem;
    font-weight:700;
    color:var(--primary);
}

.takipci-sub{
    font-size:.78rem;
    color:var(--muted);
}

.takipci-btn{
    background:transparent;
    border:1px solid var(--primary);
    color:var(--primary);
    padding:6px 14px;
    border-radius:20px;
    font-size:.75rem;
    font-weight:700;
    cursor:pointer;
    transition:.2s;
}

.takipci-btn:hover{
    background:var(--primary);
    color:#000;
}

.empty-box{
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

<div class="takipci-wrapper">

    <div class="takipci-title">
        Takipçiler
    </div>

    <asp:Repeater ID="rptTakipciler" runat="server">
        <ItemTemplate>
            <div class="takipci-item"
                 onclick="location.href='Kullanici.aspx?id=<%# Eval("ID") %>'">

                <img class="takipci-avatar"
                     src='<%# Eval("ProfilFoto") %>'
                     onerror="this.src='Gorsel/pp.png';" />

                <div class="takipci-info">
                    <div class="takipci-username">
                        <%# Eval("KullaniciAdi") %>
                    </div>
                    <div class="takipci-sub">
                        CineScope kullanıcısı
                    </div>
                </div>

                <button type="button" class="takipci-btn"
                        onclick="event.stopPropagation();
                                 location.href='Kullanici.aspx?id=<%# Eval("ID") %>'">
                    Profili Gör
                </button>

            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlBos" runat="server" Visible="false" CssClass="empty-box">
        <span>👀</span>
        Henüz takipçin yok
    </asp:Panel>

</div>

</asp:Content>
