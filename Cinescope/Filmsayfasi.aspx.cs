using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinescope
{
    public partial class Filmsayfasi : System.Web.UI.Page
    {
        string connStr =
            "Server=(localdb)\\MSSQLLocalDB;Database=CineScopeDB;Trusted_Connection=True;";


        private int GetCurrentUserID()
        {
            if (Session["KullaniciID"] == null)
                return 0;

            return Convert.ToInt32(Session["KullaniciID"]);
        }

        private void SetButtonDone(Button btn, string text)
        {
            if (btn == null) return;

            btn.Text = text;
            btn.Enabled = false;

            if (btn.CssClass == null) btn.CssClass = "";
            btn.CssClass = (btn.CssClass + " btn-active btn-disabled").Trim();
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("Filmler.aspx");

            if (!IsPostBack)
            {
                hfFilmID.Value = Request.QueryString["id"];

                IzlendiDurumunuKontrolEt();
                DahaSonraDurumuKontrolEt();
                FavoriDurumuKontrolEt();

                YorumlariGetir();
                PuanYildizlariniYukle();
            }
        }


        private void IzlendiDurumunuKontrolEt()
        {
            int uid = GetCurrentUserID();
            if (uid == 0) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM IzlenenFilmler WHERE KullaniciID=@u AND FilmID=@f", conn);

                cmd.Parameters.AddWithValue("@u", uid);
                cmd.Parameters.AddWithValue("@f", hfFilmID.Value);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    SetButtonDone(btnIzledim, " İzlendi");
            }
        }

        private void DahaSonraDurumuKontrolEt()
        {
            int uid = GetCurrentUserID();
            if (uid == 0) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM DahaSonraFilmler WHERE KullaniciID=@u AND FilmID=@f", conn);

                cmd.Parameters.AddWithValue("@u", uid);
                cmd.Parameters.AddWithValue("@f", hfFilmID.Value);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    SetButtonDone(btnDahaSonra, " Daha Sonra İzlenecek");
            }
        }

        private void FavoriDurumuKontrolEt()
        {
            int uid = GetCurrentUserID();
            if (uid == 0) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM FavoriFilmler WHERE KullaniciID=@u AND FilmID=@f", conn);

                cmd.Parameters.AddWithValue("@u", uid);
                cmd.Parameters.AddWithValue("@f", hfFilmID.Value);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    SetButtonDone(btnFavori, " Favorilere Eklendi");
            }
        }

     
        protected void btnIzledim_Click(object sender, EventArgs e)
        {
            FilmEkle("IzlenenFilmler", "IzlenmeTarihi");
            IzlendiDurumunuKontrolEt();
        }

        protected void btnDahaSonra_Click(object sender, EventArgs e)
        {
            FilmEkle("DahaSonraFilmler", "Tarih");
            DahaSonraDurumuKontrolEt(); 
        }

        protected void btnFavori_Click(object sender, EventArgs e)
        {
            FilmEkle("FavoriFilmler", "Tarih");
            FavoriDurumuKontrolEt(); 
        }

        private void FilmEkle(string tablo, string tarihKolon)
        {
            int uid = GetCurrentUserID();
            if (uid == 0)
            {
                if (lblMesaj != null) lblMesaj.Text = "Giriş yapmalısın.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($@"
INSERT INTO {tablo}
(KullaniciID, FilmID, FilmAdi, PosterPath, {tarihKolon})
SELECT @KID, @FID, @FA, @P, GETDATE()
WHERE NOT EXISTS
(SELECT 1 FROM {tablo} WHERE KullaniciID=@KID AND FilmID=@FID)", conn);

                cmd.Parameters.AddWithValue("@KID", uid);
                cmd.Parameters.AddWithValue("@FID", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@FA", hfFilmAdi.Value);
                cmd.Parameters.AddWithValue("@P", hfPoster.Value);

                cmd.ExecuteNonQuery();
            }
        }

        
        private void PuanYildizlariniYukle()
        {
            int userId = GetCurrentUserID();
            if (userId == 0) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Puan FROM IzlenenFilmler WHERE KullaniciID=@KID AND FilmID=@FID", conn);

                cmd.Parameters.AddWithValue("@KID", userId);
                cmd.Parameters.AddWithValue("@FID", hfFilmID.Value);

                object r = cmd.ExecuteScalar();
                if (r != null && r != DBNull.Value)
                    hfPuan.Value = r.ToString();
            }
        }

        protected void btnPuanKaydet_Click(object sender, EventArgs e)
        {
            int userId = GetCurrentUserID();
            if (userId == 0) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM IzlenenFilmler WHERE KullaniciID=@KID AND FilmID=@FID)
    UPDATE IzlenenFilmler SET Puan=@Puan
    WHERE KullaniciID=@KID AND FilmID=@FID
ELSE
    INSERT INTO IzlenenFilmler
    (KullaniciID, FilmID, FilmAdi, PosterPath, Puan, IzlenmeTarihi)
    VALUES (@KID, @FID, @FA, @P, @Puan, GETDATE())", conn);

                cmd.Parameters.AddWithValue("@KID", userId);
                cmd.Parameters.AddWithValue("@FID", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@FA", hfFilmAdi.Value);
                cmd.Parameters.AddWithValue("@P", hfPoster.Value);
                cmd.Parameters.AddWithValue("@Puan", hfPuan.Value);

                cmd.ExecuteNonQuery();
            }

            IzlendiDurumunuKontrolEt();
        }

        
        private void YorumlariGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
