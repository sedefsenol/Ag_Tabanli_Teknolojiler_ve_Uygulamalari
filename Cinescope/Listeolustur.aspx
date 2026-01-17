<%@ Page Title="Liste Oluştur" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Listeolustur.aspx.cs"
    Inherits="Cinescope.Listeolustur" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
:root{ 
    --primary:#ffb3d9; 
    --primary-soft:rgba(255,179,217,.1); 
    --bg-dark:#1f1f1f; 
    --bg-card:#262626; 
    --border:#3a3a3a; 
    --text:#eee;
    --text-muted:#888;
}
body{background:var(--bg-dark)!important;color:var(--text);}

.tab-wrapper{
    width:95%;margin:40px auto 0;
    border-bottom:2px solid var(--border);
    display:flex;gap:45px;
}
.tab-item{
    font-size:1.2rem;font-weight:600;
    color:var(--text-muted);
    padding-bottom:12px;text-decoration:none;
    position:relative;
}
.tab-item.active{color:var(--primary);}
.tab-item.active::after{
    content:"";position:absolute;left:0;bottom:-2px;
    width:100%;height:3px;background:var(--primary);
}

.content-container{
    width:95%;max-width:1100px;
    margin:40px auto 120px;
    background:var(--bg-card);
    border:1px solid var(--border);
    border-radius:20px;
    padding:35px;
}

.form-label{
    display:block;margin-top:20px;
    margin-bottom:8px;
    font-weight:600;color:var(--primary);
}
.form-control,.dropdown{
    width:100%;background:#1b1b1b;
    border:1px solid var(--border);
    border-radius:10px;
    padding:12px 15px;color:#fff;
}

.toggle-row{
    display:flex;gap:12px;margin-bottom:10px;
}
.toggle-card{
    flex:1;
    text-align:center;
    padding:14px;
    border-radius:14px;
    border:1px solid var(--border);
    cursor:pointer;
    color:var(--text-muted);
    font-weight:600;
}
.toggle-card.active{
    background:var(--primary-soft);
    border-color:var(--primary);
    color:var(--primary);
}

.search-results{
    display:none;position:absolute;
    width:100%;background:#121212;
    border:1px solid var(--border);
    border-radius:10px;margin-top:5px;
    max-height:280px;overflow-y:auto;
    z-index:1000;
}
.search-item{
    padding:12px 15px;
    cursor:pointer;
    display:flex;
    gap:12px;
    align-items:center;
    border-bottom:1px solid #222;
}
.search-item:hover{
    background:var(--primary-soft);
    color:var(--primary);
}

.selected-grid{
    display:grid;
    grid-template-columns:repeat(auto-fill,minmax(100px,1fr));
    gap:15px;margin-top:20px;
}
.selected-card{
    position:relative;border-radius:10px;
    overflow:hidden;border:1px solid var(--border);
}
.selected-card img{width:100%;display:block;}
.selected-card span{
    position:absolute;top:6px;right:6px;
    background:rgba(0,0,0,.6);
    color:white;width:26px;height:26px;
    border-radius:50%;
    display:flex;align-items:center;
    justify-content:center;
    font-weight:900;
    cursor:pointer;
}

.btn-save{
    width:100%;margin-top:35px;
    padding:16px;background:var(--primary);
    color:#1a1a1a;border:none;
    border-radius:14px;
    font-weight:800;font-size:1.1rem;
}
</style>

<div class="tab-wrapper">
    <a href="Listeler.aspx" class="tab-item">Listelerim</a>
    <a href="Favoriler.aspx" class="tab-item">Favoriler</a>
    <a href="DahaSonra.aspx" class="tab-item">Daha Sonra İzle</a>
    <a href="ListeOlustur.aspx" class="tab-item active">Liste Oluştur</a>
</div>

