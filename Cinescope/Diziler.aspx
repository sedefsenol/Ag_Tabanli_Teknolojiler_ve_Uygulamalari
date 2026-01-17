<%@ Page Title="Diziler" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Diziler.aspx.cs" Inherits="Cinescope.Diziler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style>
    :root { --primary: #ffb3d9; --primary-soft: rgba(255, 179, 217, 0.05); --bg-dark: #1f1f1f; --bg-card: #262626; --border: #333; --text: #eee; }
    body { background: var(--bg-dark) !important; color: var(--text); font-family: 'Segoe UI', sans-serif; }
    .dizi-section-title { font-size: 1.5rem; font-weight: 700; color: var(--primary); margin-bottom: 25px; margin-top: 40px; border-left: 4px solid var(--primary); padding-left: 12px; }
    .filter-wrapper { background: var(--bg-card); padding: 15px 20px; border-radius: 12px; border: 1px solid var(--border); width: 95%; margin: 30px auto; display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 15px; }
    .dizi-btn-filter { background: transparent; border: 1px solid var(--border); color: #999; padding: 8px 16px; border-radius: 8px; font-weight: 600; font-size: 0.9rem; transition: 0.2s; cursor: pointer; }
    .dizi-btn-filter:hover { border-color: var(--primary); color: var(--primary); }
    .dizi-dropdown-menu { background: #222 !important; border: 1px solid #444 !important; border-radius: 8px !important; box-shadow: 0 10px 25px rgba(0,0,0,0.4) !important; z-index: 1050; }
    .dizi-dropdown-item { color: #bbb !important; padding: 8px 20px !important; font-size: 0.9rem !important; cursor: pointer; }
    .dizi-dropdown-item:hover { background: var(--primary-soft) !important; color: var(--primary) !important; }
    .search-container { display: flex; align-items: center; background: #1b1b1b; border: 1px solid var(--border); border-radius: 8px; padding-right: 5px; min-width: 280px; }
    .dizi-search-box { background: transparent !important; border: none !important; padding: 9px 12px !important; color: white !important; font-size: 0.9rem !important; width: 100%; outline: none !important; }
    .search-btn { background: var(--primary); color: #111; border: none; border-radius: 6px; padding: 6px 15px; cursor: pointer; font-weight: 700; font-size: 0.85rem; }
    .dizi-container { display: grid; grid-template-columns: repeat(auto-fill, minmax(160px, 1fr)); gap: 15px; }
    .dizi-card { height: 240px; background: #222; border: 1px solid var(--border); border-radius: 10px; overflow: hidden; cursor: pointer; transition: 0.2s; }
    .dizi-card img { width: 100%; height: 100%; object-fit: cover; }
    .review-poster { width: 70px; height: 100px; border-radius: 8px; object-fit: cover; }
    .hidden { display: none; }
</style>

   <div class="filter-wrapper">
    <div class="filter-groups d-flex gap-2 flex-wrap">

        <div class="btn-group">
            <button id="btnYear" type="button"
                class="dizi-btn-filter dropdown-toggle"
                data-bs-toggle="dropdown">
                Yıl
            </button>
            <ul class="dropdown-menu dizi-dropdown-menu">
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear(null)">Tümü</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2025')">2025</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2024')">2024</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2023')">2023</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2022')">2022</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2021')">2021</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2020')">2020</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2015')">2010–2019</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('2000')">2000–2009</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectYear('1990')">1990–1999</a></li>
            </ul>
        </div>

     
        <div class="btn-group">
            <button id="btnScore" type="button"
                class="dizi-btn-filter dropdown-toggle"
                data-bs-toggle="dropdown">
                Puan
            </button>
            <ul class="dropdown-menu dizi-dropdown-menu">
                <% for (int i = 10; i >= 1; i--) { %>
                    <li>
                        <a class="dropdown-item dizi-dropdown-item"
                           onclick="selectScore(<%=i%>)"><%=i%>+</a>
                    </li>
                <% } %>
            </ul>
        </div>

        <div class="btn-group">
            <button id="btnGenre" type="button"
                class="dizi-btn-filter dropdown-toggle"
                data-bs-toggle="dropdown">
                Tür
            </button>
            <ul class="dropdown-menu dizi-dropdown-menu">

                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre(null)">Tümü</a></li>

                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Aksiyon-Macera')">Aksiyon & Macera</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Dram')">Dram</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Bilim-Kurgu-Fantazi')">Bilim-Kurgu & Fantastik</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Gizem')">Gizem</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Suc')">Suç</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Gerilim')">Gerilim</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Komedi')">Komedi</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Animasyon')">Animasyon</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Belgesel')">Belgesel</a></li>
                <li><a class="dropdown-item dizi-dropdown-item" onclick="selectGenre('Aile')">Aile</a></li>

            </ul>
        </div>

        <button type="button"
            class="dizi-btn-filter"
            onclick="applyFilters()"
            style="background: var(--primary-soft); color: var(--primary);">
            Uygula
        </button>
    </div>

    <div class="search-container">
        <input id="searchInput" type="text"
            class="form-control dizi-search-box"
            placeholder="Dizi Ara..." />
        <button type="button"
            class="search-btn"
            onclick="applyFilters()">Ara</button>
    </div>
</div>


    <div id="section-popular" class="container mt-2">
        <h3 class="dizi-section-title">Popüler Diziler</h3>
        <div id="popular-row" class="dizi-container"></div>
    </div>

    <div id="section-reviews" class="container mt-5">
        <h3 class="dizi-section-title">Popüler Dizi Eleştirileri</h3>
        <div class="row g-3">
            <asp:Repeater ID="rptElestiriler" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-6">
                        <div style="display: flex; gap: 15px; background: var(--bg-card); padding: 15px; border-radius: 12px; border: 1px solid var(--border);">
                            <img class="review-poster" src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>">
                            <div style="flex:1;">
                                <a href='Dizisayfasi.aspx?id=<%# Eval("DiziID") %>&elestiri=<%# Eval("ID") %>' 
                                   style="color: var(--primary); font-weight: 600; text-decoration: none;">
                                   <%# Eval("DiziAdi") %>
                                </a>
                                <p style="color: #aaa; font-size: 0.9rem; margin: 5px 0;"><%# Eval("Yorum") %></p>
                                <div style="display: flex; justify-content: space-between; align-items: center; margin-top: 5px;">
                                    <small style="color:#666;"><%# Eval("Tarih") %></small>
                                   
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <div id="section-airing" class="container mt-5 mb-5">
        <h3 class="dizi-section-title">Gündemdeki Diziler</h3>
        <div id="airing-row" class="dizi-container"></div>
    </div>

    <div id="filtered-section" class="hidden">
        <div class="container mt-4 mb-5">
            <h3 class="dizi-section-title">Arama Sonuçları</h3>
            <div id="filtered-row" class="dizi-container"></div>
        </div>
    </div>

<script>
    const API_KEY = "02f7a3e84e6acce9ce0b5583deffb717";
    const IMG = "https://image.tmdb.org/t/p/w500";
    let selectedYear = null, selectedScore = null, selectedGenre = null;
    const GENRES = { "Aksiyon-Macera": 10759, "Dram": 18, "Bilim-Kurgu-Fantazi": 10765 };

    function selectYear(y) { selectedYear = y; document.getElementById("btnYear").innerText = y; }
    function selectScore(s) { selectedScore = s; document.getElementById("btnScore").innerText = s + "+"; }
    function selectGenre(g) { selectedGenre = GENRES[g]; document.getElementById("btnGenre").innerText = g; }

    function goDetail(m) {
        const title = m.name || m.original_name;
        const url = `Dizisayfasi.aspx?id=${m.id}&title=${encodeURIComponent(title)}&overview=${encodeURIComponent(m.overview)}&poster=${m.poster_path}`;
        window.location.href = url;
    }

    async function loadSimple(url, target) {
        const r = await fetch(url).then(res => res.json());
        const container = document.getElementById(target);
        container.innerHTML = "";
        r.results.slice(0, 7).forEach(m => {
            if (m.poster_path) {
                const card = document.createElement("div");
                card.className = "dizi-card";
                card.onclick = () => goDetail(m);
                card.innerHTML = `<img src="${IMG}${m.poster_path}">`;
                container.appendChild(card);
            }
        });
    }

    async function applyFilters() {
        const q = document.getElementById("searchInput").value.trim();
        let url = "";
        if (q) {
            url = `https://api.themoviedb.org/3/search/tv?api_key=${API_KEY}&language=tr-TR&query=${encodeURIComponent(q)}`;
        }
        else if (selectedYear || selectedGenre || selectedScore) {
            url = `https://api.themoviedb.org/3/discover/tv?api_key=${API_KEY}&language=tr-TR&sort_by=popularity.desc`;
            if (selectedYear) url += `&first_air_date_year=${selectedYear}`;
            if (selectedScore) url += `&vote_average.gte=${selectedScore}`;
            if (selectedGenre) url += `&with_genres=${selectedGenre}`;
        } else return;

        const res = await fetch(url).then(r => r.json());
        const container = document.getElementById("filtered-row");
        container.innerHTML = "";

        document.getElementById("section-popular").classList.add("hidden");
        document.getElementById("section-airing").classList.add("hidden");
        document.getElementById("section-reviews").classList.add("hidden");
        document.getElementById("filtered-section").classList.remove("hidden");

        res.results.forEach(m => {
            if (m.poster_path) {
                const card = document.createElement("div");
                card.className = "dizi-card";
                card.onclick = () => goDetail(m);
                card.innerHTML = `<img src="${IMG}${m.poster_path}">`;
                container.appendChild(card);
            }
        });
    }

    loadSimple(`https://api.themoviedb.org/3/tv/popular?api_key=${API_KEY}&language=tr-TR`, "popular-row");
    loadSimple(`https://api.themoviedb.org/3/tv/on_the_air?api_key=${API_KEY}&language=tr-TR`, "airing-row");

    document.getElementById("searchInput").addEventListener("keyup", e => { if (e.key === "Enter") applyFilters(); });
</script>
</asp:Content>