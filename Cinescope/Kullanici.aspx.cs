using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class Kullanici : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;
        int uid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("Anasayfa.aspx");

            uid = Convert.ToInt32(Request.QueryString["id"]);

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
                TakipDurumuGetir();
            }
        }

        void KullaniciBilgileriniGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT KullaniciAdi, Hakkimda FROM Kullanici WHERE ID=@id", con);
                cmd.Parameters.AddWithValue("@id", uid);

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
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                lblTakipci.Text = new SqlCommand(
                    "SELECT COUNT(*) FROM Takip WHERE TakipEdilenID=@id", con)
                { Parameters = { new SqlParameter("@id", uid) } }
                .ExecuteScalar().ToString();

                lblTakipEdilen.Text = new SqlCommand(
                    "SELECT COUNT(*) FROM Takip WHERE TakipEdenID=@id", con)
                { Parameters = { new SqlParameter("@id", uid) } }
                .ExecuteScalar().ToString();
            }
        }

        void IzlenenFilmlerMiniGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TOP 6 FilmID, PosterPath FROM IzlenenFilmler WHERE KullaniciID=@id ORDER BY IzlenmeTarihi DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@id", uid);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptIzlenenFilmlerMini.DataSource = dt;
                rptIzlenenFilmlerMini.DataBind();
            }
        }

        void IzlenenDizilerMiniGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TOP 6 DiziID, PosterPath FROM IzlenenDiziler WHERE KullaniciID=@id ORDER BY IzlenmeTarihi DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@id", uid);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptIzlenenDizilerMini.DataSource = dt;
                rptIzlenenDizilerMini.DataBind();
            }
        }
        void ListelerMiniGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT ID, ListeAdi
            FROM Listeler
            WHERE KullaniciID = @uid
              AND (Gorunurluk IS NULL OR Gorunurluk = 'Public')
            ORDER BY Tarih DESC", con);

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
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TOP 5 ID, FilmID, FilmAdi, Yorum FROM Elestiri WHERE KullaniciID=@id ORDER BY Tarih DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@id", uid);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptSonFilmElestiriler.DataSource = dt;
                rptSonFilmElestiriler.DataBind();
            }
        }

        void SonDiziElestirileriGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TOP 5 ID, DiziID, DiziAdi, Yorum FROM DiziElestiri WHERE KullaniciID=@id ORDER BY Tarih DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@id", uid);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptSonDiziElestiriler.DataSource = dt;
                rptSonDiziElestiriler.DataBind();
            }
        }

        void IstatistikleriGetir()
        {
            int filmSayisi = 0, diziSayisi = 0, listeSayisi = 0;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                filmSayisi = Convert.ToInt32(
                    new SqlCommand(
                        "SELECT COUNT(DISTINCT FilmID) FROM IzlenenFilmler WHERE KullaniciID=@uid",
                        con)
                    {
                        Parameters = { new SqlParameter("@uid", uid) }
                    }.ExecuteScalar()
                );

                diziSayisi = Convert.ToInt32(
                    new SqlCommand(
                        "SELECT COUNT(DISTINCT DiziID) FROM IzlenenDiziler WHERE KullaniciID=@uid",
                        con)
                    {
                        Parameters = { new SqlParameter("@uid", uid) }
                    }.ExecuteScalar()
                );

                listeSayisi = Convert.ToInt32(
                    new SqlCommand(
                        "SELECT COUNT(*) FROM Listeler WHERE KullaniciID=@uid",
                        con)
                    {
                        Parameters = { new SqlParameter("@uid", uid) }
                    }.ExecuteScalar()
                );
            }

            lblFilmSayisi.Text = filmSayisi.ToString();
            lblDiziSayisi.Text = diziSayisi.ToString();
            lblListeSayisi.Text = listeSayisi.ToString();

            int toplam = filmSayisi + diziSayisi;

            if (toplam < 5)
                lblRozet.Text = "🐣 Yeni Üye";
            else if (toplam < 15)
                lblRozet.Text = "🌱 Başlayan İzleyici";
            else if (toplam < 30)
                lblRozet.Text = "🎬 Film & Dizi Sever";
            else if (toplam < 60)
                lblRozet.Text = "🍿 Tutkulu İzleyici";
            else if (toplam < 100)
                lblRozet.Text = "🔥 CineScope Bağımlısı";
            else
                lblRozet.Text = "👑 Efsane İzleyici";
        }


        void TakipDurumuGetir()
        {
            if (Session["KullaniciID"] == null) return;

            int benimId = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Takip WHERE TakipEdenID=@b AND TakipEdilenID=@o", con);
                cmd.Parameters.AddWithValue("@b", benimId);
                cmd.Parameters.AddWithValue("@o", uid);

                btnTakip.Text = Convert.ToInt32(cmd.ExecuteScalar()) > 0
                    ? "Takipten Çık"
                    : "Takip Et";
            }
        }
        protected void btnOrtakListe_Click(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null) return;

            int benimId = Convert.ToInt32(Session["KullaniciID"]);

            if (benimId == uid) return;

            Response.Redirect("OrtakListe.aspx?uid=" + uid);
        }

        protected void btnTakip_Click(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null) return;

            int benimId = Convert.ToInt32(Session["KullaniciID"]);

            if (benimId == uid) return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                SqlCommand kontrol = new SqlCommand(
                    "SELECT COUNT(*) FROM Takip WHERE TakipEdenID=@b AND TakipEdilenID=@o", con);
                kontrol.Parameters.AddWithValue("@b", benimId);
                kontrol.Parameters.AddWithValue("@o", uid);

                if (Convert.ToInt32(kontrol.ExecuteScalar()) > 0)
                {
                    SqlCommand sil = new SqlCommand(
                        "DELETE FROM Takip WHERE TakipEdenID=@b AND TakipEdilenID=@o", con);
                    sil.Parameters.AddWithValue("@b", benimId);
                    sil.Parameters.AddWithValue("@o", uid);
                    sil.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand ekle = new SqlCommand(
                        @"INSERT INTO Takip (TakipEdenID, TakipEdilenID, Tarih)
                  VALUES (@b, @o, GETDATE())", con);

                    ekle.Parameters.AddWithValue("@b", benimId);
                    ekle.Parameters.AddWithValue("@o", uid);
                    ekle.ExecuteNonQuery();
                }
            }

            TakipDurumuGetir();
            TakipSayilariGetir();
        }


        protected void btnMesaj_Click(object sender, EventArgs e)
        {
            Response.Redirect("Mesajlar.aspx?id=" + uid);
        }
    }
}
