using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cinescope
{
    public partial class TakipEdilenler : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null)
            {
                Response.Redirect("Girisyap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                TakipEdilenleriGetir();
            }
        }

        void TakipEdilenleriGetir()
        {
            int userId = Convert.ToInt32(Session["KullaniciID"]);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
SELECT K.ID, K.KullaniciAdi, K.ProfilFoto
FROM Takip T
JOIN Kullanici K ON K.ID = T.TakipEdilenID
WHERE T.TakipEdenID = @uid
ORDER BY T.ID DESC", con);

                cmd.Parameters.AddWithValue("@uid", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlBos.Visible = true;
                    rptTakipEdilenler.Visible = false;
                }
                else
                {
                    rptTakipEdilenler.DataSource = dt;
                    rptTakipEdilenler.DataBind();
                }
            }
        }
    }
}
