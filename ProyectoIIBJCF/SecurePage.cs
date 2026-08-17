using System;
using System.Web.Security;
using System.Web.UI;

namespace ProyectoIIBJCF
{
    public class SecurePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            base.OnInit(e);
        }
    }
}
