using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class TumFilmler : System.Web.UI.Page
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
                TumFilmleriGetir();
            }
        }

        void TumFilmleriGetir()
        {
            int userId = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
SELECT FilmID, FilmAdi, PosterPath, IzlenmeTarihi
FROM IzlenenFilmler
WHERE KullaniciID = @uid
ORDER BY IzlenmeTarihi DESC", con);

                cmd.Parameters.AddWithValue("@uid", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlBos.Visible = true;
                    rptTumFilmler.Visible = false;
                }
                else
                {
                    rptTumFilmler.DataSource = dt;
                    rptTumFilmler.DataBind();
                }
            }
        }
    }
}
