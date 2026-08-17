using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace ProyectoIIBJCF
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                Response.Redirect("Menu.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("Escribe el usuario y la contraseña.");
                return;
            }

            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Login", connection))
                {
                    command.Parameters.Add("@UsuarioLogin", SqlDbType.NVarChar, 50).Value = user;
                    command.Parameters.Add("@Clave", SqlDbType.NVarChar, 100).Value = password;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowMessage("Usuario o contraseña incorrectos.");
                            return;
                        }

                        Session["UsuarioID"] = reader["UsuarioID"];
                        Session["NombreUsuario"] = reader["Nombre"].ToString();
                        Session["Rol"] = reader["Rol"].ToString();
                    }
                }

                FormsAuthentication.SetAuthCookie(user, false);
                Response.Redirect("Menu.aspx");
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo conectar con la base de datos.");
            }
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
        }
    }
}
