using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinescope
{
    public partial class Dizisayfasi : Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        private int CurrentUserId
        {
            get
            {
                if (Session["KullaniciID"] == null) return 0;
                return Convert.ToInt32(Session["KullaniciID"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("Diziler.aspx");

            if (!IsPostBack)
            {
                hfFilmID.Value = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(Request.QueryString["title"])) hfFilmAdi.Value = Request.QueryString["title"];
                if (!string.IsNullOrEmpty(Request.QueryString["poster"])) hfPoster.Value = Request.QueryString["poster"];

                DurumlariKontrolEt();
                ElestirileriYukle();
                PuanimiGetir();
            }
        }

      
        private void DurumlariKontrolEt()
        {
            SetState(btnIzledim, "IzlenenDiziler", "✔ İzlendi");
            SetState(btnDahaSonra, "DahaSonraDiziler", "⏰ Daha Sonra İzlenecek");
            SetState(btnFavori, "FavoriDiziler", "❤️ Favorilere Eklendi");
        }

        private void SetState(Button btn, string table, string text)
        {
            if (CurrentUserId == 0) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    $"SELECT COUNT(*) FROM {table} WHERE KullaniciID=@K AND DiziID=@D", conn);

                cmd.Parameters.AddWithValue("@K", CurrentUserId);
                cmd.Parameters.AddWithValue("@D", hfFilmID.Value);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                {
                    btn.Text = text;
                    btn.Enabled = false;
                    btn.CssClass = "filmsayfasi-btn active";
                }
                else
                {
                    btn.CssClass = "filmsayfasi-btn";
                }
            }
        }

        protected void btnIzledim_Click(object sender, EventArgs e)
        {
            Ekle("IzlenenDiziler", "IzlenmeTarihi");
            DurumlariKontrolEt();
        }

        protected void btnDahaSonra_Click(object sender, EventArgs e)
        {
            Ekle("DahaSonraDiziler", "Tarih");
            DurumlariKontrolEt();
        }

        protected void btnFavori_Click(object sender, EventArgs e)
        {
            Ekle("FavoriDiziler", "Tarih");
            DurumlariKontrolEt();
        }

        private void Ekle(string tablo, string tarihKolon)
        {
            if (CurrentUserId == 0) { lblMesaj.Text = "Giriş yapmalısın."; return; }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($@"
INSERT INTO {tablo}
(KullaniciID, DiziID, DiziAdi, PosterPath, {tarihKolon})
SELECT @K, @D, @AD, @P, GETDATE()
WHERE NOT EXISTS (
    SELECT 1 FROM {tablo} WHERE KullaniciID=@K AND DiziID=@D
)", conn);

                cmd.Parameters.AddWithValue("@K", CurrentUserId);
                cmd.Parameters.AddWithValue("@D", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@AD", hfFilmAdi.Value ?? "");
                cmd.Parameters.AddWithValue("@P", hfPoster.Value ?? "");

                cmd.ExecuteNonQuery();
            }
        }

   
        protected void btnPuanKaydet_Click(object sender, EventArgs e)
        {
            if (CurrentUserId == 0) { lblMesaj.Text = "Giriş yapmalısın."; return; }

            int puan;
            int.TryParse(hfPuan.Value, out puan);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM IzlenenDiziler WHERE KullaniciID=@K AND DiziID=@D)
    UPDATE IzlenenDiziler SET Puan=@P WHERE KullaniciID=@K AND DiziID=@D
ELSE
    INSERT INTO IzlenenDiziler
    (KullaniciID, DiziID, DiziAdi, PosterPath, Puan, IzlenmeTarihi)
    VALUES (@K, @D, @AD, @PO, @P, GETDATE())", conn);

                cmd.Parameters.AddWithValue("@K", CurrentUserId);
                cmd.Parameters.AddWithValue("@D", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@AD", hfFilmAdi.Value ?? "");
                cmd.Parameters.AddWithValue("@PO", hfPoster.Value ?? "");
                cmd.Parameters.AddWithValue("@P", puan);

                cmd.ExecuteNonQuery();
            }

            DurumlariKontrolEt();
        }

        private void PuanimiGetir()
        {
            if (CurrentUserId == 0) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Puan FROM IzlenenDiziler WHERE KullaniciID=@K AND DiziID=@D", conn);

                cmd.Parameters.AddWithValue("@K", CurrentUserId);
                cmd.Parameters.AddWithValue("@D", hfFilmID.Value);

                object r = cmd.ExecuteScalar();
                if (r != null && r != DBNull.Value)
                    hfPuan.Value = r.ToString();
            }
        }

        
        private void ElestirileriYukle()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
SELECT E.ID, E.KullaniciID, E.DiziID, E.Yorum, E.Tarih, E.Begeni, E.Begenmeme,
       K.KullaniciAdi
