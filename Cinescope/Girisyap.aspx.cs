using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace Cinescope
{
    public partial class Girisyap : Page
    {
        private string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnGirisYap_Click(object sender, EventArgs e)
        {
            lblMesaj.CssClass = "text-danger";

            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                lblMesaj.Text = "Lütfen kullanıcı adı ve şifre girin.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT ID, KullaniciAdi FROM Kullanici WHERE KullaniciAdi=@KullaniciAdi AND Sifre=@Sifre";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                        cmd.Parameters.AddWithValue("@Sifre", sifre);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                Session["KullaniciID"] = reader["ID"].ToString();
                                Session["KullaniciAdi"] = reader["KullaniciAdi"].ToString();

                                
                                Response.Redirect("Anasayfa.aspx");
                            }
                            else
                            {
                                lblMesaj.Text = "Kullanıcı adı veya şifre yanlış.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMesaj.Text = "Hata: " + ex.Message;
            }
        }
    }
}
