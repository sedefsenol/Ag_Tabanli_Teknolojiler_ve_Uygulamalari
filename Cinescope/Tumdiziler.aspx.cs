using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class TumDiziler : System.Web.UI.Page
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
                TumDizileriGetir();
            }
        }

        void TumDizileriGetir()
        {
            int userId = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
SELECT DiziID, DiziAdi, PosterPath
FROM IzlenenDiziler
WHERE KullaniciID = @uid
ORDER BY ID DESC", con);

                cmd.Parameters.AddWithValue("@uid", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlBos.Visible = true;
                    rptTumDiziler.Visible = false;
                }
                else
                {
                    rptTumDiziler.DataSource = dt;
                    rptTumDiziler.DataBind();
                }
            }
        }
    }
}
