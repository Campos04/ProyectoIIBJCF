using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoIIBJCF
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateUserData())
            {
                return;
            }

            const string query = @"INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono)
                                   VALUES (@Nombre, @CorreoElectronico, @Telefono);";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddUserParameters(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ClearForm();
                ShowMessage("El usuario se agregó correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo agregar el usuario.", true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int userId;
            if (!TryGetUserId(out userId))
            {
                return;
            }

            LoadUserById(userId);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int userId;
            if (!TryGetUserId(out userId) || !ValidateUserData())
            {
                return;
            }

            const string query = @"UPDATE Usuarios
                                   SET Nombre = @Nombre,
                                       CorreoElectronico = @CorreoElectronico,
                                       Telefono = @Telefono
                                   WHERE UsuarioID = @UsuarioID;";

            try
            {
                int affectedRows;

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddUserParameters(command);
                    command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = userId;
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    ShowMessage("No se encontró un usuario con ese ID.", true);
                    return;
                }

                ShowMessage("El usuario se modificó correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo modificar el usuario.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int userId;
            if (!TryGetUserId(out userId))
            {
                return;
            }

            const string query = "DELETE FROM Usuarios WHERE UsuarioID = @UsuarioID;";

            try
            {
                int affectedRows;

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = userId;
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    ShowMessage("No se encontró un usuario con ese ID.", true);
                    return;
                }

                ClearForm();
                ShowMessage("El usuario se borró correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo borrar el usuario.", true);
            }
        }

        private void LoadUserById(int userId)
        {
            const string query = @"SELECT UsuarioID, Nombre, CorreoElectronico, Telefono
                                   FROM Usuarios
                                   WHERE UsuarioID = @UsuarioID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = userId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowMessage("No se encontró un usuario con ese ID.", true);
                            return;
                        }

                        txtUserId.Text = reader["UsuarioID"].ToString();
                        txtName.Text = reader["Nombre"].ToString();
                        txtEmail.Text = reader["CorreoElectronico"].ToString();
                        txtPhone.Text = reader["Telefono"] == DBNull.Value ? string.Empty : reader["Telefono"].ToString();
                    }
                }

                ShowMessage("Usuario consultado correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo consultar el usuario.", true);
            }
        }

        private void AddUserParameters(SqlCommand command)
        {
            command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
            command.Parameters.Add("@CorreoElectronico", SqlDbType.NVarChar, 150).Value = txtEmail.Text.Trim();

            string phone = txtPhone.Text.Trim();
            command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 25).Value =
                string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone;
        }

        private bool ValidateUserData()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowMessage("Escribe el nombre del usuario.", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowMessage("Escribe el correo electrónico.", true);
                return false;
            }

            return true;
        }

        private bool TryGetUserId(out int userId)
        {
            if (!int.TryParse(txtUserId.Text.Trim(), out userId) || userId <= 0)
            {
                ShowMessage("Escribe un ID de usuario válido.", true);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtUserId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "message error" : "message success";
            lblMessage.Visible = true;
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["RepairDb"].ConnectionString;
        }
    }
}
