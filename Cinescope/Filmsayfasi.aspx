<%@ Page Title="Film Sayfası" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Filmsayfasi.aspx.cs"
    Inherits="Cinescope.Filmsayfasi" %>

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

.filmsayfasi-rating span{
 cursor:pointer;color:#777;font-size:2rem;margin-right:5px;transition:.2s;
}
.filmsayfasi-rating span.selected,
.filmsayfasi-rating span.hovered{
 color:var(--primary)!important;transform:scale(1.2);
}

.filmsayfasi-category-pill{
 background:#333;color:var(--primary);padding:6px 14px;
 border-radius:20px;display:inline-block;border:1px solid #555;
 margin:3px;font-weight:600;
}

#filmsayfasi-actorList{
 display:flex;gap:10px;
}
.filmsayfasi-actor-item{
 text-align:center;color:#fff;width:100px;
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
.btn-active {
    background: #ffb3d9 !important;
    color: #1f1f1f !important;
    border: none !important;
    font-weight: 800;
}

.btn-disabled {
    opacity: 0.85;
    cursor: default !important;
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
                </div>

                <div class="filmsayfasi-rating">
                    <span data-value="1">★</span>
                    <span data-value="2">★</span>
                    <span data-value="3">★</span>
                    <span data-value="4">★</span>
                    <span data-value="5">★</span>
                </div>
            </div>
        </div>
    </div>

    <h3 class="filmsayfasi-section-header">Kategoriler</h3>
    <div id="movieGenres"></div>

    <h3 class="filmsayfasi-section-header">Oyuncular</h3>
    <div id="filmsayfasi-actorList"></div>

    <h3 class="filmsayfasi-section-header">Eleştiri Yaz</h3>
    <div class="filmsayfasi-review-box">
        <asp:TextBox ID="txtElestiri" runat="server" CssClass="form-control" TextMode="MultiLine"
            Rows="4" placeholder="Düşüncelerini yaz..."></asp:TextBox>

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
    const API_KEY = "02f7a3e84e6acce9ce0b5583deffb717";
    const IMAGE_BASE = "https://image.tmdb.org/t/p/w500";

    const params = new URLSearchParams(window.location.search);
    const movieId = params.get("id");

    if (!movieId) {
        alert("Film ID bulunamadı");
    }

    document.getElementById("<%= hfFilmID.ClientID %>").value = movieId;

    
    fetch(`https://api.themoviedb.org/3/movie/${movieId}?api_key=${API_KEY}&language=tr-TR`)
        .then(res => res.json())
        .then(data => {

            document.getElementById("movieTitle").innerText = data.title || "";

            
            document.getElementById("movieOverview").innerText = data.overview || "";

            if (data.poster_path) {
                document.getElementById("posterImg").src = IMAGE_BASE + data.poster_path;
                document.getElementById("heroBackground").style.backgroundImage =
                    `url('https://image.tmdb.org/t/p/w1280${data.poster_path}')`;
            } else {
                document.getElementById("posterImg").src =
                    "https://via.placeholder.com/300x450";
            }

            document.getElementById("<%= hfFilmAdi.ClientID %>").value = data.title || "";
            document.getElementById("<%= hfPoster.ClientID %>").value = data.poster_path || "";

            let genreHtml = "";
            if (data.genres) {
                data.genres.forEach(g => {
                    genreHtml += `<span class="filmsayfasi-category-pill">${g.name}</span>`;
                });
            }
            document.getElementById("movieGenres").innerHTML = genreHtml;
        });

    fetch(`https://api.themoviedb.org/3/movie/${movieId}/credits?api_key=${API_KEY}&language=tr-TR`)
        .then(res => res.json())
        .then(data => {
            let html = "";
            if (data.cast) {
                data.cast.slice(0, 10).forEach(actor => {
                    html += `
                        <div class="filmsayfasi-actor-item">
                            <img src="${actor.profile_path ? IMAGE_BASE + actor.profile_path : 'https://via.placeholder.com/100x135'}">
                            <div>${actor.name}</div>
                        </div>`;
                });
            }
            document.getElementById("filmsayfasi-actorList").innerHTML = html;
        });

  
    const starSpans = document.querySelectorAll(".filmsayfasi-rating span");
    const hfPuanInput = document.getElementById("<%= hfPuan.ClientID %>");

    function applySelectedFromValue(val) {
        starSpans.forEach(s => {
            const v = parseInt(s.getAttribute("data-value"));
            if (v <= val) s.classList.add("selected");
            else s.classList.remove("selected");
        });
    }

    (function initStars() {
        const current = parseInt(hfPuanInput.value || "0");
        if (current > 0) applySelectedFromValue(current);
    })();

    starSpans.forEach(span => {
        span.addEventListener("mouseover", function () {
            const value = parseInt(this.getAttribute("data-value"));
            starSpans.forEach(s => {
                const v = parseInt(s.getAttribute("data-value"));
                if (v <= value) s.classList.add("hovered");
                else s.classList.remove("hovered");
            });
        });

        span.addEventListener("mouseout", function () {
            const current = parseInt(hfPuanInput.value || "0");
            starSpans.forEach(s => s.classList.remove("hovered"));
            if (current > 0) applySelectedFromValue(current);
            else starSpans.forEach(s => s.classList.remove("selected"));
        });

        span.addEventListener("click", function () {
            const value = parseInt(this.getAttribute("data-value"));
            hfPuanInput.value = value;
            applySelectedFromValue(value);
            __doPostBack("<%= btnPuanKaydet.UniqueID %>", "");
        });
    });


    function toggleReplyForm(id) {
        const form = document.getElementById(`replyForm_${id}`);
        form.classList.toggle("active");
    }
</script>


</asp:Content>