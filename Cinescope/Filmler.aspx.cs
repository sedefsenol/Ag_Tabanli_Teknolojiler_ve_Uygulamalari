using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace Cinescope
{
    public partial class Filmler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                YuklePopulerElestiriler();
            }
        }

        public class ElestiriDto
        {
            public int ID { get; set; }
            public int KullaniciID { get; set; }
            public int FilmID { get; set; }
            public string Yorum { get; set; }
            public string Tarih { get; set; }
            public int Begeni { get; set; }
            public int Begenmeme { get; set; }
            public string Poster { get; set; }   
            public string FilmAdi { get; set; }  
        }

        private string GetPosterFromTMDB(int filmId)
        {
            try
            {
                string apiKey = "02f7a3e84e6acce9ce0b5583deffb717";
                string apiUrl = $"https://api.themoviedb.org/3/movie/{filmId}?api_key={apiKey}&language=tr-TR";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8; 
                    string json = client.DownloadString(apiUrl);
                    dynamic data = JsonConvert.DeserializeObject(json);

                    return data.poster_path != null ? (string)data.poster_path : "";
                }
            }
            catch
            {
                return "";
            }
        }


        private string GetFilmTitleFromTMDB(int filmId)
        {
            try
            {
                string apiKey = "02f7a3e84e6acce9ce0b5583deffb717";
                string apiUrl = $"https://api.themoviedb.org/3/movie/{filmId}?api_key={apiKey}&language=tr-TR";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8; 
                    string json = client.DownloadString(apiUrl);
                    dynamic data = JsonConvert.DeserializeObject(json);

                    return data.title != null ? (string)data.title : "Bilinmeyen Film";
                }
            }
            catch
            {
                return "Bilinmeyen Film";
            }
        }


        private void YuklePopulerElestiriler()
        {
            List<ElestiriDto> list = new List<ElestiriDto>();

            string connStr = ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT TOP 10
                        ID,
                        KullaniciID,
                        FilmID,
                        Yorum,
                        Tarih,
                        Begeni,
                        Begenmeme
                    FROM Elestiri
                    ORDER BY Begeni DESC, Tarih DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int filmId = Convert.ToInt32(dr["FilmID"]);

                    var dto = new ElestiriDto
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        KullaniciID = Convert.ToInt32(dr["KullaniciID"]),
                        FilmID = filmId,
                        Yorum = dr["Yorum"].ToString(),
                        Tarih = Convert.ToDateTime(dr["Tarih"]).ToString("dd.MM.yyyy HH:mm"),
                        Begeni = dr["Begeni"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Begeni"]),
                        Begenmeme = dr["Begenmeme"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Begenmeme"]),
                        Poster = GetPosterFromTMDB(filmId),
                        FilmAdi = GetFilmTitleFromTMDB(filmId)
                    };

                    list.Add(dto);
                }
            }

            rptElestiriler.DataSource = list;
            rptElestiriler.DataBind();
        }
    }
}
