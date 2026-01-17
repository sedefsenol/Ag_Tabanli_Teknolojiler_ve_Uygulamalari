using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cinescope
{
    public partial class DahaSonra : Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        private int CurrentUserId
        {
            get
            {
                if (Session["KullaniciID"] == null)
                    return 0;
                return Convert.ToInt32(Session["KullaniciID"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUserId == 0)
                {
                    Response.Redirect("Girisyap.aspx");
                    return;
                }

                YukleFilmler();
                YukleDiziler();
            }
        }

       
        private void YukleFilmler()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT ID, FilmID, PosterPath
                    FROM DahaSonraFilmler
                    WHERE KullaniciID = @KID
                    ORDER BY Tarih DESC", conn);

                cmd.Parameters.AddWithValue("@KID", CurrentUserId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptDahaSonraFilmler.DataSource = dt;
                rptDahaSonraFilmler.DataBind();
            }
        }

        protected void rptDahaSonraFilmler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Sil") return;

            int id = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM DahaSonraFilmler
                    WHERE ID=@ID AND KullaniciID=@KID", conn);

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@KID", CurrentUserId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            YukleFilmler();
        }

       
        private void YukleDiziler()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT ID, DiziID, PosterPath
                    FROM DahaSonraDiziler
                    WHERE KullaniciID = @KID
                    ORDER BY Tarih DESC", conn);

                cmd.Parameters.AddWithValue("@KID", CurrentUserId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptDahaSonraDiziler.DataSource = dt;
                rptDahaSonraDiziler.DataBind();
            }
        }

        protected void rptDahaSonraDiziler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Sil") return;

            int id = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM DahaSonraDiziler
                    WHERE ID=@ID AND KullaniciID=@KID", conn);

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@KID", CurrentUserId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            YukleDiziler();
        }
    }
}
