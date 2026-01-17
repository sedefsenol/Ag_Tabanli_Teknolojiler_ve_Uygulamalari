using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class Takipciler : System.Web.UI.Page
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
                TakipcileriGetir();
            }
        }

        void TakipcileriGetir()
        {
            int userId = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
SELECT K.ID, K.KullaniciAdi, K.ProfilFoto
FROM Takip T
JOIN Kullanici K ON K.ID = T.TakipEdenID
WHERE T.TakipEdilenID = @uid
ORDER BY T.ID DESC", con);

                cmd.Parameters.AddWithValue("@uid", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlBos.Visible = true;
                    rptTakipciler.Visible = false;
                }
                else
                {
                    rptTakipciler.DataSource = dt;
                    rptTakipciler.DataBind();
                }
            }
        }
    }
}
