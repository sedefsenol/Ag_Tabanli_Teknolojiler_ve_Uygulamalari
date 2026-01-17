using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Cinescope
{
    public partial class Listeler : System.Web.UI.Page
    {
        string connStr = ConfigurationManager
            .ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null)
            {
                Response.Redirect("Girisyap.aspx");
                return;
            }

            if (!IsPostBack)
                ListeleriGetir();
        }

        void ListeleriGetir()
        {
            int kullaniciId = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"

SELECT 
    L.ID            AS ListeID,
    L.ListeAdi,
    L.FilmlerJson   AS FilmlerJson,
    0               AS IsCommon
FROM Listeler L
WHERE L.KullaniciID = @uid

UNION ALL


SELECT 
    OL.ID           AS ListeID,
    OL.ListeAdi,
    (
        SELECT 
            OLI.PosterPath AS poster
        FROM OrtakListeIcerik OLI
        WHERE OLI.OrtakListeID = OL.ID
        FOR JSON PATH
    )               AS FilmlerJson,
    1               AS IsCommon
FROM OrtakListeler OL
WHERE @uid IN (OL.OlusturanKullaniciID, OL.DigerKullaniciID)

ORDER BY ListeID DESC
", conn);

                da.SelectCommand.Parameters.AddWithValue("@uid", kullaniciId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                rptListeler.DataSource = dt;
                rptListeler.DataBind();
            }
        }

        
        public string GetFilmPosters(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return "";

            StringBuilder sb = new StringBuilder();

            try
            {
                JArray films = JArray.Parse(json);
                int count = 0;

                foreach (var film in films)
                {
                    if (count >= 12)
                        break;

                    string poster = film["poster"]?.ToString();
                    if (!string.IsNullOrEmpty(poster))
                    {
                        sb.Append($@"
<img class='film-poster'
     src='https://image.tmdb.org/t/p/w200{poster}'
     alt='poster' />
");
                        count++;
                    }
                }
            }
            catch
            {
             
            }

            return sb.ToString();
        }

        public string GetCommonBadge(object isCommon)
        {
            if (isCommon == null) return "";

            return Convert.ToInt32(isCommon) == 1
                ? "<span class='badge ortak'>Ortak Liste</span>"
                : "";
        }
    }
}
