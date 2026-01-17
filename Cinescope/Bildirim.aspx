<%@ Page Title="Bildirimler" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bildirim.aspx.cs" Inherits="Cinescope.Bildirim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    :root {
        --primary: #ffb3d9; 
        --primary-soft: rgba(255, 179, 217, 0.1);
        --primary-ultra-soft: rgba(255, 179, 217, 0.03);
        --bg-dark: #1f1f1f;
        --bg-card: #262626;
        --border: #333;
        --text: #eee;
        --text-dim: #aaa;
    }

    body { background: var(--bg-dark) !important; font-family: 'Segoe UI', sans-serif; }

    .tab-wrapper {
        width: 95%;
        margin: 40px auto 0;
        border-bottom: 1px solid var(--border);
        display: flex; gap: 40px;
    }

    .tab-item {
        padding-bottom: 12px; font-size: 1.15rem; font-weight: 600;
        color: #777; cursor: pointer; position: relative; transition: .3s;
    }

    .tab-item.active { color: var(--primary); }

    .tab-item.active::after {
        content: ""; position: absolute; bottom: -1px; left: 0;
        width: 100%; height: 2px; background: var(--primary);
    }

    .content-wrapper { width: 95%; margin: 25px auto 100px; }

    .cinescope-row {
        background: var(--bg-card);
        border: 1px solid var(--border);
        border-radius: 12px;
        padding: 16px 20px;
        margin-bottom: 10px;
        display: flex; gap: 16px; align-items: center;
        cursor: pointer;
        transition: all 0.2s ease-in-out;
    }

    .cinescope-row:hover {
        transform: scale(1.008); 
        border-color: #444;
        background: #2d2d2d;
    }

    .okunmadi {
        background: var(--primary-ultra-soft);
        border-color: rgba(255, 179, 217, 0.15);
    }

    .row-visual {
        width: 42px; height: 42px; min-width: 42px;
        background: var(--primary-soft);
        color: var(--primary);
        display: flex; align-items: center; justify-content: center;
        border-radius: 50%; font-size: 16px; overflow: hidden;
    }

    .row-visual img { width: 100%; height: 100%; object-fit: cover; }

    .row-content { flex: 1; color: var(--text-dim); font-size: 0.95rem; line-height: 1.4; }
    .row-content b { color: var(--primary); font-weight: 600; }

    .empty-box { color: #666; text-align: center; padding: 60px 0; }
    .hidden { display: none; }
</style>

<div class="tab-wrapper">
    <div id="tabBildirim" class="tab-item active" onclick="showTab('bildirim')">Bildirimler</div>
    <div id="tabOneri" class="tab-item" onclick="showTab('oneri')">Önerilenler</div>
</div>

<div class="content-wrapper">
    
    <div id="bildirimArea">
        <asp:Repeater ID="rptBildirimler" runat="server">
            <ItemTemplate>
                <div class='cinescope-row <%# Convert.ToInt32(Eval("Okundu")) == 1 ? "" : "okunmadi" %>'
                     onclick="window.location.href='<%# Eval("YonlendirmeUrl") %>'">
                    
                    <div class="row-visual">
                        <i class='fa-solid <%# Eval("IconClass") %>'></i>
                    </div>

                    <div class="row-content">
                        <b><%# Eval("GonderenAdi") %></b> <%# Eval("Mesaj") %>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Panel ID="pnlBosBildirim" runat="server" CssClass="empty-box" Visible="false">
            Henüz bir bildirim yok.
        </asp:Panel>
    </div>

    <div id="oneriArea" class="hidden">
        <asp:Repeater ID="rptOnerilenler" runat="server">
            <ItemTemplate>
                <div class="cinescope-row" onclick="window.location.href='Kullanici.aspx?id=<%# Eval("ID") %>'">
                    
                    <div class="row-visual">
                        <img src='<%# Eval("ProfilFoto") %>' onerror="this.src='Gorsel/pp.png'" />
                    </div>

                    <div class="row-content">
                        <b><%# Eval("KullaniciAdi") %></b> kullanıcısını keşfet<br />
                        <small style="font-size: 0.85rem; opacity: 0.7;"><%# Eval("Neden") %></small>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Panel ID="pnlBosOneri" runat="server" CssClass="empty-box" Visible="false">
            Şu an için yeni bir öneri yok.
        </asp:Panel>
    </div>

</div>

<script>
    function showTab(tab) {
        document.getElementById("bildirimArea").classList.toggle("hidden", tab !== 'bildirim');
        document.getElementById("oneriArea").classList.toggle("hidden", tab !== 'oneri');
        document.getElementById("tabBildirim").classList.toggle("active", tab === 'bildirim');
        document.getElementById("tabOneri").classList.toggle("active", tab === 'oneri');
    }
</script>

</asp:Content>