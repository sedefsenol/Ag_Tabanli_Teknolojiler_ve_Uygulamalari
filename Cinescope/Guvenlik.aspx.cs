using System;
using System.Data.SqlClient;

namespace Cinescope
{
    public partial class Guvenlik : System.Web.UI.Page
    {
        private const string BaglantiDizesi =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CineScopeDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null)
            {
                Response.Redirect("Girisyap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                EmailGetir();
            }
        }

        private void EmailGetir()
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection conn = new SqlConnection(BaglantiDizesi))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Email FROM Kullanici WHERE ID=@id", conn);

                cmd.Parameters.AddWithValue("@id", id);
                txtEmail.Text = cmd.ExecuteScalar()?.ToString();
            }
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);

            bool sifreDegisiyor =
                !string.IsNullOrWhiteSpace(txtYeni.Text) ||
                !string.IsNullOrWhiteSpace(txtYeniTekrar.Text);

            using (SqlConnection conn = new SqlConnection(BaglantiDizesi))
            {
                conn.Open();

                
                if (!sifreDegisiyor)
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Kullanici SET Email=@e WHERE ID=@id", conn);

                    cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    lblMesaj.Text = "E-posta adresi güncellendi.";
                    lblMesaj.ForeColor = System.Drawing.Color.LightGreen;
                    return;
                }

                if (txtYeni.Text != txtYeniTekrar.Text)
                {
                    lblMesaj.Text = "Yeni şifreler uyuşmuyor.";
                    lblMesaj.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                SqlCommand kontrol = new SqlCommand(
                    "SELECT Sifre FROM Kullanici WHERE ID=@id", conn);

                kontrol.Parameters.AddWithValue("@id", id);
                string dbSifre = kontrol.ExecuteScalar()?.ToString();

                if (dbSifre != txtMevcut.Text)
                {
                    lblMesaj.Text = "Mevcut şifre yanlış.";
                    lblMesaj.ForeColor = System.Drawing.Color.Red;
                    return;
                }

             
                SqlCommand guncelle = new SqlCommand(@"
UPDATE Kullanici
SET
    Email = @e,
    Sifre = @s
WHERE ID = @id", conn);

                guncelle.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                guncelle.Parameters.AddWithValue("@s", txtYeni.Text.Trim());
                guncelle.Parameters.AddWithValue("@id", id);
                guncelle.ExecuteNonQuery();

                lblMesaj.Text = "E-posta ve şifre güncellendi.";
                lblMesaj.ForeColor = System.Drawing.Color.FromArgb(255, 179, 217); // #ffb3d9
            }
        }
    }
}
