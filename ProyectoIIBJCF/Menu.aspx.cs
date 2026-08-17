using System;
using System.Web.Security;

namespace ProyectoIIBJCF
{
    public partial class Menu : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string name = Session["NombreUsuario"] == null ? User.Identity.Name : Session["NombreUsuario"].ToString();
                lblUser.Text = name;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");
        }
    }
}
