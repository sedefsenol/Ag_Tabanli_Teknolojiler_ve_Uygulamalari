using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Cinescope
{
    public partial class Diziler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                YuklePopulerDiziElestirileri();
            }
        }

        public class DiziElestiriDto
        {
            public int ID { get; set; }
            public int DiziID { get; set; }
            public string Yorum { get; set; }
            public string Tarih { get; set; }
            public int Begeni { get; set; }
            public string Poster { get; set; }
            public string DiziAdi { get; set; }
        }

        private void YuklePopulerDiziElestirileri()
        {
            List<DiziElestiriDto> list = new List<DiziElestiriDto>();
            string connStr = ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT TOP 10 
                        ID, DiziID, Yorum, Tarih, Begeni, DiziAdi, Poster 
                    FROM DiziElestiri 
                    ORDER BY Begeni DESC, Tarih DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DiziElestiriDto
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        DiziID = Convert.ToInt32(dr["DiziID"]),
                        Yorum = dr["Yorum"].ToString(),
                        Tarih = Convert.ToDateTime(dr["Tarih"]).ToString("dd.MM.yyyy HH:mm"),
                        Begeni = dr["Begeni"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Begeni"]),
                        Poster = dr["Poster"].ToString(),
                        DiziAdi = dr["DiziAdi"].ToString()
                    });
                }
            }
            rptElestiriler.DataSource = list;
            rptElestiriler.DataBind();
        }
    }
}