SELECT E.ID, E.Yorum, E.Tarih, E.KullaniciID,
       K.KullaniciAdi, E.Begeni, E.Begenmeme
FROM Elestiri E
JOIN Kullanici K ON K.ID = E.KullaniciID
WHERE E.FilmID=@FID
ORDER BY E.ID DESC", conn);

                cmd.Parameters.AddWithValue("@FID", hfFilmID.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptElestiri.DataSource = dt;
                rptElestiri.DataBind();
            }
        }

        protected void btnElestiriGonder_Click(object sender, EventArgs e)
        {
            int userId = GetCurrentUserID();
            if (userId == 0) { if (lblMesaj != null) lblMesaj.Text = "Giriş yapmalısın."; return; }
            if (txtElestiri == null || string.IsNullOrWhiteSpace(txtElestiri.Text)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
INSERT INTO Elestiri
(KullaniciID, FilmID, FilmAdi, Poster, Yorum, Tarih, Begeni, Begenmeme)
VALUES
(@KID, @FID, @FA, @P, @Y, GETDATE(), 0, 0)", conn);

                cmd.Parameters.AddWithValue("@KID", userId);
                cmd.Parameters.AddWithValue("@FID", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@FA", hfFilmAdi.Value);
                cmd.Parameters.AddWithValue("@P", hfPoster.Value);
                cmd.Parameters.AddWithValue("@Y", txtElestiri.Text.Trim());

                cmd.ExecuteNonQuery();
            }

            txtElestiri.Text = "";
            YorumlariGetir();
        }

        protected void rptElestiri_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Begen")
                RunQuery("UPDATE Elestiri SET Begeni = ISNULL(Begeni,0) + 1 WHERE ID=" + id);
            else if (e.CommandName == "Begenme")
                RunQuery("UPDATE Elestiri SET Begenmeme = ISNULL(Begenmeme,0) + 1 WHERE ID=" + id);
            else if (e.CommandName == "SilYorum")
                RunQuery("DELETE FROM Elestiri WHERE ID=" + id);

            YorumlariGetir();
        }

       
        protected void rptElestiri_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (drv == null) return;

                int sahibi = Convert.ToInt32(drv["KullaniciID"]);

                LinkButton sil = (LinkButton)e.Item.FindControl("btnYorumSil");
                if (sil != null)
                    sil.Visible = (GetCurrentUserID() == sahibi);

                Repeater rpt = (Repeater)e.Item.FindControl("rptYanit");
                if (rpt != null)
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(@"
SELECT Y.ID, Y.Yorum, Y.Tarih, Y.KullaniciID,
       K.KullaniciAdi, Y.Begeni, Y.Begenmeme
FROM ElestiriYanitlari Y
JOIN Kullanici K ON K.ID = Y.KullaniciID
WHERE Y.AnaElestiriID=@ID
ORDER BY Y.ID ASC", conn);

                        cmd.Parameters.AddWithValue("@ID", drv["ID"]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        rpt.DataSource = dt;
                        rpt.DataBind();
                    }
                }
            }
        }

      
        protected void btnYanitGonder_Click(object sender, EventArgs e)
        {
            int userId = GetCurrentUserID();
            if (userId == 0) { if (lblMesaj != null) lblMesaj.Text = "Giriş yapmalısın."; return; }

            Button btn = (Button)sender;
            int anaID = Convert.ToInt32(btn.CommandArgument);

            RepeaterItem item = (RepeaterItem)btn.NamingContainer;
            TextBox txt = (TextBox)item.FindControl("txtYanit");
            if (txt == null || string.IsNullOrWhiteSpace(txt.Text)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
INSERT INTO ElestiriYanitlari
(AnaElestiriID, KullaniciID, Yorum, Tarih, Begeni, Begenmeme)
VALUES
(@AID, @KID, @Y, GETDATE(), 0, 0)", conn);

                cmd.Parameters.AddWithValue("@AID", anaID);
                cmd.Parameters.AddWithValue("@KID", userId);
                cmd.Parameters.AddWithValue("@Y", txt.Text.Trim());

                cmd.ExecuteNonQuery();
            }

            YorumlariGetir();
        }


        protected void rptYanit_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "YanitBegen")
                RunQuery("UPDATE ElestiriYanitlari SET Begeni = ISNULL(Begeni,0) + 1 WHERE ID=" + id);
            else if (e.CommandName == "YanitBegenme")
                RunQuery("UPDATE ElestiriYanitlari SET Begenmeme = ISNULL(Begenmeme,0) + 1 WHERE ID=" + id);
            else if (e.CommandName == "SilYanit")
                RunQuery("DELETE FROM ElestiriYanitlari WHERE ID=" + id);

            YorumlariGetir();
        }

        protected void rptYanit_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (drv == null) return;

                LinkButton sil = (LinkButton)e.Item.FindControl("btnYanitSil");
                if (sil != null)
                    sil.Visible = (GetCurrentUserID() == Convert.ToInt32(drv["KullaniciID"]));
            }
        }

        private void RunQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
