<%@ Page Language="C#"
    AutoEventWireup="true"
    MasterPageFile="~/Site.Master"
    CodeBehind="OrtakListeDuzenle.aspx.cs"
    Inherits="Cinescope.OrtakListeDuzenle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
.wrapper{
    width:80%;
    margin:40px auto;
    background:#262626;
    padding:30px;
    border-radius:14px;
    color:#eee;
}
.header h2{
    margin:0;
    color:#ffb3d9;
}
.search{
    width:100%;
    padding:10px;
    margin-top:20px;
    background:#1b1b1b;
    border:1px solid #444;
    border-radius:8px;
    color:#fff;
}
.result-row,.film-row{
    display:flex;
    flex-wrap:wrap;
    gap:12px;
    margin-top:15px;
}
.film-poster{
    width:80px;
    height:120px;
    border-radius:8px;
    border:1px solid #444;
    cursor:pointer;
}
.film-poster:hover{
    outline:2px solid #ffb3d9;
}
.btn{
    margin-top:30px;
    background:#ffb3d9;
    border:none;
    padding:12px 24px;
    border-radius:10px;
    font-weight:800;
    cursor:pointer;
    color:#000;
}
.note{
    margin-top:10px;
    color:#aaa;
    font-size:0.9rem;
}
</style>

<div class="wrapper">

    <div class="header">
        <h2><asp:Label ID="lblListeAdi" runat="server" /></h2>
        <p><asp:Label ID="lblAciklama" runat="server" /></p>
    </div>

    <input id="searchInput" class="search" placeholder="Film ara..." />

    

    <h3 style="margin-top:25px;">Arama Sonuçları</h3>
    <div id="searchResults" class="result-row"></div>

    <h3 style="margin-top:25px;">Eklenen Filmler</h3>
    <div id="filmContainer" class="film-row"></div>

    <asp:HiddenField ID="hfItems" runat="server" ClientIDMode="Static"/>

    <asp:Button ID="btnKaydet"
        runat="server"
        Text="Kaydet"
        CssClass="btn"
        OnClick="btnKaydet_Click" />

</div>

<script>
    const API_KEY = "02f7a3e84e6acce9ce0b5583deffb717";
    const IMG = "https://image.tmdb.org/t/p/w200";
    let items = [];

    function loadItems(json) {
        try {
            items = JSON.parse(json || "[]");
        } catch {
            items = [];
        }
        render();
    }

    function render() {
        const c = document.getElementById("filmContainer");
        c.innerHTML = "";

        items.forEach((x, i) => {
            const img = document.createElement("img");
            img.src = IMG + x.PosterPath;
            img.className = "film-poster";
            img.title = "Silmek için tıkla";

            img.onclick = () => {
                if (!confirm("Bu filmi listeden silmek istiyor musun?")) return;
                items.splice(i, 1);
                sync();
                render();
            };

            c.appendChild(img);
        });

        sync();
    }

    function sync() {
        hfItems.value = JSON.stringify(items);
    }

    document.getElementById("searchInput").oninput = async e => {
        const q = e.target.value.trim();
        const c = document.getElementById("searchResults");
        c.innerHTML = "";

        if (q.length < 3) return;

        const r = await fetch(
            `https://api.themoviedb.org/3/search/movie?api_key=${API_KEY}&language=tr-TR&query=${encodeURIComponent(q)}`
        );
        const d = await r.json();

        d.results.slice(0, 8).forEach(x => {
            if (!x.poster_path) return;

            const img = document.createElement("img");
            img.src = IMG + x.poster_path;
            img.className = "film-poster";
            img.title = "Listeye ekle";

            img.onclick = () => {
                if (items.some(i => i.IcerikID === x.id)) return;

                items.push({
                    IcerikID: x.id,
                    PosterPath: x.poster_path
                });

                c.innerHTML = "";
                render();
            };

            c.appendChild(img);
        });
    };
</script>

</asp:Content>
