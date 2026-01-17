<%@ Page Title="Dizi Sayfası" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Dizisayfasi.aspx.cs"
    Inherits="Cinescope.Dizisayfasi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hfFilmID" runat="server" />
    <asp:HiddenField ID="hfFilmAdi" runat="server" />
    <asp:HiddenField ID="hfPoster" runat="server" />
    <asp:HiddenField ID="hfPuan" runat="server" />

    <asp:Button ID="btnPuanKaydet" runat="server" Style="display:none;"
        OnClick="btnPuanKaydet_Click" />

<style>
:root{
 --primary:#ffb3d9;
 --primary-soft:#ffd9eb;
 --dark:#1f1f1f;
 --card:#262626;
 --border:#3a3a3a;
 --text:#eee;
 --text-light:#ccc;
}

.filmsayfasi-hero-background{
 position:absolute;top:0;left:0;width:100%;height:550px;
 background-size:cover;background-position:center;
 filter:blur(8px) brightness(.45);z-index:-1;
}

.filmsayfasi-container{
 width:90%;max-width:1200px;margin:auto;padding:50px 0 80px;
}
.filmsayfasi-detail{
 display:flex;gap:40px;background:rgba(30,30,30,.7);
 padding:30px;border-radius:15px;
}
.filmsayfasi-poster{
 width:300px;height:450px;border-radius:15px;object-fit:cover;
 box-shadow:0 4px 20px rgba(0,0,0,.6);
}
.filmsayfasi-section-header{
 margin-top:40px;font-size:1.8rem;font-weight:800;
 color:var(--primary);border-left:6px solid var(--primary);
 padding-left:12px;
}

#movieTitle{
 color:var(--primary)!important;
}

.filmsayfasi-actions{
 margin-top:260px;display:flex;justify-content:space-between;flex-wrap:wrap;
}
.filmsayfasi-btn{
 background:#2b2b2b;color:var(--primary);
 border:1px solid var(--primary);padding:7px 14px;border-radius:10px;font-weight:600;
}
.filmsayfasi-btn:hover{
 background:var(--primary);color:#000;
}

.filmsayfasi-rating span {
    cursor:pointer;
    color:#777;
    font-size:2rem;
    margin-right:5px;
    transition:.2s;
}

.filmsayfasi-rating span.selected,
.filmsayfasi-rating span.hovered {
    color:#ffb3d9;
    transform:scale(1.2);
}


.filmsayfasi-category-pill{
 background:#333;color:var(--primary);padding:6px 14px;
 border-radius:20px;display:inline-block;border:1px solid #555;
 margin:3px;font-weight:600;
}

#filmsayfasi-actorList{
 display:flex;gap:10px;overflow-x: auto;padding-bottom: 10px;
}
.filmsayfasi-actor-item{
 text-align:center;color:#fff;min-width:100px;
}
.filmsayfasi-actor-item img{
 width:100px;height:135px;object-fit:cover;border-radius:10px;margin-bottom:5px;
}

.filmsayfasi-review-box{
 background:#222;padding:20px;border-radius:12px;border:1px solid #444;
}
.form-control{
 background:#333!important;color:#fff!important;border:1px solid var(--primary)!important;
}
.btn-gonder{
 background:var(--primary);border:none;color:#000;font-weight:700;
 padding:10px 20px;border-radius:10px;
}

.review-card{
 background:var(--card);border-radius:15px;padding:18px;margin-bottom:18px;
 border-left:5px solid var(--primary);box-shadow:0 3px 10px rgba(0,0,0,.4);
}
.review-header{
 display:flex;justify-content:space-between;
 border-bottom:1px solid var(--border);margin-bottom:10px;padding-bottom:5px;
}
.review-user{
 color:var(--primary-soft);font-size:1.1rem;font-weight:700;
}
.review-date{
 color:var(--text-light);font-size:.85rem;
}
.review-text{
 color:var(--text-light);margin:10px 0;line-height:1.55;
}

.review-actions{
 display:flex;gap:18px;align-items:center;
}

