<%@ Page Title="Mesajlar" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Mesajlar.aspx.cs"
    Inherits="Cinescope.Mesajlar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --primary:#ffb3d9;
    --bg:#111;
    --card:#1b1b1b;
    --soft:#161616;
    --border:#333;
    --text:#eee;
}

.page{
    display:flex;
    height:calc(100vh - 100px);
    background:#0f0f0f;
}

.left{
    width:30%;
    background:#181818;
    border-right:1px solid var(--border);
    display:flex;
    flex-direction:column;
}

.tabs{
    display:flex;
}
.tab{
    flex:1;
    padding:14px;
    text-align:center;
    font-weight:700;
    color:#aaa;
    cursor:pointer;
}
.tab.active{
    color:var(--primary);
    border-bottom:3px solid var(--primary);
}

.list{
    flex:1;
    overflow-y:auto;
}
.user{
    display:flex;
    gap:12px;
    padding:14px 18px;
    cursor:pointer;
    border-bottom:1px solid #222;
    align-items:center;
}
.user:hover{background:#ffffff10;}
.user.active{background:#ffffff15;}

.user img{
    width:46px;
    height:46px;
    border-radius:50%;
    object-fit:cover;
}

.right{
    flex:1;
    display:flex;
}

.chat{
    flex:1;
    display:flex;
    flex-direction:column;
}

.chat-header{
    display:flex;
    align-items:center;
    gap:14px;
    padding:16px 20px;
    background:var(--soft);
    border-bottom:1px solid var(--border);
}
.chat-header img{
    width:50px;
    height:50px;
    border-radius:50%;
    border:2px solid var(--primary);
}
.chat-header span{
    font-size:18px;
    font-weight:800;
}

.chat-body{
    flex:1;
    padding:22px;
    background:var(--card);
    overflow-y:auto;
    display:flex;
    flex-direction:column;
    gap:14px;
}

.me{
    align-self:flex-end;
    background:var(--primary);
    color:#000;
    padding:12px 16px;
    border-radius:18px 18px 4px 18px;
    max-width:65%;
}
.you{
    align-self:flex-start;
    background:#222;
    color:#fff;
    padding:12px 16px;
    border-radius:18px 18px 18px 4px;
    max-width:65%;
}

.chat-input{
    padding:14px;
    background:var(--soft);
    border-top:1px solid var(--border);
    display:flex;
    gap:12px;
}

.chat-input input{
    flex:1;
    background:#0f0f0f;
    border:1px solid var(--border);
    color:#fff;
    border-radius:12px;
    padding:12px 16px;
    font-size:14px;
}

.chat-input button{
    background:var(--primary);
    border:none;
    border-radius:12px;
    padding:0 28px;
    font-weight:800;
}

.req-actions{
    display:flex;
    gap:12px;
    padding:14px;
}
.ok{background:#4caf50!important;}
.no{background:#f44336!important;}

.req-actions{
    display:flex;
    gap:12px;
    padding:14px;
    background:var(--soft);
    border-top:1px solid var(--border);
}

.req-actions .btn{
    flex:1;
    height:44px;
    border:none;
    border-radius:12px;
    font-weight:800;
    font-size:14px;
    cursor:pointer;
    transition:.2s;
}

.req-actions .btn-accept{
    background:var(--primary);
    color:#000;
}

.req-actions .btn-reject{
    background:rgba(255,179,217,.25);
    color:var(--primary);
    border:1px solid var(--primary);
}

.req-actions .btn:hover{
    opacity:.9;
    transform:translateY(-1px);
}

</style>

<div class="page">

<div class="left">

    <div class="tabs">
        <div class="tab active" id="tMsg" onclick="showTab('msg')">Mesajlar</div>
        <div class="tab" id="tReq" onclick="showTab('req')">
            İstekler (<asp:Label ID="lblIstekSayisi" runat="server" Text="0" />)
        </div>
    </div>

    <div class="list" id="msgList">
        <asp:Repeater ID="rptKonusmalar" runat="server">
            <ItemTemplate>
                <div class="user <%# Eval("ID").ToString()==Request.QueryString["uid"]?"active":"" %>"
                     onclick="location='Mesajlar.aspx?uid=<%# Eval("ID") %>'">
                    <img src="<%# Eval("ProfilFoto") %>" />
                    <div><%# Eval("KullaniciAdi") %></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="list" id="reqList" style="display:none">
        <asp:Repeater ID="rptIstekler" runat="server">
            <ItemTemplate>
                <div class="user"
                     onclick="location='Mesajlar.aspx?uid=<%# Eval("GonderenID") %>'">
                    <img src="<%# Eval("ProfilFoto") %>" />
                    <div><%# Eval("KullaniciAdi") %></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</div>

<div class="right">

<asp:Panel ID="pnlBos" runat="server" />

<asp:Panel ID="pnlSohbet" runat="server" CssClass="chat" Visible="false">

    <div class="chat-header">
    <asp:Image ID="imgHedef" runat="server" />
    <asp:Label ID="lblSohbetBaslik" runat="server" />
</div>


    <div class="chat-body">
        <asp:Repeater ID="rptMesajlar" runat="server">
            <ItemTemplate>
                <div class="<%# Convert.ToInt32(Eval("GonderenID"))==
                    Convert.ToInt32(Session["KullaniciID"]) ? "me":"you" %>">
                    <%# Eval("Mesaj") %>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="chat-input">
        <asp:TextBox ID="txtMesaj" runat="server" placeholder="Mesaj yaz..." />
        <asp:Button ID="btnGonder" runat="server" Text="Gönder"
            OnClick="btnGonder_Click" />
    </div>

    <asp:Panel ID="pnlIstekOnay" runat="server" Visible="false">
    <div class="req-actions">
        <asp:Button ID="btnIstekKabul" runat="server"
            Text="Kabul Et"
            CssClass="btn btn-accept"
            OnClick="btnIstekKabul_Click" />

        <asp:Button ID="btnIstekRed" runat="server"
            Text="Reddet"
            CssClass="btn btn-reject"
            OnClick="btnIstekRed_Click" />
    </div>
</asp:Panel>


</asp:Panel>

</div>
</div>

<script>
    function showTab(t) {
        msgList.style.display = t === "msg" ? "block" : "none";
        reqList.style.display = t === "req" ? "block" : "none";
        tMsg.classList.toggle("active", t === "msg");
        tReq.classList.toggle("active", t === "req");
    }
</script>

</asp:Content>
