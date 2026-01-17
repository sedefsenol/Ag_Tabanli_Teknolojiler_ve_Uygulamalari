using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace Cinescope
{
    public partial class OrtakListeDuzenle : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        int listeID;
        int benimID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null ||
                !int.TryParse(Request.QueryString["id"], out listeID))
            {
                Response.Redirect("Listeler.aspx");
                return;
            }

            benimID = Convert.ToInt32(Session["KullaniciID"]);

            if (!IsPostBack)
            {
                ListeBilgileriniGetir();
                MevcutIcerikleriGetir();
            }
        }

        void ListeBilgileriniGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
SELECT ListeAdi, Aciklama
FROM OrtakListeler
WHERE ID=@id", con);

                cmd.Parameters.AddWithValue("@id", listeID);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblListeAdi.Text = dr["ListeAdi"].ToString();
                    lblAciklama.Text = dr["Aciklama"].ToString();
                }
            }
        }

        void MevcutIcerikleriGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
SELECT IcerikID, PosterPath
FROM OrtakListeIcerik
WHERE OrtakListeID=@id", con);

                cmd.Parameters.AddWithValue("@id", listeID);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                var list = new List<object>();

                while (dr.Read())
                {
                    list.Add(new
                    {
                        IcerikID = dr["IcerikID"],
                        PosterPath = dr["PosterPath"].ToString()
                    });
                }

                var js = new JavaScriptSerializer();
                string json = js.Serialize(list);
                hfItems.Value = json;

                ClientScript.RegisterStartupScript(
                    GetType(),
                    "load",
                    $"loadItems('{json.Replace("'", "\\'")}');",
                    true
                );
            }
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            var js = new JavaScriptSerializer();
            dynamic items = js.Deserialize<dynamic>(hfItems.Value);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                SqlCommand sil = new SqlCommand(
                    "DELETE FROM OrtakListeIcerik WHERE OrtakListeID=@id", con);
                sil.Parameters.AddWithValue("@id", listeID);
                sil.ExecuteNonQuery();

                foreach (var x in items)
                {
                    SqlCommand ekle = new SqlCommand(@"
INSERT INTO OrtakListeIcerik
(OrtakListeID, IcerikTipi, IcerikID, PosterPath, EkleyenKullaniciID)
VALUES
(@lid, 'film', @iid, @poster, @ekleyen)", con);

                    ekle.Parameters.AddWithValue("@lid", listeID);
                    ekle.Parameters.AddWithValue("@iid", x["IcerikID"]);
                    ekle.Parameters.AddWithValue("@poster", x["PosterPath"]);
                    ekle.Parameters.AddWithValue("@ekleyen", benimID);
                    ekle.ExecuteNonQuery();
                }
            }

            Response.Redirect("Listesayfasi.aspx?lid=" + listeID);
        }
    }
}