.like-icon, .like-count{
 color:#ff9fcf!important;font-size:1.1rem;transition:.2s;
}
.dislike-icon, .dislike-count{
 color:#ffcbe2!important;font-size:1.1rem;transition:.2s;
}

.reply-form-container{
 margin-top:12px;margin-left:20px;padding-left:12px;
 border-left:3px solid var(--primary);
 display:none;
}
.reply-form-container.active{ display:block; }

.reply-item{
 background:#1e1e1e;border-radius:10px;padding:14px;margin-top:10px;
 border-left:4px solid var(--primary-soft);
}

.yanit-link{
    color: var(--primary) !important;
    font-weight: 700;
    cursor: pointer;
    text-decoration: none !important;
}

.yanit-link:hover{
    color: var(--primary-soft) !important;
}

.sil-link {
    color: #ff6b6b !important;
    font-size: 0.85rem;
    margin-left: 10px;
    text-decoration: none;
}
.filmsayfasi-btn {
    background:#2b2b2b;
    color:var(--primary);
    border:1px solid var(--primary);
    padding:7px 14px;
    border-radius:10px;
    font-weight:600;
    transition: .2s;
}

.filmsayfasi-btn.active {
    background: var(--primary) !important;
    color: #000 !important;
    border-color: var(--primary) !important;
}
#seasonContainer{
    display:flex;
    flex-wrap:wrap;
    gap:12px;
    margin-top:10px;
}

.season-box{
    padding:8px 16px;
    border:1px solid var(--border);
    border-radius:12px;
    cursor:pointer;
    font-weight:700;
    color:var(--primary);
    background:#1e1e1e;
    transition:.2s;
    user-select:none;
}

.season-box:hover{
    border-color:var(--primary);
}

.season-box.active{
    background:var(--primary);
    color:#000;
    border-color:var(--primary);
}

.episode-list{
    display:none;
    width:100%;
    margin-top:15px;
    padding:15px;
    background:#1e1e1e;
    border:1px solid var(--border);
    border-radius:12px;
}

</style>

<div id="heroBackground" class="filmsayfasi-hero-background"></div>

<div class="filmsayfasi-container">

    <div class="filmsayfasi-detail">
        <img id="posterImg" class="filmsayfasi-poster" src="" />

        <div style="flex: 1;">
            <h2 id="movieTitle" style="font-size: 2.4rem;"></h2>
            <p id="movieOverview" style="color: #ddd; line-height: 1.6;"></p>

            <div class="filmsayfasi-actions">
                <div class="action-buttons-group">
                    <asp:Button ID="btnIzledim" runat="server" CssClass="filmsayfasi-btn"
                        Text="İzledim" OnClick="btnIzledim_Click" />

                    <asp:Button ID="btnDahaSonra" runat="server" CssClass="filmsayfasi-btn"
                        Text="Daha Sonra İzle" OnClick="btnDahaSonra_Click" />

                    <asp:Button ID="btnFavori" runat="server" CssClass="filmsayfasi-btn"
                        Text="Favorilere Ekle" OnClick="btnFavori_Click" />
                    <asp:Label ID="lblIzlendiDurum" runat="server"
    CssClass="d-block mt-2 fw-bold text-success"></asp:Label>

