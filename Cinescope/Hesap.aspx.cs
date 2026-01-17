using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class Hesap : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null)
                Response.Redirect("girisyap.aspx");

            if (!IsPostBack)
            {
                KullaniciBilgileriniGetir();
                TakipSayilariGetir();
                IzlenenFilmlerMiniGetir();
                IzlenenDizilerMiniGetir();
                ListelerMiniGetir();
                SonFilmElestirileriGetir();
                SonDiziElestirileriGetir();
                IstatistikleriGetir();
            }
        }

        void KullaniciBilgileriniGetir()
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT KullaniciAdi, Hakkimda FROM Kullanici WHERE ID=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblKullaniciAdi.Text = dr["KullaniciAdi"].ToString();
                    lblHakkimda.Text = dr["Hakkimda"].ToString();
                }
            }
        }

        void TakipSayilariGetir()
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                lblTakipci.Text = new SqlCommand("SELECT COUNT(*) FROM Takip WHERE TakipEdilenID=@id", con)
                { Parameters = { new SqlParameter("@id", id) } }.ExecuteScalar().ToString();

                lblTakipEdilen.Text = new SqlCommand("SELECT COUNT(*) FROM Takip WHERE TakipEdenID=@id", con)
                { Parameters = { new SqlParameter("@id", id) } }.ExecuteScalar().ToString();
            }
        }

        void IzlenenFilmlerMiniGetir()
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 6 FilmID, FilmAdi, PosterPath FROM IzlenenFilmler WHERE KullaniciID=@id ORDER BY IzlenmeTarihi DESC", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptIzlenenFilmlerMini.DataSource = dt;
                rptIzlenenFilmlerMini.DataBind();
            }
        }

        void IzlenenDizilerMiniGetir()
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 6 DiziID, DiziAdi, PosterPath FROM IzlenenDiziler WHERE KullaniciID=@id ORDER BY IzlenmeTarihi DESC", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptIzlenenDizilerMini.DataSource = dt;
                rptIzlenenDizilerMini.DataBind();
            }
        }

        void ListelerMiniGetir()
        {
            int uid = Convert.ToInt32(Session["KullaniciID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT ID, ListeAdi FROM Listeler WHERE KullaniciID=@uid ORDER BY Tarih DESC", con))
            {
                cmd.Parameters.AddWithValue("@uid", uid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptListelerMini.DataSource = dt;
                rptListelerMini.DataBind();
            }
        }

        void SonFilmElestirileriGetir()
        {
            int uid = Convert.ToInt32(Session["KullaniciID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 5 ID, FilmID, FilmAdi, Poster, Yorum, Begeni FROM Elestiri WHERE KullaniciID=@uid ORDER BY Tarih DESC", con))
            {
                cmd.Parameters.AddWithValue("@uid", uid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptSonFilmElestiriler.DataSource = dt;
                rptSonFilmElestiriler.DataBind();
            }
        }

        void SonDiziElestirileriGetir()
        {
            int uid = Convert.ToInt32(Session["KullaniciID"]);
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 5 ID, DiziID, DiziAdi, Poster, Yorum, Begeni FROM DiziElestiri WHERE KullaniciID=@uid ORDER BY Tarih DESC", con))
            {
                cmd.Parameters.AddWithValue("@uid", uid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptSonDiziElestiriler.DataSource = dt;
                rptSonDiziElestiriler.DataBind();
            }
        }

        void IstatistikleriGetir()
        {
            int uid = Convert.ToInt32(Session["KullaniciID"]);
            int filmSayisi = 0, diziSayisi = 0, listeSayisi = 0;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                filmSayisi = Convert.ToInt32(new SqlCommand("SELECT COUNT(DISTINCT FilmID) FROM IzlenenFilmler WHERE KullaniciID=@uid", con) { Parameters = { new SqlParameter("@uid", uid) } }.ExecuteScalar());
                diziSayisi = Convert.ToInt32(new SqlCommand("SELECT COUNT(DISTINCT DiziID) FROM IzlenenDiziler WHERE KullaniciID=@uid", con) { Parameters = { new SqlParameter("@uid", uid) } }.ExecuteScalar());
                listeSayisi = Convert.ToInt32(new SqlCommand("SELECT COUNT(*) FROM Listeler WHERE KullaniciID=@uid", con) { Parameters = { new SqlParameter("@uid", uid) } }.ExecuteScalar());
            }

            lblFilmSayisi.Text = filmSayisi.ToString();
            lblDiziSayisi.Text = diziSayisi.ToString();
            lblListeSayisi.Text = listeSayisi.ToString();

            int toplam = filmSayisi + diziSayisi;
            if (toplam < 5) lblRozet.Text = "🐣 Yeni Üye";
            else if (toplam < 15) lblRozet.Text = "🌱 Yeni Başlayan İzleyici";
            else if (toplam < 30) lblRozet.Text = "🎬 Film & Dizi Sever";
            else if (toplam < 60) lblRozet.Text = "🍿 Tutkulu İzleyici";
            else if (toplam < 100) lblRozet.Text = "🔥 CineScope Bağımlısı";
            else lblRozet.Text = "👑 Efsane İzleyici";
        }

        protected void btnAyarlar_Click(object sender, EventArgs e)
        {
            Response.Redirect("HesapAyarlari.aspx");
        }

        protected void btnCikis_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("girisyap.aspx");
        }
    }
}