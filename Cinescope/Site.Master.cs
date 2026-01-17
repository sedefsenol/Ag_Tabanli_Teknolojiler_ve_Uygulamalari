using System;
using System.Web.UI;

namespace Cinescope
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Master page her yüklendiğinde çalışır
            if (!IsPostBack)
            {
                if (Session["KullaniciID"] == null)
                {
                    btnHesap.Text = "Giriş Yap";
                }
                else
                {
                    btnHesap.Text = "Hesap";
                }
            }
        }

        protected void btnHesap_Click(object sender, EventArgs e)
        {
            if (Session["KullaniciID"] == null)
            {
                Response.Redirect("Girisyap.aspx");
            }
            else
            {
                Response.Redirect("Hesap.aspx");
            }
        }
    }
}
