using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace Cinescope
{
    public partial class Kayitol : Page
    {
       
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnKayitOl_Click(object sender, EventArgs e)
        {
           
            string isim = txtIsim.Text.Trim();
            string email = txtEmail.Text.Trim();
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();
            string sifreTekrar = txtSifreTekrar.Text.Trim();

            
            if (string.IsNullOrEmpty(isim) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                lblMesaj.Text = "Lütfen tüm alanları doldurun.";
                lblMesaj.CssClass = "text-danger mt-3 d-block text-center";
                return;
            }

            
            if (sifre != sifreTekrar)
            {
                lblMesaj.Text = "Şifreler uyuşmuyor.";
                lblMesaj.CssClass = "text-danger mt-3 d-block text-center";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    
                    string sql = "INSERT INTO Kullanici (Isim, Email, KullaniciAdi, Sifre) VALUES (@Isim, @Email, @KullaniciAdi, @Sifre)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Isim", isim);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                        cmd.Parameters.AddWithValue("@Sifre", sifre);

                        cmd.ExecuteNonQuery();
                    }
                }

                
                lblMesaj.Text = "Kayıt başarılı! Giriş yapabilirsiniz.";
                lblMesaj.CssClass = "text-success mt-3 d-block text-center";

               
                txtIsim.Text = "";
                txtEmail.Text = "";
                txtKullaniciAdi.Text = "";
                txtSifre.Text = "";
                txtSifreTekrar.Text = "";

                Response.Redirect("GirisYap.aspx");
            }
            catch (Exception ex)
            {

                lblMesaj.Text = "Hata: " + ex.Message;
                lblMesaj.CssClass = "text-danger mt-3 d-block text-center";
            }
        }
    }
}
