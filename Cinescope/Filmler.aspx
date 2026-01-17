<%@ Page Title="Filmler" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Filmler.aspx.cs" Inherits="Cinescope.Filmler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    :root { --primary: #ffb3d9; --primary-soft: rgba(255, 179, 217, 0.05); --bg-dark: #1f1f1f; --bg-card: #262626; --border: #333; --text: #eee; }
    body { background: var(--bg-dark) !important; color: var(--text); font-family: 'Segoe UI', sans-serif; }
    .filmler-section-title { font-size: 1.5rem; font-weight: 700; color: var(--primary); margin-bottom: 25px; margin-top: 40px; border-left: 4px solid var(--primary); padding-left: 12px; }
    .filter-wrapper { background: var(--bg-card); padding: 15px 20px; border-radius: 12px; border: 1px solid var(--border); width: 95%; margin: 30px auto; display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 15px; }
    .filter-groups { display: flex; gap: 10px; align-items: center; flex-wrap: wrap; }
    .filmler-btn-filter { background: transparent; border: 1px solid var(--border); color: #999; padding: 8px 16px; border-radius: 8px; font-weight: 600; font-size: 0.9rem; transition: 0.2s; }
    .filmler-btn-filter:hover { border-color: var(--primary); color: var(--primary); }
    .filmler-dropdown-menu { background: #222 !important; border: 1px solid #444 !important; border-radius: 8px !important; max-height: none !important; overflow: visible !important; box-shadow: 0 10px 25px rgba(0,0,0,0.4) !important; z-index: 1050; }
    .filmler-dropdown-item { color: #bbb !important; padding: 8px 20px !important; font-size: 0.9rem !important; cursor: pointer; }
    .filmler-dropdown-item:hover { background: var(--primary-soft) !important; color: var(--primary) !important; }
    .search-container { display: flex; align-items: center; background: #1b1b1b; border: 1px solid var(--border); border-radius: 8px; padding-right: 5px; min-width: 280px; }
    .filmler-search-box { background: transparent !important; border: none !important; padding: 9px 12px !important; color: white !important; font-size: 0.9rem !important; width: 100%; outline: none !important; }
    .search-btn { background: var(--primary); color: #111; border: none; border-radius: 6px; padding: 6px 15px; cursor: pointer; font-weight: 700; font-size: 0.85rem; }
    .filmler-film-container { display: grid; grid-template-columns: repeat(auto-fill, minmax(160px, 1fr)); gap: 15px; }
    .filmler-film-card { height: 240px; background: #222; border: 1px solid var(--border); border-radius: 10px; overflow: hidden; cursor: pointer; transition: border-color 0.2s; }
    .filmler-film-card:hover { border-color: var(--primary); }
    .filmler-film-card img { width: 100%; height: 100%; object-fit: cover; }
    .filmler-review-item { display: flex; gap: 15px; background: var(--bg-card); padding: 15px; border-radius: 12px; border: 1px solid var(--border); }
    .review-poster { width: 70px; height: 100px; border-radius: 8px; object-fit: cover; }
    .review-title { color: var(--primary); font-weight: 600; text-decoration: none; font-size: 1.1rem; }
    .review-text { color: #aaa; font-size: 0.9rem; line-height: 1.4; margin: 5px 0; }
    .hidden { display: none; }
</style>

    <div class="filter-wrapper">
    <div class="filter-groups d-flex gap-2 flex-wrap">

        <div class="btn-group">
            <button id="btnYear" type="button"
                class="filmler-btn-filter dropdown-toggle"
                data-bs-toggle="dropdown">
                Yıl
            </button>
            <ul class="dropdown-menu filmler-dropdown-menu">
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear(null)">Tümü</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('2025')">2025</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('2024')">2024</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('2023')">2023</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('2022')">2022</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('2021')">2021</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('2015')">2010–2019</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('2000')">2000–2009</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('1990')">1990–1999</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectYear('1980')">1980–1989</a></li>
            </ul>
        </div>

   
        <div class="btn-group">
            <button id="btnScore" type="button"
                class="filmler-btn-filter dropdown-toggle"
                data-bs-toggle="dropdown">
                Puan
            </button>
            <ul class="dropdown-menu filmler-dropdown-menu">
                <% for (int i = 10; i >= 1; i--) { %>
                    <li>
                        <a class="dropdown-item filmler-dropdown-item"
                           onclick="selectScore(<%=i%>)"><%=i%>+</a>
                    </li>
                <% } %>
            </ul>
        </div>

        <div class="btn-group">
            <button id="btnGenre" type="button"
                class="filmler-btn-filter dropdown-toggle"
                data-bs-toggle="dropdown">
                Tür
            </button>
            <ul class="dropdown-menu filmler-dropdown-menu">

                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre(null)">Tümü</a></li>

                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Aksiyon')">Aksiyon</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Macera')">Macera</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Bilim-Kurgu')">Bilim-Kurgu</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Fantastik')">Fantastik</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Dram')">Dram</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Gerilim')">Gerilim</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Suc')">Suç</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Korku')">Korku</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Komedi')">Komedi</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Animasyon')">Animasyon</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Romantik')">Romantik</a></li>
                <li><a class="dropdown-item filmler-dropdown-item" onclick="selectGenre('Belgesel')">Belgesel</a></li>

            </ul>
        </div>

        <button type="button"
            class="filmler-btn-filter"
            onclick="applyFilters()"
            style="background: var(--primary-soft); color: var(--primary);">
            Uygula
        </button>
    </div>

    <div class="search-container">
        <input id="searchInput" type="text"
            class="form-control filmler-search-box"
            placeholder="Film ara..." />
        <button type="button"
            class="search-btn"
            onclick="applyFilters()">Ara</button>
    </div>
</div>


    <div id="section-popular" class="container mt-2">
        <h3 class="filmler-section-title">Popüler Filmler</h3>
        <div id="popular-row" class="filmler-film-container"></div>
    </div>

    <div id="section-reviews" class="container mt-5">
        <h3 class="filmler-section-title">Popüler Eleştiriler</h3>
        <div class="row g-3">
            <asp:Repeater ID="rptElestiriler" runat="server">
                <ItemTemplate>
                    <div class="col-12 col-md-6">
                        <div class="filmler-review-item">
                            <img class="review-poster" src="https://image.tmdb.org/t/p/w200<%# Eval("Poster") %>">
                            <div style="flex:1;">
                                <a href='Filmsayfasi.aspx?id=<%# Eval("FilmID") %>' class="review-title"><%# Eval("FilmAdi") %></a>
                                <p class="review-text"><%# Eval("Yorum") %></p>
                                <small style="color:#666;"><%# Eval("Tarih") %></small>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <div id="section-upcoming" class="container mt-5 mb-5">
        <h3 class="filmler-section-title">Yakında Çıkacaklar</h3>
        <div id="upcoming-row" class="filmler-film-container"></div>
    </div>

    <div id="filtered-section" class="hidden">
        <div class="container mt-4 mb-5">
            <h3 class="filmler-section-title">Arama Sonuçları</h3>
            <div id="filtered-row" class="filmler-film-container"></div>
        </div>
    </div>

<script>
    const API_KEY = "02f7a3e84e6acce9ce0b5583deffb717";
    const IMG = "https://image.tmdb.org/t/p/w500";
    let selectedYear = null, selectedScore = null, selectedGenre = null;
    const GENRES = { "Aksiyon": 28, "Bilim-Kurgu": 878, "Komedi": 35 };

    function selectYear(y) { selectedYear = y; document.getElementById("btnYear").innerText = y; }
    function selectScore(s) { selectedScore = s; document.getElementById("btnScore").innerText = s + "+"; }
    function selectGenre(g) { selectedGenre = GENRES[g]; document.getElementById("btnGenre").innerText = g; }

    function goDetail(m) {
        const title = m.title || m.name;
        const url = `Filmsayfasi.aspx?id=${m.id}&title=${encodeURIComponent(title)}&overview=${encodeURIComponent(m.overview)}&poster=${m.poster_path}`;
        window.location.href = url;
    }

    async function loadSimple(url, target) {
        const r = await fetch(url).then(res => res.json());
        const container = document.getElementById(target);
        container.innerHTML = "";
        r.results.slice(0, 7).forEach(m => {
            if (m.poster_path) {
                const card = document.createElement("div");
                card.className = "filmler-film-card";
                card.onclick = () => goDetail(m);
                card.innerHTML = `<img src="${IMG}${m.poster_path}">`;
                container.appendChild(card);
            }
        });
    }

    loadSimple(`https://api.themoviedb.org/3/movie/popular?api_key=${API_KEY}&language=tr-TR`, "popular-row");
    loadSimple(`https://api.themoviedb.org/3/movie/upcoming?api_key=${API_KEY}&language=tr-TR`, "upcoming-row");

    async function applyFilters() {
        const q = document.getElementById("searchInput").value.trim();
        let url = "";
        if (q) { url = `https://api.themoviedb.org/3/search/movie?api_key=${API_KEY}&language=tr-TR&query=${encodeURIComponent(q)}`; }
        else if (selectedYear || selectedGenre || selectedScore) {
            url = `https://api.themoviedb.org/3/discover/movie?api_key=${API_KEY}&language=tr-TR&sort_by=popularity.desc`;
            if (selectedYear) url += `&primary_release_year=${selectedYear}`;
            if (selectedScore) url += `&vote_average.gte=${selectedScore}`;
            if (selectedGenre) url += `&with_genres=${selectedGenre}`;
        } else return;

        const res = await fetch(url).then(r => r.json());
        const container = document.getElementById("filtered-row");
        container.innerHTML = "";
        document.getElementById("section-popular").classList.add("hidden");
        document.getElementById("section-upcoming").classList.add("hidden");
        document.getElementById("section-reviews").classList.add("hidden");
        document.getElementById("filtered-section").classList.remove("hidden");

        res.results.slice(0, 7).forEach(m => {
            if (m.poster_path) {
                const card = document.createElement("div");
                card.className = "filmler-film-card";
                card.onclick = () => goDetail(m);
                card.innerHTML = `<img src="${IMG}${m.poster_path}">`;
                container.appendChild(card);
            }
        });
    }
    document.getElementById("searchInput").addEventListener("keyup", e => { if (e.key === "Enter") applyFilters(); });
</script>
</asp:Content>