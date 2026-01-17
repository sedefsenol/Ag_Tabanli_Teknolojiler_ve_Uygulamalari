using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class Mesajlar : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;
        int benimID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null)
                Response.Redirect("Girisyap.aspx");

            benimID = Convert.ToInt32(Session["KullaniciID"]);

            if (!IsPostBack)
            {
                KonusmalariGetir();
                IstekleriGetir();
            }

            if (Request.QueryString["uid"] != null)
            {
                int hedefId = Convert.ToInt32(Request.QueryString["uid"]);
                SohbetiAc(hedefId);
            }
            else
            {
                pnlBos.Visible = true;
                pnlSohbet.Visible = false;
            }
        }

        void KonusmalariGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT DISTINCT 
                        K.ID, K.KullaniciAdi, K.ProfilFoto
                    FROM Mesajlar M
                    JOIN Kullanici K
                      ON (K.ID = M.GonderenID OR K.ID = M.AliciID)
                    WHERE (M.GonderenID = @id OR M.AliciID = @id)
                      AND K.ID <> @id
                    ORDER BY K.KullaniciAdi
                ", conn);

                cmd.Parameters.AddWithValue("@id", benimID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptKonusmalar.DataSource = dt;
                rptKonusmalar.DataBind();
            }
        }

        void IstekleriGetir()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT MI.GonderenID,
                           K.KullaniciAdi,
                           K.ProfilFoto
                    FROM MesajIstekleri MI
                    JOIN Kullanici K ON K.ID = MI.GonderenID
                    WHERE MI.AliciID = @id
                      AND MI.Durum = 0
                    GROUP BY MI.GonderenID, K.KullaniciAdi, K.ProfilFoto
                    ORDER BY K.KullaniciAdi
                ", conn);

                cmd.Parameters.AddWithValue("@id", benimID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptIstekler.DataSource = dt;
                rptIstekler.DataBind();

                lblIstekSayisi.Text = dt.Rows.Count.ToString();
            }
        }

        void SohbetiAc(int hedefId)
        {
            pnlBos.Visible = false;
            pnlSohbet.Visible = true;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT KullaniciAdi, ProfilFoto FROM Kullanici WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", hedefId);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                   
                    lblSohbetBaslik.Text = dr["KullaniciAdi"].ToString();

                    imgHedef.ImageUrl = dr["ProfilFoto"].ToString();
                }
                dr.Close();
            }

            if (MesajlasmaVarMi(hedefId))
            {
                pnlIstekOnay.Visible = false;
                MesajlariGetir(hedefId);
            }
            else if (MesajIstegiVarMi(hedefId))
            {
                pnlIstekOnay.Visible = true;
                MesajIstekMesajlariniGetir(hedefId);
            }
            else
            {
                pnlIstekOnay.Visible = false;
                rptMesajlar.DataSource = null;
                rptMesajlar.DataBind();
            }
        }

        void MesajlariGetir(int hedefId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT GonderenID, AliciID, Mesaj, Tarih
                    FROM Mesajlar
                    WHERE (GonderenID=@b AND AliciID=@h)
                       OR (GonderenID=@h AND AliciID=@b)
                    ORDER BY Tarih
                ", conn);

                cmd.Parameters.AddWithValue("@b", benimID);
                cmd.Parameters.AddWithValue("@h", hedefId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptMesajlar.DataSource = dt;
                rptMesajlar.DataBind();
            }
        }

        void MesajIstekMesajlariniGetir(int hedefId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT GonderenID, Mesaj, Tarih
                    FROM MesajIstekleri
                    WHERE Durum = 0
                      AND (
                            (GonderenID=@b AND AliciID=@h)
                         OR (GonderenID=@h AND AliciID=@b)
                      )
                    ORDER BY Tarih
                ", conn);

                cmd.Parameters.AddWithValue("@b", benimID);
                cmd.Parameters.AddWithValue("@h", hedefId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptMesajlar.DataSource = dt;
                rptMesajlar.DataBind();
            }
        }

        protected void btnGonder_Click(object sender, EventArgs e)
        {
            if (txtMesaj.Text.Trim() == "") return;
            if (Request.QueryString["uid"] == null) return;

            int hedefId = Convert.ToInt32(Request.QueryString["uid"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (MesajlasmaVarMi(hedefId))
                    cmd = new SqlCommand("INSERT INTO Mesajlar (GonderenID,AliciID,Mesaj,Tarih) VALUES(@g,@a,@m,GETDATE())", conn);
                else
                    cmd = new SqlCommand("INSERT INTO MesajIstekleri (GonderenID,AliciID,Mesaj,Tarih,Durum) VALUES(@g,@a,@m,GETDATE(),0)", conn);

                cmd.Parameters.AddWithValue("@g", benimID);
                cmd.Parameters.AddWithValue("@a", hedefId);
                cmd.Parameters.AddWithValue("@m", txtMesaj.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtMesaj.Text = "";
            KonusmalariGetir();
            IstekleriGetir();
            SohbetiAc(hedefId);
        }

        bool MesajlasmaVarMi(int hedefId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(*) FROM Mesajlar
                    WHERE (GonderenID=@b AND AliciID=@h)
                       OR (GonderenID=@h AND AliciID=@b)
                ", conn);

                cmd.Parameters.AddWithValue("@b", benimID);
                cmd.Parameters.AddWithValue("@h", hedefId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        bool MesajIstegiVarMi(int hedefId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(*) FROM MesajIstekleri
                    WHERE GonderenID=@h AND AliciID=@b AND Durum=0
                ", conn);

                cmd.Parameters.AddWithValue("@h", hedefId);
                cmd.Parameters.AddWithValue("@b", benimID);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        protected void btnIstekKabul_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["uid"] == null) return;
            int hedefId = Convert.ToInt32(Request.QueryString["uid"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand insert = new SqlCommand(@"
                    INSERT INTO Mesajlar (GonderenID,AliciID,Mesaj,Tarih)
                    SELECT GonderenID,AliciID,Mesaj,Tarih
                    FROM MesajIstekleri
                    WHERE (
                            (GonderenID=@h AND AliciID=@b)
                         OR (GonderenID=@b AND AliciID=@h)
                          )
                      AND Durum=0
                ", conn);
                insert.Parameters.AddWithValue("@h", hedefId);
                insert.Parameters.AddWithValue("@b", benimID);
                insert.ExecuteNonQuery();

                SqlCommand upd = new SqlCommand(@"
                    UPDATE MesajIstekleri SET Durum=1
                    WHERE (
                            (GonderenID=@h AND AliciID=@b)
                         OR (GonderenID=@b AND AliciID=@h)
                          )
                      AND Durum=0
                ", conn);
                upd.Parameters.AddWithValue("@h", hedefId);
                upd.Parameters.AddWithValue("@b", benimID);
                upd.ExecuteNonQuery();
            }

            KonusmalariGetir();
            IstekleriGetir();
            SohbetiAc(hedefId);
        }

        protected void btnIstekRed_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["uid"] == null) return;
            int hedefId = Convert.ToInt32(Request.QueryString["uid"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE MesajIstekleri SET Durum=2
                    WHERE GonderenID=@h AND AliciID=@b AND Durum=0
                ", conn);

                cmd.Parameters.AddWithValue("@h", hedefId);
                cmd.Parameters.AddWithValue("@b", benimID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            KonusmalariGetir();
            IstekleriGetir();
            Response.Redirect("Mesajlar.aspx");
        }
    }
}
