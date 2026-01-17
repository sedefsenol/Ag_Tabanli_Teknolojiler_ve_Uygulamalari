using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Cinescope
{
    public partial class Elestiriler : Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                YukleSonElestiriler();          
                YuklePopulerElestiriler();      
                YuklePopulerKullanicilar();     
                YuklePopulerFilmElestirileri(); 
                YuklePopulerDiziElestirileri(); 
            }
        }

        private void YukleSonElestiriler()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(@"
                SELECT TOP 5 *
                FROM (
                    SELECT
                        'Film' AS Tur,
                        E.ID AS ElestiriID,
                        E.KullaniciID,
                        E.FilmID,
                        NULL AS DiziID,
                        E.FilmAdi AS Baslik,
                        E.Yorum,
                        E.Tarih,
                        E.Poster,
                        K.KullaniciAdi,
                        K.ProfilFoto,
                        E.Begeni
                    FROM Elestiri E
                    INNER JOIN Kullanici K ON K.ID = E.KullaniciID

                    UNION ALL

                    SELECT
                        'Dizi' AS Tur,
                        D.ID AS ElestiriID,
                        D.KullaniciID,
                        NULL,
                        D.DiziID,
                        D.DiziAdi,
                        D.Yorum,
                        D.Tarih,
                        D.Poster,
                        K.KullaniciAdi,
                        K.ProfilFoto,
                        D.Begeni
                    FROM DiziElestiri D
                    INNER JOIN Kullanici K ON K.ID = D.KullaniciID
                ) X
                ORDER BY Tarih DESC
            ", conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptSonElestiriler.DataSource = dt;
                rptSonElestiriler.DataBind();
            }
        }

        private void YuklePopulerElestiriler()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(@"
                SELECT TOP 5 *
                FROM (
                    SELECT
                        'Film' AS Tur,
                        E.ID AS ElestiriID,
                        E.KullaniciID,
                        E.FilmID,
                        NULL AS DiziID,
                        E.FilmAdi AS Baslik,
                        E.Yorum,
                        E.Tarih,
                        E.Poster,
                        K.KullaniciAdi,
                        K.ProfilFoto,
                        E.Begeni
                    FROM Elestiri E
                    INNER JOIN Kullanici K ON K.ID = E.KullaniciID

                    UNION ALL

                    SELECT
                        'Dizi',
                        D.ID,
                        D.KullaniciID,
                        NULL,
                        D.DiziID,
                        D.DiziAdi,
                        D.Yorum,
                        D.Tarih,
                        D.Poster,
                        K.KullaniciAdi,
                        K.ProfilFoto,
                        D.Begeni
                    FROM DiziElestiri D
                    INNER JOIN Kullanici K ON K.ID = D.KullaniciID
                ) X
                ORDER BY Begeni DESC, Tarih DESC
            ", conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptPopulerElestiriler.DataSource = dt;
                rptPopulerElestiriler.DataBind();
            }
        }


        private void YuklePopulerKullanicilar()
        {
            if (Session["KullaniciID"] == null) return;
            int myId = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(@"
                SELECT TOP 5 ID, KullaniciAdi, ProfilFoto
                FROM Kullanici
                WHERE ID <> @ID
                ORDER BY ID DESC
            ", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@ID", myId);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptPopulerKullanicilar.DataSource = dt;
                rptPopulerKullanicilar.DataBind();
            }
        }

       
        private void YuklePopulerFilmElestirileri()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(@"
                SELECT TOP 3 FilmID, FilmAdi, Yorum, Poster
                FROM Elestiri
                ORDER BY Begeni DESC
            ", conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptPopulerFilmElestiri.DataSource = dt;
                rptPopulerFilmElestiri.DataBind();
            }
        }

       
        private void YuklePopulerDiziElestirileri()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(@"
                SELECT TOP 3 DiziID, DiziAdi, Yorum, Poster
                FROM DiziElestiri
                ORDER BY Begeni DESC
            ", conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptPopulerDiziElestiri.DataSource = dt;
                rptPopulerDiziElestiri.DataBind();
            }
        }
    }
}
