using System;
using System.Data.SqlClient;

namespace Cinescope
{
    public partial class HesapAyarlari : System.Web.UI.Page
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
                LoadKullanici();
            }
        }

        
        private void LoadKullanici()
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection conn = new SqlConnection(BaglantiDizesi))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
SELECT KullaniciAdi, Hakkimda, HesapGorunurluk
FROM Kullanici
WHERE ID = @id", conn);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtKullaniciAdi.Text = dr["KullaniciAdi"].ToString();
                    txtHakkimda.Text = dr["Hakkimda"].ToString();
                    ddlGorunurluk.SelectedValue =
                        dr["HesapGorunurluk"].ToString();
                }
            }
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection conn = new SqlConnection(BaglantiDizesi))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
UPDATE Kullanici
SET
    KullaniciAdi = @k,
    Hakkimda = @h,
    HesapGorunurluk = @g
WHERE ID = @id", conn);

                cmd.Parameters.AddWithValue("@k", txtKullaniciAdi.Text.Trim());
                cmd.Parameters.AddWithValue("@h", txtHakkimda.Text.Trim());
                cmd.Parameters.AddWithValue("@g", ddlGorunurluk.SelectedValue);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

                lblMesaj.Text = "Bilgiler başarıyla güncellendi!";
            }
        }
    }
}
