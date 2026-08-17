using System;
using System.Web.UI;

namespace ProyectoIIBJCF
{
    public class SecurePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx", true);
            }
        }
    }
}