<asp:Label ID="lblFavoriDurum" runat="server"
    CssClass="d-block mt-1 fw-bold text-warning"></asp:Label>
                </div>

                <div class="filmsayfasi-rating">
                    <span data-value="1">★</span>
                    <span data-value="2">★</span>
                    <span data-value="3">★</span>
                    <span data-value="4">★</span>
                    <span data-value="5">★</span>
                </div>
                <asp:Label ID="lblPuanDurum" runat="server"
    CssClass="d-block mt-2 fw-bold text-info"></asp:Label>

            </div>
        </div>
    </div>

    <h3 class="filmsayfasi-section-header">Kategoriler</h3>
    <div id="movieGenres"></div>

    <h3 class="filmsayfasi-section-header">Oyuncular</h3>
    <div id="filmsayfasi-actorList"></div>
    <h3 class="filmsayfasi-section-header">Bölümler</h3>
    <div id="seasonContainer"></div>
    

    <h3 class="filmsayfasi-section-header">Eleştiri Yaz</h3>
    <div class="filmsayfasi-review-box">
        <asp:TextBox ID="txtElestiri" runat="server" CssClass="form-control" TextMode="MultiLine"
            Rows="4" placeholder="Dizi hakkında düşüncelerini yaz..."></asp:TextBox>

        <asp:Button ID="btnElestiriGonder" runat="server"
            CssClass="btn-gonder mt-3" Text="Gönder" OnClick="btnElestiriGonder_Click"/>

        <asp:Label ID="lblMesaj" runat="server" CssClass="mt-3 text-warning d-block"></asp:Label>
    </div>

    <h3 class="filmsayfasi-section-header">Yapılan Eleştiriler</h3>

    <asp:Repeater ID="rptElestiri" runat="server" OnItemCommand="rptElestiri_ItemCommand" OnItemDataBound="rptElestiri_DataBound">
        <ItemTemplate>
            <div class="review-card">
                <div class="review-header">
                    <span class="review-user"><%# Eval("KullaniciAdi") %></span>
                    <span class="review-date"><%# Eval("Tarih","{0:dd.MM.yyyy HH:mm}") %></span>
                </div>

                <div class="review-text"><%# Eval("Yorum") %></div>

                <div class="review-actions">
                    <asp:LinkButton ID="btnBegendim" runat="server"
                        CommandName="Begen"
                        CommandArgument='<%# Eval("ID") %>'>
                        <i class="fa-solid fa-thumbs-up like-icon"></i>
                        <span class="like-count"><%# Eval("Begeni") %></span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnBegenmedim" runat="server"
                        CommandName="Begenme"
                        CommandArgument='<%# Eval("ID") %>'>
                        <i class="fa-solid fa-thumbs-down dislike-icon"></i>
                        <span class="dislike-count"><%# Eval("Begenmeme") %></span>
                    </asp:LinkButton>

                    <a href="javascript:void(0);" class="yanit-link"
                       onclick="toggleReplyForm('<%# Eval("ID") %>');">
                        Yanıtla
                    </a>

                    <asp:LinkButton ID="btnYorumSil" runat="server"
                        CommandName="SilYorum"
                        CommandArgument='<%# Eval("ID") %>'
                        CssClass="sil-link" Visible="false">
                        🗑 Sil
                    </asp:LinkButton>
                </div>

                <div id="replyForm_<%# Eval("ID") %>" class="reply-form-container">
                    <asp:TextBox ID="txtYanit" runat="server"
                        CssClass="form-control mb-2"
                        TextMode="MultiLine" Rows="2"
                        placeholder="Yanıt yaz..."></asp:TextBox>

                    <asp:Button ID="btnYanitGonder" runat="server"
                        Text="Yanıtı Gönder"
                        CommandArgument='<%# Eval("ID") %>'
                        OnClick="btnYanitGonder_Click"
                        CssClass="btn-gonder btn-sm" />
                </div>

                <asp:Repeater ID="rptYanit" runat="server"
                    OnItemDataBound="rptYanit_ItemDataBound"
                    OnItemCommand="rptYanit_ItemCommand">
                    <ItemTemplate>
                        <div class="reply-item">
                            <div class="review-header">
                                <b><%# Eval("KullaniciAdi") %></b> —
                                <small><%# Eval("Tarih", "{0:dd.MM.yyyy HH:mm}") %></small>
                            </div>
                            <div class="review-body">
                                <%# Eval("Yorum") %>
                            </div>
                            <div class="review-actions">
                                <asp:LinkButton ID="btnYanitBegen" runat="server"
                                    CommandName="YanitBegen"
                                    CommandArgument='<%# Eval("ID") %>'>
                                    <i class="fa-solid fa-thumbs-up like-icon"></i>
                                    <span class="like-count"><%# Eval("Begeni") %></span>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnYanitBegenme" runat="server"
                                    CommandName="YanitBegenme"
                                    CommandArgument='<%# Eval("ID") %>'>
                                    <i class="fa-solid fa-thumbs-down dislike-icon"></i>
                                    <span class="dislike-count"><%# Eval("Begenmeme") %></span>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnYanitSil" runat="server"
                                    CommandName="SilYanit"
                                    CommandArgument='<%# Eval("ID") %>'
                                    CssClass="sil-link" Visible="false">
                                    🗑 Sil
                                </asp:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<script>
    document.addEventListener("DOMContentLoaded", function () {

        const API_KEY = "02f7a3e84e6acce9ce0b5583deffb717";
        const IMAGE_BASE = "https://image.tmdb.org/t/p/w500";

        const params = new URLSearchParams(window.location.search);
        const diziId = params.get("id");

        if (!diziId) {
            alert("Dizi bulunamadı");
            return;
        }

        window.toggleSeason = function (seasonNo) {

            document.querySelectorAll(".season-box")
                .forEach(x => x.classList.remove("active"));

            const box = document.getElementById("box_" + seasonNo);
            if (box) box.classList.add("active");

            fetch("Dizisayfasi.aspx/SezonIzlendi", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    diziId: parseInt(diziId),
                    sezonNo: seasonNo
                })
            });
        };

        fetch(`https://api.themoviedb.org/3/tv/${diziId}?api_key=${API_KEY}&language=tr-TR`)
            .then(r => r.json())
            .then(d => {

                document.getElementById("movieTitle").innerText = d.name;
                document.getElementById("movieOverview").innerText = d.overview || "Açıklama bulunamadı.";
                document.getElementById("posterImg").src = d.poster_path ? IMAGE_BASE + d.poster_path : "https://via.placeholder.com/300x450";

                if (d.backdrop_path) {
                    document.getElementById("heroBackground").style.backgroundImage =
                        `url('${IMAGE_BASE}${d.backdrop_path}')`;
                }

                let g = "";
                if (d.genres) d.genres.forEach(x => g += `<span class="filmsayfasi-category-pill">${x.name}</span>`);
                document.getElementById("movieGenres").innerHTML = g;

                document.getElementById("<%= hfFilmID.ClientID %>").value = diziId;
            document.getElementById("<%= hfFilmAdi.ClientID %>").value = d.name;
            document.getElementById("<%= hfPoster.ClientID %>").value = d.poster_path || "";
        });

    fetch(`https://api.themoviedb.org/3/tv/${diziId}/credits?api_key=${API_KEY}&language=tr-TR`)
        .then(r => r.json())
        .then(d => {
            let h = "";
            if (d.cast) {
                d.cast.slice(0, 10).forEach(a => {
                    h += `
                    <div class="filmsayfasi-actor-item">
                        <img src="${a.profile_path ? IMAGE_BASE + a.profile_path : 'https://via.placeholder.com/100x135'}">
                        <div>${a.name}</div>
                    </div>`;
                });
            }
            document.getElementById("filmsayfasi-actorList").innerHTML = h;
        });

    fetch(`https://api.themoviedb.org/3/tv/${diziId}?api_key=${API_KEY}&language=tr-TR`)
        .then(r => r.json())
        .then(tv => {

            if (!tv.seasons) return;

            let s = "";

            tv.seasons.forEach(season => {

                if (season.season_number === 0) return;

                s += `
                <div id="box_${season.season_number}"
                     class="season-box"
                     onclick="toggleSeason(${season.season_number})">
                    Sezon ${season.season_number}
                </div>`;
            });

            document.getElementById("seasonContainer").innerHTML = s;
        });

    const stars = document.querySelectorAll(".filmsayfasi-rating span");
    const hfPuan = document.getElementById("<%= hfPuan.ClientID %>");

    function applyStars(v) {
        stars.forEach(s => {
            const x = parseInt(s.dataset.value);
            s.classList.toggle("selected", x <= v);
        });
    }

    stars.forEach(star => {

        star.addEventListener("mouseover", () => {
            applyStars(parseInt(star.dataset.value));
        });

        star.addEventListener("mouseout", () => {
            applyStars(parseInt(hfPuan.value || "0"));
        });

        star.addEventListener("click", () => {
            const v = parseInt(star.dataset.value);
            hfPuan.value = v;
            applyStars(v);
            __doPostBack("<%= btnPuanKaydet.UniqueID %>", "");
        });
    });

});
</script>


</asp:Content>