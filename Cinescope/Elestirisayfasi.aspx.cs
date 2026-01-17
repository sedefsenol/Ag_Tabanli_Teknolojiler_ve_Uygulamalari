using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Cinescope
{
    public partial class Elestirisayfasi : System.Web.UI.Page
    {
        string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=CineScopeDB;Trusted_Connection=True;";

        int AktifKullaniciID => Session["KullaniciID"] == null ? 0 : Convert.ToInt32(Session["KullaniciID"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hfFilmID.Value = Request.QueryString["filmId"];
                hfSecilenElestiriID.Value = Request.QueryString["elestiriId"];

                FilmBilgisiGetir();
                PuanYildizlariniYukle();
                ElestirileriGetir();
            }
        }

        void FilmBilgisiGetir()
        {
            using (SqlConnection c = new SqlConnection(connStr))
            {
                c.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 FilmAdi, Poster FROM Elestiri WHERE FilmID=@fid", c);
                cmd.Parameters.AddWithValue("@fid", hfFilmID.Value);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    hfFilmAdi.Value = dr["FilmAdi"].ToString();
                    hfPoster.Value = dr["Poster"].ToString();
                }
            }
        }

        private void PuanYildizlariniYukle()
        {
            if (AktifKullaniciID == 0) return;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Puan FROM IzlenenFilmler WHERE KullaniciID=@KID AND FilmID=@FilmID", conn);
                cmd.Parameters.AddWithValue("@KID", AktifKullaniciID);
                cmd.Parameters.AddWithValue("@FilmID", hfFilmID.Value);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) hfPuan.Value = result.ToString();
            }
        }

        void ElestirileriGetir()
        {
            using (SqlConnection c = new SqlConnection(connStr))
            {
                c.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT E.ID, E.Yorum, E.Tarih, E.KullaniciID, E.Begeni, E.Begenmeme, K.KullaniciAdi
                    FROM Elestiri E JOIN Kullanici K ON K.ID = E.KullaniciID
                    WHERE E.FilmID = @fid
                    ORDER BY CASE WHEN E.ID = @secID THEN 0 ELSE 1 END, E.Tarih DESC", c);

                cmd.Parameters.AddWithValue("@fid", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@secID", string.IsNullOrEmpty(hfSecilenElestiriID.Value) ? (object)DBNull.Value : hfSecilenElestiriID.Value);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                rptElestiriler.DataSource = dt;
                rptElestiriler.DataBind();
            }
        }

        protected void btnPuanKaydet_Click(object sender, EventArgs e)
        {
            if (AktifKullaniciID == 0) return;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    IF EXISTS (SELECT 1 FROM IzlenenFilmler WHERE KullaniciID=@KID AND FilmID=@FID)
                    UPDATE IzlenenFilmler SET Puan=@Puan WHERE KullaniciID=@KID AND FilmID=@FID
                    ELSE INSERT INTO IzlenenFilmler (KullaniciID, FilmID, FilmAdi, PosterPath, Puan, IzlenmeTarihi)
                    VALUES (@KID, @FID, @FA, @P, @Puan, GETDATE())", conn);
                cmd.Parameters.AddWithValue("@KID", AktifKullaniciID);
                cmd.Parameters.AddWithValue("@FID", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@FA", hfFilmAdi.Value);
                cmd.Parameters.AddWithValue("@P", hfPoster.Value);
                cmd.Parameters.AddWithValue("@Puan", hfPuan.Value);
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnIzledim_Click(object sender, EventArgs e) => RunActionQuery("INSERT INTO IzlenenFilmler (KullaniciID, FilmID, FilmAdi, PosterPath, IzlenmeTarihi) SELECT @KID, @FID, @FA, @P, GETDATE() WHERE NOT EXISTS (SELECT 1 FROM IzlenenFilmler WHERE KullaniciID=@KID AND FilmID=@FID)");
        protected void btnDahaSonra_Click(object sender, EventArgs e) => RunActionQuery("INSERT INTO DahaSonraFilmler (KullaniciID, FilmID, FilmAdi, PosterPath, Tarih) SELECT @KID, @FID, @FA, @P, GETDATE() WHERE NOT EXISTS (SELECT 1 FROM DahaSonraFilmler WHERE KullaniciID=@KID AND FilmID=@FID)");
        protected void btnFavori_Click(object sender, EventArgs e) => RunActionQuery("INSERT INTO FavoriFilmler (KullaniciID, FilmID, FilmAdi, PosterPath, Tarih) SELECT @KID, @FID, @FA, @P, GETDATE() WHERE NOT EXISTS (SELECT 1 FROM FavoriFilmler WHERE KullaniciID=@KID AND FilmID=@FID)");

        private void RunActionQuery(string sql)
        {
            if (AktifKullaniciID == 0) return;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@KID", AktifKullaniciID);
                cmd.Parameters.AddWithValue("@FID", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@FA", hfFilmAdi.Value);
                cmd.Parameters.AddWithValue("@P", hfPoster.Value);
                cmd.ExecuteNonQuery();
            }
        }

        protected void rptElestiri_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int elestiriID = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Begen") ElestiriBegenVeBildirim(elestiriID, AktifKullaniciID);
            else if (e.CommandName == "Begenme") RunQuery("UPDATE Elestiri SET Begenmeme += 1 WHERE ID=@id", elestiriID);
            else if (e.CommandName == "SilYorum") RunQuery("DELETE FROM Elestiri WHERE ID=@id", elestiriID);
            ElestirileriGetir();
        }

        void ElestiriBegenVeBildirim(int elestiriID, int begenenID)
        {
            if (begenenID == 0) return;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"DECLARE @sahibi INT; SELECT @sahibi = KullaniciID FROM Elestiri WHERE ID = @eid;
                    UPDATE Elestiri SET Begeni += 1 WHERE ID = @eid;
                    IF @sahibi IS NOT NULL AND @sahibi <> @begenen
                    INSERT INTO Bildirimler (GonderenKullaniciID, AlanKullaniciID, BildirimTuru, IlgiliID, Mesaj)
                    VALUES (@begenen, @sahibi, 'LIKE_REVIEW', @eid, 'eleştirini beğendi');", conn);
                cmd.Parameters.AddWithValue("@eid", elestiriID);
                cmd.Parameters.AddWithValue("@begenen", begenenID);
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnElestiriGonder_Click(object sender, EventArgs e)
        {
            if (AktifKullaniciID == 0 || string.IsNullOrWhiteSpace(txtElestiri.Text)) return;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Elestiri (KullaniciID, FilmID, FilmAdi, Poster, Yorum, Tarih, Begeni, Begenmeme) VALUES (@KID, @FilmID, @FilmAdi, @Poster, @Yorum, GETDATE(), 0, 0)", conn);
                cmd.Parameters.AddWithValue("@KID", AktifKullaniciID);
                cmd.Parameters.AddWithValue("@FilmID", hfFilmID.Value);
                cmd.Parameters.AddWithValue("@FilmAdi", hfFilmAdi.Value);
                cmd.Parameters.AddWithValue("@Poster", hfPoster.Value);
                cmd.Parameters.AddWithValue("@Yorum", txtElestiri.Text);
                cmd.ExecuteNonQuery();
            }
            txtElestiri.Text = "";
            ElestirileriGetir();
        }

        protected void btnYanitGonder_Click(object sender, EventArgs e)
        {
            if (AktifKullaniciID == 0) return;
            Button btn = (Button)sender;
            int anaID = Convert.ToInt32(btn.CommandArgument);
            TextBox txt = (TextBox)((RepeaterItem)btn.NamingContainer).FindControl("txtYanit");
            if (string.IsNullOrWhiteSpace(txt.Text)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"DECLARE @sahibi INT; SELECT @sahibi = KullaniciID FROM Elestiri WHERE ID = @eid;
                    INSERT INTO ElestiriYanitlari (AnaElestiriID, KullaniciID, Yorum, Tarih, Begeni, Begenmeme) VALUES (@eid, @kid, @yorum, GETDATE(), 0, 0);
                    IF @sahibi IS NOT NULL AND @sahibi <> @kid
                    INSERT INTO Bildirimler (GonderenKullaniciID, AlanKullaniciID, BildirimTuru, IlgiliID, Mesaj) VALUES (@kid, @sahibi, 'REPLY_REVIEW', @eid, 'eleştirine yanıt verdi');", conn);
                cmd.Parameters.AddWithValue("@eid", anaID);
                cmd.Parameters.AddWithValue("@kid", AktifKullaniciID);
                cmd.Parameters.AddWithValue("@yorum", txt.Text);
                cmd.ExecuteNonQuery();
            }
            ElestirileriGetir();
        }

        protected void rptElestiri_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                int yorumSahibi = Convert.ToInt32(drv["KullaniciID"]);
                LinkButton silBtn = (LinkButton)e.Item.FindControl("btnYorumSil");
                if (AktifKullaniciID == yorumSahibi) silBtn.Visible = true;

                Repeater rpt = (Repeater)e.Item.FindControl("rptYanit");
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ID, KullaniciID, Yorum, Tarih, Begeni, Begenmeme, (SELECT KullaniciAdi FROM Kullanici WHERE ID=KullaniciID) AS KullaniciAdi FROM ElestiriYanitlari WHERE AnaElestiriID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(drv["ID"]));
                    DataTable dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    rpt.DataSource = dt;
                    rpt.DataBind();
                }
            }
        }

        protected void rptYanit_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "YanitBegen") RunQuery("UPDATE ElestiriYanitlari SET Begeni += 1 WHERE ID=@id", id);
            else if (e.CommandName == "YanitBegenme") RunQuery("UPDATE ElestiriYanitlari SET Begenmeme += 1 WHERE ID=@id", id);
            else if (e.CommandName == "SilYanit") RunQuery("DELETE FROM ElestiriYanitlari WHERE ID=@id", id);
            ElestirileriGetir();
        }

        protected void rptYanit_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton silBtn = (LinkButton)e.Item.FindControl("btnYanitSil");
                if (AktifKullaniciID == Convert.ToInt32(drv["KullaniciID"])) silBtn.Visible = true;
            }
        }

        void RunQuery(string sql, int id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}