FROM DiziElestiri E
JOIN Kullanici K ON K.ID = E.KullaniciID
WHERE E.DiziID=@D
ORDER BY E.Tarih DESC", conn);

                cmd.Parameters.AddWithValue("@D", hfFilmID.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptElestiri.DataSource = dt;
                rptElestiri.DataBind();
            }
        }

        protected void btnElestiriGonder_Click(object sender, EventArgs e)
        {
            if (CurrentUserId == 0) { lblMesaj.Text = "Giriş yapmalısın."; return; }
            if (string.IsNullOrWhiteSpace(txtElestiri.Text)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
INSERT INTO DiziElestiri
(KullaniciID, DiziID, Yorum, Tarih, Begeni, Begenmeme, DiziAdi, Poster)
VALUES (@K,@D,@Y,GETDATE(),0,0,@AD,@P)", conn);

                cmd.Parameters.AddWithValue("@K", CurrentUserId);
                cmd.Parameters.AddWithValue("@D", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@Y", txtElestiri.Text.Trim());
                cmd.Parameters.AddWithValue("@AD", hfFilmAdi.Value ?? "");
                cmd.Parameters.AddWithValue("@P", hfPoster.Value ?? "");

                cmd.ExecuteNonQuery();
            }

            txtElestiri.Text = "";
            ElestirileriYukle();
        }

        protected void rptElestiri_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string sql = "";

            if (e.CommandName == "Begen")
                sql = "UPDATE DiziElestiri SET Begeni = ISNULL(Begeni,0) + 1 WHERE ID=@ID";
            else if (e.CommandName == "Begenme")
                sql = "UPDATE DiziElestiri SET Begenmeme = ISNULL(Begenmeme,0) + 1 WHERE ID=@ID";
            else if (e.CommandName == "SilYorum")
                sql = @"
DELETE FROM DiziElestiriYanitlari WHERE AnaElestiriID=@ID;
DELETE FROM DiziElestiri WHERE ID=@ID;";

            if (sql == "") return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", e.CommandArgument);
                cmd.ExecuteNonQuery();
            }

            ElestirileriYukle();
        }

        protected void rptElestiri_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem) return;

            int ownerId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "KullaniciID"));

           
            LinkButton btnSil = (LinkButton)e.Item.FindControl("btnYorumSil");
            if (btnSil != null)
                btnSil.Visible = (ownerId == CurrentUserId);

            int anaId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ID"));
            Repeater rptYanit = (Repeater)e.Item.FindControl("rptYanit");
            if (rptYanit != null)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(@"
SELECT Y.ID, Y.AnaElestiriID, Y.KullaniciID, Y.Yorum, Y.Tarih, Y.Begeni, Y.Begenmeme,
       K.KullaniciAdi
FROM DiziElestiriYanitlari Y
JOIN Kullanici K ON K.ID = Y.KullaniciID
WHERE Y.AnaElestiriID=@A
ORDER BY Y.Tarih ASC", conn);

                    cmd.Parameters.AddWithValue("@A", anaId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptYanit.DataSource = dt;
                    rptYanit.DataBind();
                }
            }
        }

        protected void btnYanitGonder_Click(object sender, EventArgs e)
        {
            if (CurrentUserId == 0) { lblMesaj.Text = "Giriş yapmalısın."; return; }

            Button btn = (Button)sender;
            int anaId = Convert.ToInt32(btn.CommandArgument);

            RepeaterItem item = (RepeaterItem)btn.NamingContainer;
            TextBox txt = (TextBox)item.FindControl("txtYanit");
            if (txt == null || string.IsNullOrWhiteSpace(txt.Text)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
INSERT INTO DiziElestiriYanitlari
(AnaElestiriID, KullaniciID, Yorum, Tarih, Begeni, Begenmeme)
VALUES (@A,@K,@Y,GETDATE(),0,0)", conn);

                cmd.Parameters.AddWithValue("@A", anaId);
                cmd.Parameters.AddWithValue("@K", CurrentUserId);
                cmd.Parameters.AddWithValue("@Y", txt.Text.Trim());

                cmd.ExecuteNonQuery();
            }

            ElestirileriYukle();
        }

        protected void rptYanit_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem) return;

            int ownerId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "KullaniciID"));
            LinkButton btnSil = (LinkButton)e.Item.FindControl("btnYanitSil");

            if (btnSil != null)
                btnSil.Visible = (ownerId == CurrentUserId);
        }

        [System.Web.Services.WebMethod]
        public static void SezonIzlendi(int diziId, int sezonNo)
        {
            if (HttpContext.Current.Session["KullaniciID"] == null)
                return;

            int kullaniciId = Convert.ToInt32(HttpContext.Current.Session["KullaniciID"]);

            string connStr =
                ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
IF EXISTS (
    SELECT 1 FROM IzlenenDiziSezon
    WHERE KullaniciID=@K AND DiziID=@D AND SezonNo=@S
)
    UPDATE IzlenenDiziSezon
    SET Izlendi = CASE WHEN Izlendi=1 THEN 0 ELSE 1 END,
        IzlenmeTarihi = GETDATE()
    WHERE KullaniciID=@K AND DiziID=@D AND SezonNo=@S
ELSE
    INSERT INTO IzlenenDiziSezon
    (KullaniciID, DiziID, SezonNo, Izlendi)
    VALUES (@K,@D,@S,1)
", conn);

                cmd.Parameters.AddWithValue("@K", kullaniciId);
                cmd.Parameters.AddWithValue("@D", diziId);
                cmd.Parameters.AddWithValue("@S", sezonNo);
                cmd.ExecuteNonQuery();
            }
        }

        protected void rptYanit_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string sql = "";

            if (e.CommandName == "YanitBegen")
                sql = "UPDATE DiziElestiriYanitlari SET Begeni = ISNULL(Begeni,0) + 1 WHERE ID=@ID";
            else if (e.CommandName == "YanitBegenme")
                sql = "UPDATE DiziElestiriYanitlari SET Begenmeme = ISNULL(Begenmeme,0) + 1 WHERE ID=@ID";
            else if (e.CommandName == "SilYanit")
                sql = "DELETE FROM DiziElestiriYanitlari WHERE ID=@ID";

            if (sql == "") return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", e.CommandArgument);
                cmd.ExecuteNonQuery();
            }

            ElestirileriYukle();
        }
    }
}
