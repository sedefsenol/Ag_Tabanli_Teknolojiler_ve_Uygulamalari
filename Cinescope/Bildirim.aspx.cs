using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class Bildirim : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null)
            {
                Response.Redirect("girisyap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BildirimleriOkunduYap();
                BildirimleriGetir();
                OnerilenleriGetir();
            }
        }

        void BildirimleriGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
SELECT * FROM (

   
    SELECT 
        CAST(B.Okundu AS INT) AS Okundu,
        B.Mesaj,
        K.KullaniciAdi AS GonderenAdi,
        B.OlusturmaTarihi AS Tarih,
        CASE 
            WHEN B.BildirimTuru = 'LIKE_REVIEW' THEN 'fa-heart'
            WHEN B.BildirimTuru = 'REPLY_REVIEW' THEN 'fa-comment'
            ELSE 'fa-bell'
        END AS IconClass,
        'Elestirisayfasi.aspx?filmId=' + CAST(E.FilmID AS NVARCHAR)
            + '&elestiriId=' + CAST(E.ID AS NVARCHAR) AS YonlendirmeUrl
    FROM Bildirimler B
    JOIN Kullanici K ON K.ID = B.GonderenKullaniciID
    LEFT JOIN Elestiri E ON E.ID = B.IlgiliID
    WHERE 
        B.AlanKullaniciID = @uid 
        AND B.BildirimTuru IN ('LIKE_REVIEW', 'REPLY_REVIEW')

    UNION ALL

    
    SELECT 
        1 AS Okundu,
        'sana mesaj göndermek istiyor' AS Mesaj,
        K.KullaniciAdi AS GonderenAdi,
        MI.Tarih AS Tarih,
        'fa-paper-plane' AS IconClass,
        'Mesajlar.aspx?uid=' + CAST(MI.GonderenID AS NVARCHAR) AS YonlendirmeUrl
    FROM MesajIstekleri MI
    JOIN Kullanici K ON K.ID = MI.GonderenID
    WHERE MI.AliciID = @uid AND MI.Durum = 0

    UNION ALL

    
    SELECT 
        1 AS Okundu,
        'seni takip etti, sen de takip etmek ister misin?' AS Mesaj,
        K.KullaniciAdi AS GonderenAdi,
        T.Tarih AS Tarih,
        'fa-user-plus' AS IconClass,
        'Kullanici.aspx?id=' + CAST(T.TakipEdenID AS NVARCHAR) AS YonlendirmeUrl
    FROM Takip T
    JOIN Kullanici K ON K.ID = T.TakipEdenID
    WHERE T.TakipEdilenID = @uid

    UNION ALL

   
    SELECT 
        1 AS Okundu,
        'bu film çok eleştiriliyor sen de bak: ' + FilmAdi AS Mesaj,
        'Trend' AS GonderenAdi,
        MAX(Tarih) AS Tarih,
        'fa-fire' AS IconClass,
        'Elestirisayfasi.aspx?filmId=' + CAST(FilmID AS NVARCHAR) AS YonlendirmeUrl
    FROM Elestiri
    WHERE Tarih >= DATEADD(hour, -24, GETDATE())
    GROUP BY FilmID, FilmAdi
    HAVING COUNT(*) >= 10

    UNION ALL

    
    SELECT 
        CAST(B.Okundu AS INT) AS Okundu,
        B.Mesaj,
        K.KullaniciAdi AS GonderenAdi,
        B.OlusturmaTarihi AS Tarih,
        'fa-users' AS IconClass,
        'OrtakListeDuzenle.aspx?id=' + CAST(B.IlgiliID AS NVARCHAR) AS YonlendirmeUrl
    FROM Bildirimler B
    JOIN Kullanici K ON K.ID = B.GonderenKullaniciID
    WHERE 
        B.AlanKullaniciID = @uid
        AND B.BildirimTuru = 'ORTAK_LISTE'

) AS Result
ORDER BY Tarih DESC
", con);

                cmd.Parameters.AddWithValue("@uid", Session["KullaniciID"]);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                rptBildirimler.DataSource = dt;
                rptBildirimler.DataBind();
                pnlBosBildirim.Visible = (dt.Rows.Count == 0);
            }
        }

        void BildirimleriOkunduYap()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Bildirimler SET Okundu = 1 WHERE AlanKullaniciID = @uid", con);

                cmd.Parameters.AddWithValue("@uid", Session["KullaniciID"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        void OnerilenleriGetir()
        {
            int benimID = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
;WITH OrtakFilm AS (
    SELECT B.KullaniciID, COUNT(*) AS OrtakSayi
    FROM IzlenenFilmler A
    JOIN IzlenenFilmler B ON A.FilmID = B.FilmID
    WHERE A.KullaniciID = @BenimID AND B.KullaniciID <> @BenimID
    GROUP BY B.KullaniciID
)
SELECT TOP 8
    K.ID,
    K.KullaniciAdi,
    ISNULL(K.ProfilFoto, 'Gorsel/pp.png') AS ProfilFoto,
    'Film zevkiniz uyuşuyor!' AS Neden
FROM Kullanici K
JOIN OrtakFilm O ON O.KullaniciID = K.ID
LEFT JOIN Takip T ON T.TakipEdenID = @BenimID AND T.TakipEdilenID = K.ID
WHERE T.ID IS NULL AND K.ID <> @BenimID
ORDER BY O.OrtakSayi DESC
", con);

                cmd.Parameters.AddWithValue("@BenimID", benimID);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                rptOnerilenler.DataSource = dt;
                rptOnerilenler.DataBind();
                pnlBosOneri.Visible = (dt.Rows.Count == 0);
            }
        }
    }
}
