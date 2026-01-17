using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Cinescope
{
    public partial class Anasayfa : System.Web.UI.Page
    {
        string connStr = ConfigurationManager
            .ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulerDiziElestirileriGetir();
                PopulerFilmElestirileriGetir();
                PopulerKullanicilariGetir();   
                OnerilenKullanicilariGetir();  
            }
        }

        void PopulerDiziElestirileriGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT TOP 5 DiziID, DiziAdi, Poster, Yorum
                    FROM DiziElestiri
                    ORDER BY Begeni DESC, Tarih DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptDiziElestiri.DataSource = dt;
                rptDiziElestiri.DataBind();
            }
        }

        void PopulerFilmElestirileriGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT TOP 5 FilmID, FilmAdi, Poster, Yorum
                    FROM Elestiri
                    ORDER BY Begeni DESC, Tarih DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptFilmElestiri.DataSource = dt;
                rptFilmElestiri.DataBind();
            }
        }

        void PopulerKullanicilariGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT TOP 5
                        K.ID,
                        K.KullaniciAdi,
                        ISNULL(K.ProfilFoto,'Gorsel/pp.png') AS ProfilFoto,
                        COUNT(T.ID) AS TakipciSayisi
                    FROM Kullanici K
                    LEFT JOIN Takip T ON T.TakipEdilenID = K.ID
                    GROUP BY K.ID, K.KullaniciAdi, K.ProfilFoto
                    ORDER BY TakipciSayisi DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptPopulerKullanicilar.DataSource = dt;
                rptPopulerKullanicilar.DataBind();
            }
        }

        void OnerilenKullanicilariGetir()
        {
            if (Session["KullaniciID"] == null)
            {
                rptOnerilenler.DataSource = null;
                rptOnerilenler.DataBind();
                return;
            }

            int benimID = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    ;WITH OrtakFilm AS (
                        SELECT B.KullaniciID, COUNT(*) AS OrtakFilm
                        FROM IzlenenFilmler A
                        JOIN IzlenenFilmler B ON A.FilmID=B.FilmID
                        WHERE A.KullaniciID=@BenimID AND B.KullaniciID<>@BenimID
                        GROUP BY B.KullaniciID
                    ),
                    OrtakDizi AS (
                        SELECT B.KullaniciID, COUNT(*) AS OrtakDizi
                        FROM IzlenenDiziler A
                        JOIN IzlenenDiziler B ON A.DiziID=B.DiziID
                        WHERE A.KullaniciID=@BenimID AND B.KullaniciID<>@BenimID
                        GROUP BY B.KullaniciID
                    )
                    SELECT TOP 5
                        K.ID,
                        K.KullaniciAdi,
                        ISNULL(K.ProfilFoto,'Gorsel/pp.png') AS ProfilFoto,
                        CASE
                            WHEN F.OrtakFilm IS NOT NULL AND D.OrtakDizi IS NOT NULL
                                THEN 'Film ve dizi zevkleriniz benziyor'
                            WHEN F.OrtakFilm IS NOT NULL
                                THEN 'Film zevkiniz benziyor'
                            WHEN D.OrtakDizi IS NOT NULL
                                THEN 'Dizi zevkiniz benziyor'
                            ELSE 'İlginizi çekebilir'
                        END AS Neden
                    FROM Kullanici K
                    LEFT JOIN OrtakFilm F ON F.KullaniciID=K.ID
                    LEFT JOIN OrtakDizi D ON D.KullaniciID=K.ID
                    LEFT JOIN Takip T ON T.TakipEdenID=@BenimID AND T.TakipEdilenID=K.ID
                    WHERE K.ID<>@BenimID           
                      AND T.ID IS NULL
                      AND (F.OrtakFilm IS NOT NULL OR D.OrtakDizi IS NOT NULL)
                    ORDER BY (ISNULL(F.OrtakFilm,0)*2 + ISNULL(D.OrtakDizi,0)) DESC", con);

                da.SelectCommand.Parameters.AddWithValue("@BenimID", benimID);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptOnerilenler.DataSource = dt;
                rptOnerilenler.DataBind();
            }
        }

        protected string GetUserProfileUrl(object userIdObj)
        {
            if (Session["KullaniciID"] == null)
                return "Kullanici.aspx?id=" + userIdObj.ToString();

            int benimId = Convert.ToInt32(Session["KullaniciID"]);
            int hedefId = Convert.ToInt32(userIdObj);

            if (benimId == hedefId)
                return "Hesap.aspx";

            return "Kullanici.aspx?id=" + hedefId;
        }
    }
}
