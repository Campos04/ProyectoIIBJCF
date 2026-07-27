using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ProyectoIIBJCF
{
    public partial class Equipos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int userId;
            if (!ValidateEquipmentData(out userId))
            {
                return;
            }

            const string query = @"INSERT INTO Equipos (TipoEquipo, Modelo, UsuarioID)
                                   VALUES (@TipoEquipo, @Modelo, @UsuarioID);";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddEquipmentParameters(command, userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ClearForm();
                ShowMessage("El equipo se agregó correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo agregar el equipo.", true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int equipmentId;
            if (!TryGetEquipmentId(out equipmentId))
            {
                return;
            }

            LoadEquipmentById(equipmentId);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int equipmentId;
            int userId;

            if (!TryGetEquipmentId(out equipmentId) || !ValidateEquipmentData(out userId))
            {
                return;
            }

            const string query = @"UPDATE Equipos
                                   SET TipoEquipo = @TipoEquipo,
                                       Modelo = @Modelo,
                                       UsuarioID = @UsuarioID
                                   WHERE EquipoID = @EquipoID;";

            try
            {
                int affectedRows;

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddEquipmentParameters(command, userId);
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId;
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    ShowMessage("No se encontró un equipo con ese ID.", true);
                    return;
                }

                ShowMessage("El equipo se modificó correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo modificar el equipo.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int equipmentId;
            if (!TryGetEquipmentId(out equipmentId))
            {
                return;
            }

            const string query = "DELETE FROM Equipos WHERE EquipoID = @EquipoID;";

            try
            {
                int affectedRows;

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId;
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    ShowMessage("No se encontró un equipo con ese ID.", true);
                    return;
                }

                ClearForm();
                ShowMessage("El equipo se borró correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo borrar el equipo.", true);
            }
        }

        private void LoadUsers()
        {
            const string query = "SELECT UsuarioID, Nombre FROM Usuarios ORDER BY Nombre;";

            ddlUsers.Items.Clear();
            ddlUsers.Items.Add(new ListItem("-- Seleccione un usuario --", string.Empty));

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string userId = reader["UsuarioID"].ToString();
                            string userName = reader["Nombre"].ToString();
                            ddlUsers.Items.Add(new ListItem(userName, userId));
                        }
                    }
                }
            }
            catch (SqlException)
            {
                ShowMessage("No se pudieron cargar los usuarios.", true);
            }
        }

        private void LoadEquipmentById(int equipmentId)
        {
            const string query = @"SELECT EquipoID, TipoEquipo, Modelo, UsuarioID
                                   FROM Equipos
                                   WHERE EquipoID = @EquipoID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowMessage("No se encontró un equipo con ese ID.", true);
                            return;
                        }

                        txtEquipmentId.Text = reader["EquipoID"].ToString();
                        txtEquipmentType.Text = reader["TipoEquipo"].ToString();
                        txtModel.Text = reader["Modelo"].ToString();

                        string userId = reader["UsuarioID"].ToString();
                        if (ddlUsers.Items.FindByValue(userId) != null)
                        {
                            ddlUsers.SelectedValue = userId;
                        }
                    }
                }

                ShowMessage("Equipo consultado correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo consultar el equipo.", true);
            }
        }

        private void AddEquipmentParameters(SqlCommand command, int userId)
        {
            command.Parameters.Add("@TipoEquipo", SqlDbType.NVarChar, 80).Value = txtEquipmentType.Text.Trim();
            command.Parameters.Add("@Modelo", SqlDbType.NVarChar, 100).Value = txtModel.Text.Trim();
            command.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = userId;
        }

        private bool ValidateEquipmentData(out int userId)
        {
            userId = 0;

            if (string.IsNullOrWhiteSpace(txtEquipmentType.Text))
            {
                ShowMessage("Escribe el tipo de equipo.", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                ShowMessage("Escribe el modelo del equipo.", true);
                return false;
            }

            if (!int.TryParse(ddlUsers.SelectedValue, out userId) || userId <= 0)
            {
                ShowMessage("Selecciona un usuario.", true);
                return false;
            }

            return true;
        }

        private bool TryGetEquipmentId(out int equipmentId)
        {
            if (!int.TryParse(txtEquipmentId.Text.Trim(), out equipmentId) || equipmentId <= 0)
            {
                ShowMessage("Escribe un ID de equipo válido.", true);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtEquipmentId.Text = string.Empty;
            txtEquipmentType.Text = string.Empty;
            txtModel.Text = string.Empty;
            ddlUsers.SelectedIndex = 0;
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
