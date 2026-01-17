using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Cinescope
{
    public partial class OrtakListe : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["CineScopeDB"].ConnectionString;

        int benimId;
        int digerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null || Request.QueryString["uid"] == null)
            {
                Response.Redirect("Anasayfa.aspx");
                return;
            }

            benimId = Convert.ToInt32(Session["KullaniciID"]);
            digerId = Convert.ToInt32(Request.QueryString["uid"]);

            if (benimId == digerId)
            {
                Response.Redirect("Kullanici.aspx?id=" + digerId);
                return;
            }
        }

        protected void btnOlustur_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtListeAdi.Text))
                return;

            int ortakListeId;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

               
                SqlCommand cmd = new SqlCommand(@"
INSERT INTO OrtakListeler
(
    ListeAdi,
    Aciklama,
    OlusturanKullaniciID,
    DigerKullaniciID
)
OUTPUT INSERTED.ID
VALUES
(
    @adi,
    @aciklama,
    @ben,
    @diger
)", con);

                cmd.Parameters.AddWithValue("@adi", txtListeAdi.Text.Trim());
                cmd.Parameters.AddWithValue("@aciklama", txtAciklama.Text.Trim());
                cmd.Parameters.AddWithValue("@ben", benimId);
                cmd.Parameters.AddWithValue("@diger", digerId);

                ortakListeId = (int)cmd.ExecuteScalar();

        
                SqlCommand bildirimCmd = new SqlCommand(@"
INSERT INTO Bildirimler
(
    AlanKullaniciID,
    GonderenKullaniciID,
    BildirimTuru,
    Mesaj,
    IlgiliID,
    OlusturmaTarihi,
    Okundu
)
VALUES
(
    @alan,
    @gonderen,
    'ORTAK_LISTE',
    'seninle ortak bir liste oluşturdu, sen de film ekle',
    @listeId,
    GETDATE(),
    0
)
", con);

                bildirimCmd.Parameters.AddWithValue("@alan", digerId);
                bildirimCmd.Parameters.AddWithValue("@gonderen", benimId);
                bildirimCmd.Parameters.AddWithValue("@listeId", ortakListeId);

                bildirimCmd.ExecuteNonQuery();
            }

            
            Response.Redirect("Listeler.aspx");
        }
    }
}
