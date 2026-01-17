using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Cinescope
{
    public partial class Listeolustur : System.Web.UI.Page
    {
        string cs = ConfigurationManager
            .ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["KullaniciID"] == null)
            {
                Response.Redirect("girisyap.aspx");
                return;
            }
        }

        
        protected void btnOlustur_Click(object sender, EventArgs e)
        {
            int kullaniciID = Convert.ToInt32(Session["KullaniciID"]);

            string listeAdi = txtListeAdi.Text.Trim();
            string aciklama = txtAciklama.Text.Trim();
            string gorunurluk = ddlGorunurluk.SelectedValue;

            string iceriklerJson = hfSelectedFilms.Value;

   
            if (string.IsNullOrEmpty(listeAdi))
                return;

            if (string.IsNullOrEmpty(iceriklerJson) || iceriklerJson == "[]")
                return;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Listeler
                    (
                        KullaniciID,
                        ListeAdi,
                        Aciklama,
                        Gorunurluk,
                        FilmlerJson,
                        IsCommon,
                        Tarih
                    )
                    VALUES
                    (
                        @kullaniciID,
                        @listeAdi,
                        @aciklama,
                        @gorunurluk,
                        @filmlerJson,
                        0,
                        GETDATE()
                    )
                ", con);

                cmd.Parameters.AddWithValue("@kullaniciID", kullaniciID);
                cmd.Parameters.AddWithValue("@listeAdi", listeAdi);
                cmd.Parameters.AddWithValue("@aciklama", aciklama);
                cmd.Parameters.AddWithValue("@gorunurluk", gorunurluk);
                cmd.Parameters.AddWithValue("@filmlerJson", iceriklerJson);

                cmd.ExecuteNonQuery();
            }

            Response.Redirect("Listeler.aspx");
        }
    }
}