<div class="content-container">

    <asp:HiddenField ID="hfSelectedFilms" runat="server" ClientIDMode="Static"/>

    <h2 style="margin-top:0;color:var(--primary);font-weight:800;">
        Yeni Liste Oluştur
    </h2>

    <label class="form-label">Liste Adı</label>
    <asp:TextBox ID="txtListeAdi" runat="server"
        CssClass="form-control"
        placeholder="Örn: Hafta Sonu Maratonu"/>

    <label class="form-label">Açıklama</label>
    <asp:TextBox ID="txtAciklama" runat="server"
        CssClass="form-control"
        TextMode="MultiLine" Rows="3"/>

    <label class="form-label">Görünürlük</label>
    <asp:DropDownList ID="ddlGorunurluk" runat="server" CssClass="dropdown">
        <asp:ListItem Value="Public">Herkese Açık</asp:ListItem>
        <asp:ListItem Value="Private">Sadece Ben</asp:ListItem>
    </asp:DropDownList>

    <br /><br /><br /><br />
    <div class="toggle-row">
        <div id="typeFilm" class="toggle-card active" onclick="setType('film')">
            Film
        </div>
        <div id="typeDizi" class="toggle-card" onclick="setType('dizi')">
            Dizi
        </div>
    </div>

    <div style="position:relative;">
        <input id="searchInput" class="form-control"
               placeholder="Ara..."
               autocomplete="off"/>
        <div id="searchResults" class="search-results"></div>
    </div>

    

    <div id="selectedContainer" class="selected-grid"></div>

    <asp:Button ID="btnOlustur" runat="server"
        CssClass="btn-save"
        Text="Listeyi Kaydet"
        OnClick="btnOlustur_Click"/>
</div>

<script>
    document.addEventListener("DOMContentLoaded", () => {

        const API_KEY = "02f7a3e84e6acce9ce0b5583deffb717";
        const IMG = "https://image.tmdb.org/t/p/w200";

        let type = "film";
        let selected = [];

        window.setType = t => {
            type = t;
            typeFilm.classList.toggle("active", t === "film");
            typeDizi.classList.toggle("active", t === "dizi");
            searchInput.value = "";
            searchResults.style.display = "none";
        };

        searchInput.oninput = e => {
            const q = e.target.value.trim();
            if (q.length < 2) {
                searchResults.style.display = "none";
                return;
            }

            const endpoint = type === "film" ? "movie" : "tv";

            fetch(`https://api.themoviedb.org/3/search/${endpoint}?api_key=${API_KEY}&language=tr-TR&query=${encodeURIComponent(q)}`)
                .then(r => r.json())
                .then(d => {
                    searchResults.innerHTML = "";
                    d.results
                        .filter(x => x.poster_path)
                        .slice(0, 6)
                        .forEach(x => {
                            const title = x.title || x.name;
                            const div = document.createElement("div");
                            div.className = "search-item";
                            div.innerHTML = `
                        <img src="${IMG + x.poster_path}" width="40">
                        <span>${title}</span>`;
                            div.onclick = () => {
                                if (!selected.some(s => s.id === x.id && s.type === type)) {
                                    selected.push({
                                        id: x.id,
                                        type: type,
                                        poster: x.poster_path
                                    });
                                    sync();
                                }
                                searchInput.value = "";
                                searchResults.style.display = "none";
                            };
                            searchResults.appendChild(div);
                        });
                    searchResults.style.display = "block";
                });
        };

        function sync() {
            selectedContainer.innerHTML = "";
            countDisplay.innerText = selected.length;

            selected.forEach((x, i) => {
                selectedContainer.innerHTML += `
                <div class="selected-card">
                    <img src="${IMG + x.poster}">
                    <span onclick="removeItem(${i})">×</span>
                </div>`;
            });

            hfSelectedFilms.value = JSON.stringify(selected);
        }

        window.removeItem = i => {
            selected.splice(i, 1);
            sync();
        };

        document.addEventListener("click", e => {
            if (!e.target.closest("#searchInput"))
                searchResults.style.display = "none";
        });

    });
</script>

</asp:Content>
