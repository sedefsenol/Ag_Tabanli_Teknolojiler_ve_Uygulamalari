using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Cinescope
{
    public partial class Favoriler : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        int CurrentUserId
        {
            get { return Session["KullaniciID"] == null ? 0 : Convert.ToInt32(Session["KullaniciID"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
{
    string postedTab = Request.Form["hfActiveTab"];
    if (!string.IsNullOrEmpty(postedTab))
        Session["FavTab"] = postedTab;

    if (!IsPostBack)
    {
        YukleFavoriFilmler();
        YukleFavoriDiziler();
    }
}


        void YukleFavoriFilmler()
        {
            using (SqlConnection c = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM FavoriFilmler WHERE KullaniciID=@u", c))
            {
                da.SelectCommand.Parameters.AddWithValue("@u", CurrentUserId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptFavoriFilmler.DataSource = dt;
                rptFavoriFilmler.DataBind();
            }
        }

        void YukleFavoriDiziler()
        {
            using (SqlConnection c = new SqlConnection(connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM FavoriDiziler WHERE KullaniciID=@u", c))
            {
                da.SelectCommand.Parameters.AddWithValue("@u", CurrentUserId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptFavoriDiziler.DataSource = dt;
                rptFavoriDiziler.DataBind();
            }
        }

        protected void rptFavoriFilmler_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                using (SqlConnection c = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM FavoriFilmler WHERE ID=@id", c))
                {
                    cmd.Parameters.AddWithValue("@id", e.CommandArgument);
                    c.Open();
                    cmd.ExecuteNonQuery();
                }
                YukleFavoriFilmler();
            }
        }

        protected void rptFavoriDiziler_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                using (SqlConnection c = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM FavoriDiziler WHERE ID=@id", c))
                {
                    cmd.Parameters.AddWithValue("@id", e.CommandArgument);
                    c.Open();
                    cmd.ExecuteNonQuery();
                }
                YukleFavoriDiziler();
            }
        }
    }
}
