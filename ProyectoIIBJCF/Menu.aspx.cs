using System;
using System.Web.Security;

namespace ProyectoIIBJCF
{
    public partial class Menu : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["logout"] == "1")
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();
                Response.Redirect("Login.aspx");
            }
        }
    }
}
