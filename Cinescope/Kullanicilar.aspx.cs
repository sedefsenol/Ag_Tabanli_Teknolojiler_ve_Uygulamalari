using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Cinescope
{
    public partial class Kullanicilar : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                KullaniciListesiniYukle();
            }
        }

        private void KullaniciListesiniYukle()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlSorgu = "SELECT id, username, email, role, created_at FROM users";

                using (SqlCommand cmd = new SqlCommand(sqlSorgu, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        GridViewKullanicilar.DataSource = dt;
                        GridViewKullanicilar.DataBind();
                    }
                }
            }
        }
    }
}
