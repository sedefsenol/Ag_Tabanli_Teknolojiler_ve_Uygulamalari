using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace Cinescope
{
    public partial class Listesayfasi : System.Web.UI.Page
    {
        string connStr = ConfigurationManager
            .ConnectionStrings["CineScopeDB"].ConnectionString;

        int listeID;
        int listeSahibiID = 0;   
        int? girisYapanID = null;
        bool ortakListeMi = false;
        int digerKullaniciID = 0;


        protected void Page_Load(object sender, EventArgs e)
        {

           

            if (!int.TryParse(Request.QueryString["lid"], out listeID))
                Response.Redirect("Listeler.aspx");
           

            if (Session["KullaniciID"] != null)
                girisYapanID = Convert.ToInt32(Session["KullaniciID"]);
            ListeTurunuBelirle();

            if (!IsPostBack)
            {
                ListeBilgileriniGetir();
                FilmleriGetir();
                DuzenlemeYetkisiniAyarla(); 
            }
        }
        void ListeTurunuBelirle()
        {
            ortakListeMi = false;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
SELECT OlusturanKullaniciID, DigerKullaniciID
FROM OrtakListeler
WHERE ID = @id", con);

                cmd.Parameters.AddWithValue("@id", listeID);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ortakListeMi = true;
                    listeSahibiID = Convert.ToInt32(dr["OlusturanKullaniciID"]);
                    digerKullaniciID = Convert.ToInt32(dr["DigerKullaniciID"]);
                }
            }
        }


        void ListeBilgileriniGetir()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (ortakListeMi)
                {
                    cmd = new SqlCommand(@"
SELECT ListeAdi, Aciklama
FROM OrtakListeler
WHERE ID=@id", con);
                }
                else
                {
                    cmd = new SqlCommand(@"
SELECT ListeAdi, Aciklama, KullaniciID
FROM Listeler
WHERE ID=@id", con);
                }

                cmd.Parameters.AddWithValue("@id", listeID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblListeAdi.Text = dr["ListeAdi"].ToString();
                    lblAciklama.Text = dr["Aciklama"].ToString();

                    txtListeAdi.Text = lblListeAdi.Text;
                    txtAciklama.Text = lblAciklama.Text;

                    if (!ortakListeMi)
                        listeSahibiID = Convert.ToInt32(dr["KullaniciID"]);
                }
                else
                {
                    Response.Redirect("Listeler.aspx");
                }
            }
        }


        void DuzenlemeYetkisiniAyarla()
        {
           

            if (girisYapanID == null)
            {
                btnEdit.Visible = false;
                pnlEdit.Visible = false;
                return;
            }

            if (ortakListeMi)
            {
                btnEdit.Visible =
                    girisYapanID == listeSahibiID ||
                    girisYapanID == digerKullaniciID;
            }
            else
            {
                btnEdit.Visible = girisYapanID == listeSahibiID;
            }

        }

        void FilmleriGetir()
        {
            if (ortakListeMi)
            {
                OrtakListeFilmleriniGetir();
                return;
            }


            ltPosters.Text = "";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT FilmlerJson
                    FROM Listeler
                    WHERE ID=@id", con);

                cmd.Parameters.AddWithValue("@id", listeID);

                con.Open();
                object jsonObj = cmd.ExecuteScalar();

                if (jsonObj == null || jsonObj == DBNull.Value)
                    return;

                string json = jsonObj.ToString();

                JavaScriptSerializer js = new JavaScriptSerializer();
                var films = js.Deserialize<List<Dictionary<string, object>>>(json);

                foreach (var f in films)
                {
                    string poster = "";
                    string title = "";

                    if (f.ContainsKey("poster")) poster = f["poster"]?.ToString();
                    else if (f.ContainsKey("PosterPath")) poster = f["PosterPath"]?.ToString();
                    else if (f.ContainsKey("poster_path")) poster = f["poster_path"]?.ToString();

                    if (f.ContainsKey("title")) title = f["title"]?.ToString();
                    else if (f.ContainsKey("FilmAdi")) title = f["FilmAdi"]?.ToString();
                    else if (f.ContainsKey("name")) title = f["name"]?.ToString();

                    if (string.IsNullOrEmpty(poster)) continue;

                    ltPosters.Text += $@"
                        <img class='film-poster'
                             src='https://image.tmdb.org/t/p/w200{poster}'
                             title='{title}' />";
                }

                hfFilmlerJson.Value = json;

                ClientScript.RegisterStartupScript(
                    GetType(),
                    "loadFilms",
                    $"loadFilms('{json.Replace("'", "\\'")}');",
                    true
                );
            }
        }

        void OrtakListeFilmleriniGetir()
        {
            ltPosters.Text = "";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
SELECT PosterPath
FROM OrtakListeIcerik
WHERE OrtakListeID = @id
ORDER BY Tarih DESC", con);

                cmd.Parameters.AddWithValue("@id", listeID);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ltPosters.Text += $@"
<img class='film-poster'
     src='https://image.tmdb.org/t/p/w200{dr["PosterPath"]}' />";
                }
            }

            pnlEdit.Visible = false;
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (ortakListeMi)
            {
                Response.Redirect("OrtakListeDuzenle.aspx?id=" + listeID);
                return;
            }

            if (girisYapanID == null || girisYapanID != listeSahibiID)
                return;

            pnlEdit.Visible = !pnlEdit.Visible;
            viewFilmContainer.Visible = !pnlEdit.Visible;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (girisYapanID == null || girisYapanID != listeSahibiID)
                return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Listeler
                    SET ListeAdi=@adi,
                        Aciklama=@aciklama,
                        FilmlerJson=@json
                    WHERE ID=@id", con);

                cmd.Parameters.AddWithValue("@adi", txtListeAdi.Text);
                cmd.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
                cmd.Parameters.AddWithValue("@json", hfFilmlerJson.Value);
                cmd.Parameters.AddWithValue("@id", listeID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("Listesayfasi.aspx?lid=" + listeID);
        }
    }
}
