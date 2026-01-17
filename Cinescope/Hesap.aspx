<%@ Page Title="Hesap" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Hesap.aspx.cs"
    Inherits="Cinescope.Hesap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --p:#ffb3d9;
    --bg:#1f1f1f;
    --card:#262626;
    --b:#3a3a3a;
    --t:#eee;
    --m:#888;
}
body{background:var(--bg)!important;color:var(--t);font-family:'Segoe UI', Tahoma, sans-serif;}

.hesap-profil-card{
    width:95%;margin:40px auto 0;
    background:var(--card);border:1px solid var(--b);
    border-radius:18px;padding:30px;
    display:flex;gap:30px;align-items:center;
}
.hesap-profil-foto{
    width:140px;height:140px;border-radius:50%;
    padding:6px;background:linear-gradient(135deg,var(--p),#ffd9eb);
}
.hesap-profil-foto img{
    width:100%;height:100%;border-radius:50%;
    object-fit:contain;background:#000;padding:6px;
}
.hesap-chip{
    display:inline-block;margin-top:14px;margin-right:12px;
    background:#2a2a2a;border:1px solid var(--b);
    border-radius:20px;padding:6px 14px;
}
.hesap-chip a, .hesap-chip a * {
    color:var(--p) !important;
    text-decoration:none !important;
    font-weight:700;
}

.hesap-btn-group {
    display: flex;
    flex-direction: column;
    gap: 12px;
    align-items: flex-end;
}

.hesap-btn{
    background:var(--p);color:#1f1f1f;
    padding:10px 20px;border-radius:10px;
    border:none;font-weight:800;cursor:pointer;
    text-align: center;
    min-width: 130px;
    text-decoration: none !important;
    transition: transform 0.2s;
}

.hesap-btn-logout {
    background: transparent;
    border: 2px solid var(--p);
    color: var(--p);
}

.hesap-btn:hover {
    transform: translateY(-2px);
    opacity: 0.9;
}

.hesap-hakkimda-wide{
    width:95%;margin:20px auto 0;
    background:var(--card);border:1px solid var(--b);
    border-radius:16px;padding:20px 26px;
    color:var(--m);
}

.istatistik-wrapper{
    width:95%; margin:25px auto;
    display:grid; grid-template-columns:repeat(4,1fr); gap:20px;
}
.istatistik-card{
    background: var(--card); border: 1px solid var(--b);
    border-radius: 20px; padding: 25px 15px; text-align: center;
}
.istatistik-number{
    display: block; font-size: 2.2rem; font-weight: 900;
    color: var(--p); line-height: 1; margin-bottom: 5px;
}
.istatistik-text{
    font-size: 0.75rem; color: var(--m);
    text-transform: uppercase; letter-spacing: 1.5px; font-weight: 600;
}

.liste-grid{
    display:grid;
    grid-template-columns: repeat(auto-fill, minmax(130px, 1fr));
    gap: 25px; padding: 15px 5px;
}
.liste-card-wrapper {
    position: relative;
    display: block;
    text-decoration: none !important;
}
.liste-card-wrapper::before {
    content: ''; position: absolute; top: -6px; left: 8px; right: 8px; bottom: 6px;
    background: #333; border: 1px solid var(--b); border-radius: 14px; z-index: 1;
}
.liste-card{
    position: relative; z-index: 2;
    background: #2a2a2a; border: 1px solid var(--b);
    border-radius: 14px; padding: 20px 10px;
    text-align: center; transition: .3s;
    min-height: 100px; display: flex; flex-direction: column; 
    align-items: center; justify-content: center;
}
.liste-card:hover { transform: translateY(-5px); border-color: var(--p); }
.liste-icon-stack { margin-bottom: 8px; color: var(--p); }
.liste-card .name{ font-weight: 700; font-size: 0.85rem; color: #fff; line-height: 1.2; }

.two-col-layout{
    width:95%;margin:30px auto 80px;
    display:grid;grid-template-columns:1.3fr 1fr;
    gap:35px;
}
.left-col, .right-col {
    display: flex;
    flex-direction: column;
    gap: 40px; 
}
.box{ background:var(--card); border:1px solid var(--b); border-radius:16px; padding:20px; }
.section-title{ margin:0 0 18px; font-size:1.2rem; font-weight:800; display:flex; justify-content:space-between; align-items:center; }
.section-title a{ color:var(--p); font-weight:700; text-decoration:none !important; font-size: 0.9rem; }

.box a { text-decoration: none !important; display: inline-block; }
.mini-poster{
    width: 90px; height: 135px; border-radius: 10px;
    overflow: hidden; margin: 6px;
    border: 1px solid var(--b); transition: .25s;
}
.mini-poster img{ width: 100%; height: 100%; object-fit: cover; }
.mini-poster:hover{ transform: translateY(-6px); border-color: var(--p); }

.elestiri-container { display: flex; flex-direction: column; width: 100%; }
.elestiri-link {
    text-decoration: none !important; color: inherit;
    display: block; width: 100%;
}
.mini-elestiri {
    padding: 18px 0; border-bottom: 1px solid var(--b); transition: .2s;
}
.mini-elestiri:last-child { border-bottom: none; }
.mini-elestiri strong { display: block; color: var(--p); font-size: 1.05rem; margin-bottom: 5px; }
.mini-elestiri p {
    margin: 0; font-size: 0.9rem; color: var(--m); line-height: 1.5;
    display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical; overflow: hidden;
}
.mini-elestiri:hover { padding-left: 8px; }

@media(max-width:1100px){ .two-col-layout{grid-template-columns:1fr;} }
</style>

<div class="hesap-profil-card">
    <div class="hesap-profil-foto">
        <asp:Image ID="imgProfil" runat="server" ImageUrl="Gorsel/pp.png" />
    </div>
    <div style="flex:1; display: flex; justify-content: space-between; align-items: flex-start;">
        <div>
            <h2><asp:Label ID="lblKullaniciAdi" runat="server"></asp:Label></h2>
            <span class="hesap-chip"><a href="Takipci.aspx"><asp:Label ID="lblTakipci" runat="server"></asp:Label> Takipçi</a></span>
            <span class="hesap-chip"><a href="Takipedilen.aspx"><asp:Label ID="lblTakipEdilen" runat="server"></asp:Label> Takip Edilen</a></span>
        </div>
        
        <div class="hesap-btn-group">
            <asp:LinkButton ID="btnAyarlar" runat="server" CssClass="hesap-btn" OnClick="btnAyarlar_Click">Ayarlar</asp:LinkButton>
            <asp:LinkButton ID="btnCikis" runat="server" CssClass="hesap-btn hesap-btn-logout" OnClick="btnCikis_Click">Çıkış Yap</asp:LinkButton>
        </div>
    </div>
</div>

<div class="hesap-hakkimda-wide">
    <asp:Label ID="lblHakkimda" runat="server"></asp:Label>
</div>

<div class="istatistik-wrapper">
    <div class="istatistik-card">
        <asp:Label ID="lblFilmSayisi" runat="server" CssClass="istatistik-number" />
        <div class="istatistik-text">Film İzlendi</div>
    </div>
    <div class="istatistik-card">
        <asp:Label ID="lblDiziSayisi" runat="server" CssClass="istatistik-number" />
        <div class="istatistik-text">Dizi İzlendi</div>
    </div>
    <div class="istatistik-card">
        <asp:Label ID="lblListeSayisi" runat="server" CssClass="istatistik-number" />
        <div class="istatistik-text">Koleksiyon</div>
    </div>
    <div class="istatistik-card">
        <asp:Label ID="lblRozet" runat="server" CssClass="istatistik-number" />
        <div class="istatistik-text">Rozet Skoru</div>
    </div>
</div>

<div class="two-col-layout">
    <div class="left-col">
        <div class="box">
            <h3 class="section-title"><span>İzlediğim Filmler</span><a href="Tumfilmler.aspx">Tüm Filmler</a></h3>
            <asp:Repeater ID="rptIzlenenFilmlerMini" runat="server">
                <ItemTemplate>
                    <a href='Filmsayfasi.aspx?id=<%# Eval("FilmID") %>'>
                        <div class="mini-poster"><img src="https://image.tmdb.org/t/p/w300<%# Eval("PosterPath") %>" /></div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="box">
            <h3 class="section-title"><span>İzlediğim Diziler</span><a href="Tumdiziler.aspx">Tüm Diziler</a></h3>
            <asp:Repeater ID="rptIzlenenDizilerMini" runat="server">
                <ItemTemplate>
                    <a href='Dizisayfasi.aspx?id=<%# Eval("DiziID") %>'>
                        <div class="mini-poster"><img src="https://image.tmdb.org/t/p/w300<%# Eval("PosterPath") %>" /></div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="box">
            <h3 class="section-title"><span>Listelerim</span><a href="Listeler.aspx">Tüm Listeler</a></h3>
            <div class="liste-grid">
                <asp:Repeater ID="rptListelerMini" runat="server">
                    <ItemTemplate>
                        <a href='Listesayfasi.aspx?lid=<%# Eval("ID") %>' class="liste-card-wrapper">
                            <div class="liste-card">
                                <div class="liste-icon-stack">
                                    <svg fill="currentColor" width="18" height="18" viewBox="0 0 16 16"><path d="M12 13c0 1.105-1.12 2-2.5 2S7 14.105 7 13s1.12-2 2.5-2 2.5.895 2.5 2z"/><path fill-rule="evenodd" d="M12 3v10h-1V3h1z"/><path d="M11 2.82a1 1 0 0 1 .804-.98l3-.6A1 1 0 0 1 16 2.22V4l-5 1V2.82z"/><path fill-rule="evenodd" d="M0 11.5a.5.5 0 0 1 .5-.5H4a.5.5 0 0 1 0 1H.5a.5.5 0 0 1-.5-.5zm0-4A.5.5 0 0 1 .5-.5H4a.5.5 0 0 1 0 1H.5a.5.5 0 0 1-.5-.5zm0-4A.5.5 0 0 1 .5-.5H4a.5.5 0 0 1 0 1H.5a.5.5 0 0 1-.5-.5z"/></svg>
                                </div>
                                <div class="name"><%# Eval("ListeAdi") %></div>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <div class="right-col">
        <div class="box">
            <h3 class="section-title"><span>Son Film Eleştirilerim</span></h3>
            <div class="elestiri-container">
                <asp:Repeater ID="rptSonFilmElestiriler" runat="server">
                    <ItemTemplate>
                        <a href='Filmsayfasi.aspx?id=<%# Eval("FilmID") %>&elestiri=<%# Eval("ID") %>' class="elestiri-link">
                            <div class="mini-elestiri">
                                <strong><%# Eval("FilmAdi") %></strong>
                                <p><%# Eval("Yorum") %></p>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="box">
            <h3 class="section-title"><span>Son Dizi Eleştirilerim</span></h3>
            <div class="elestiri-container">
                <asp:Repeater ID="rptSonDiziElestiriler" runat="server">
                    <ItemTemplate>
                        <a href='Dizisayfasi.aspx?id=<%# Eval("DiziID") %>&elestiri=<%# Eval("ID") %>' class="elestiri-link">
                            <div class="mini-elestiri">
                                <strong><%# Eval("DiziAdi") %></strong>
                                <p><%# Eval("Yorum") %></p>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</div>

</asp:Content>