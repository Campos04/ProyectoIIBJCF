using System;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoIIBJCF
{
    public partial class Usuarios : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) LoadGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateUserData()) return;

            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Usuarios_Agregar", connection))
                {
                    AddUserParameters(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ClearForm();
                LoadGrid();
                ShowMessage("El usuario se agregó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo agregar el usuario.", true); }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int userId;
            if (!TryGetUserId(out userId)) return;
            LoadUserById(userId);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int userId;
            if (!TryGetUserId(out userId) || !ValidateUserData()) return;

            try
            {
                int affectedRows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Usuarios_Modificar", connection))
                {
                    command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = userId;
                    AddUserParameters(command);
                    connection.Open();
                    affectedRows = Convert.ToInt32(command.ExecuteScalar());
                }

                if (affectedRows == 0) { ShowMessage("No se encontró un usuario con ese ID.", true); return; }
                LoadGrid();
                ShowMessage("El usuario se modificó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo modificar el usuario.", true); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int userId;
            if (!TryGetUserId(out userId)) return;

            try
            {
                int affectedRows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Usuarios_Borrar", connection))
                {
                    command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = userId;
                    connection.Open();
                    affectedRows = Convert.ToInt32(command.ExecuteScalar());
                }

                if (affectedRows == 0) { ShowMessage("No se pudo borrar. Verifica el ID o que no sea un usuario de acceso.", true); return; }
                ClearForm();
                LoadGrid();
                ShowMessage("El usuario se borró correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo borrar el usuario. Puede tener equipos asociados.", true); }
        }

        private void LoadUserById(int userId)
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Usuarios_Consultar", connection))
                {
                    command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = userId;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) { ShowMessage("No se encontró un usuario con ese ID.", true); return; }
                        txtUserId.Text = reader["UsuarioID"].ToString();
                        txtName.Text = reader["Nombre"].ToString();
                        txtEmail.Text = reader["CorreoElectronico"].ToString();
                        txtPhone.Text = reader["Telefono"] == DBNull.Value ? string.Empty : reader["Telefono"].ToString();
                    }
                }
                ShowMessage("Usuario consultado correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo consultar el usuario.", true); }
        }

        private void LoadGrid()
        {
            try { gvUsers.DataSource = DbHelper.GetTable("sp_Usuarios_Listar"); gvUsers.DataBind(); }
            catch (SqlException) { ShowMessage("No se pudo cargar la lista de usuarios.", true); }
        }

        private void AddUserParameters(SqlCommand command)
        {
            command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
            command.Parameters.Add("@CorreoElectronico", SqlDbType.NVarChar, 150).Value = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 25).Value = string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone;
        }

        private bool ValidateUserData()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) { ShowMessage("Escribe el nombre del usuario.", true); return false; }
            if (string.IsNullOrWhiteSpace(txtEmail.Text)) { ShowMessage("Escribe el correo electrónico.", true); return false; }
            return true;
        }

        private bool TryGetUserId(out int userId)
        {
            if (!int.TryParse(txtUserId.Text.Trim(), out userId) || userId <= 0) { ShowMessage("Escribe un ID de usuario válido.", true); return false; }
            return true;
        }

        private void ClearForm() { txtUserId.Text = ""; txtName.Text = ""; txtEmail.Text = ""; txtPhone.Text = ""; }
        private void ShowMessage(string message, bool isError) { lblMessage.Text = message; lblMessage.CssClass = isError ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
