<%@ Page Title="Liste" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Listesayfasi.aspx.cs"
    Inherits="Cinescope.Listesayfasi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{
    --primary:#ffb3d9;
    --bg-dark:#1f1f1f;
    --bg-card:#262626;
    --border:#3a3a3a;
    --text:#eee;
}
body{background:var(--bg-dark)!important;color:var(--text);}

.liste-wrapper{
    width:80%;
    margin:40px auto 120px auto;
    background:var(--bg-card);
    padding:30px;
    border-radius:14px;
    border:1px solid var(--border);
}

.liste-header{
    display:flex;
    justify-content:space-between;
    align-items:center;
    gap:12px;
}

.liste-adi{
    font-size:1.8rem;
    font-weight:800;
    color:var(--primary);
}

.btn{
    background:var(--primary);
    border:none;
    padding:8px 18px;
    border-radius:8px;
    font-weight:800;
    cursor:pointer;
    color:#000;
}

.form-label{
    margin-top:15px;
    margin-bottom:6px;
    font-weight:600;
    color:#ccc;
}
.form-control{
    width:100%;
    background:#1b1b1b;
    border:1px solid var(--border);
    border-radius:8px;
    padding:10px;
    color:#fff;
}

.film-row{
    display:flex;
    flex-wrap:wrap;
    gap:12px;
    margin-top:15px;
}
.film-poster{
    width:80px;
    height:120px;
    border-radius:8px;
    object-fit:cover;
    cursor:pointer;
    border:1px solid #444;
}
.film-poster:hover{
    outline:2px solid var(--primary);
}

.small-note{ color:#aaa; font-size:0.9rem; margin-top:8px; }
</style>

<div class="liste-wrapper">

    
    <div class="liste-header">
        <div class="liste-adi">
            <asp:Label ID="lblListeAdi" runat="server" />
        </div>

        <asp:Button ID="btnEdit" runat="server"
            Text="Düzenle"
            CssClass="btn"
            OnClick="btnEdit_Click" />
    </div>

    <p style="margin-top:10px; color:#ddd;">
        <asp:Label ID="lblAciklama" runat="server" />
    </p>

   
    <div class="film-row" id="viewFilmContainer" runat="server">
    <asp:Literal ID="ltPosters" runat="server" />
</div>



    <asp:Panel ID="pnlEdit" runat="server" Visible="false">

        <hr style="margin:30px 0;border-color:#333"/>

        <h3 style="color:var(--primary); margin-top:0;">Listeyi Düzenle</h3>

        <asp:Label CssClass="form-label" runat="server">Liste Adı</asp:Label>
        <asp:TextBox ID="txtListeAdi" runat="server" CssClass="form-control" />

        <asp:Label CssClass="form-label" runat="server">Açıklama</asp:Label>
        <asp:TextBox ID="txtAciklama" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />

        <asp:HiddenField ID="hfFilmlerJson" runat="server" ClientIDMode="Static" />

        <div class="form-label">Filmler (silmek için postere tıkla)</div>
        <div id="editFilmContainer" class="film-row"></div>

        <asp:Button ID="btnSave" runat="server"
            Text="Kaydet"
            CssClass="btn"
            OnClick="btnSave_Click"
            Style="margin-top:20px" />
    </asp:Panel>

</div>

<script>
    let films = [];

    function loadFilms(json) {
        try {
            films = JSON.parse(json || "[]");
        } catch (e) {
            films = [];
        }
        renderEditFilms();
    }

    function renderEditFilms() {
        const container = document.getElementById("editFilmContainer");
        if (!container) return;

        container.innerHTML = "";

        films.forEach((f, index) => {
            const img = document.createElement("img");
            img.className = "film-poster";
            img.src = "https://image.tmdb.org/t/p/w200" + (f.poster || f.PosterPath || "");
            img.title = f.title || f.FilmAdi || "";

            img.onclick = () => {
                if (!confirm("Bu filmi listeden silmek istiyor musun?")) return;

                films.splice(index, 1);

                document.getElementById("hfFilmlerJson").value =
                    JSON.stringify(films);

                renderEditFilms();
            };

            container.appendChild(img);
        });
    }

</script>

</asp:Content>