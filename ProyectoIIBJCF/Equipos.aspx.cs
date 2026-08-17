using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ProyectoIIBJCF
{
    public partial class Equipos : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
                LoadGrid();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int userId;
            if (!ValidateEquipmentData(out userId)) return;
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateStoredProcedureCommand("sp_Equipos_Agregar", connection))
                {
                    AddEquipmentParameters(command, userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                ClearForm(); LoadGrid(); ShowMessage("El equipo se agregó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo agregar el equipo.", true); }
        }

        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetEquipmentId(out id)) LoadEquipmentById(id); }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id, userId;
            if (!TryGetEquipmentId(out id) || !ValidateEquipmentData(out userId)) return;
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateStoredProcedureCommand("sp_Equipos_Modificar", connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = id;
                    AddEquipmentParameters(command, userId);
                    connection.Open(); command.ExecuteNonQuery();
                }
                LoadGrid(); ShowMessage("El equipo se modificó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo modificar el equipo.", true); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetEquipmentId(out id)) return;
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateStoredProcedureCommand("sp_Equipos_Eliminar", connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = id;
                    connection.Open(); command.ExecuteNonQuery();
                }
                ClearForm(); LoadGrid(); ShowMessage("El equipo se borró correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo borrar el equipo. Revisa si tiene reparaciones asociadas.", true); }
        }

        private void LoadUsers()
        {
            ddlUsers.Items.Clear(); ddlUsers.Items.Add(new ListItem("-- Seleccione un usuario --", ""));
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateStoredProcedureCommand("sp_Usuarios_Listar", connection))
                {
                    connection.Open(); using (SqlDataReader reader = command.ExecuteReader())
                        while (reader.Read()) ddlUsers.Items.Add(new ListItem(reader["Nombre"].ToString(), reader["UsuarioID"].ToString()));
                }
            }
            catch (SqlException) { ShowMessage("No se pudieron cargar los usuarios.", true); }
        }

        private void LoadEquipmentById(int id)
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateStoredProcedureCommand("sp_Equipos_Consultar", connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = id;
                    connection.Open(); using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) { ShowMessage("No se encontró un equipo con ese ID.", true); return; }
                        txtEquipmentId.Text = reader["EquipoID"].ToString(); txtEquipmentType.Text = reader["TipoEquipo"].ToString(); txtModel.Text = reader["Modelo"].ToString();
                        string userId = reader["UsuarioID"].ToString(); if (ddlUsers.Items.FindByValue(userId) != null) ddlUsers.SelectedValue = userId;
                    }
                }
                ShowMessage("Equipo consultado correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo consultar el equipo.", true); }
        }

        private void LoadGrid()
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateStoredProcedureCommand("sp_Equipos_Listar", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                { DataTable table = new DataTable(); adapter.Fill(table); gridData.DataSource = table; gridData.DataBind(); }
            }
            catch (SqlException) { ShowMessage("No se pudo cargar la lista de equipos.", true); }
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
            if (string.IsNullOrWhiteSpace(txtEquipmentType.Text)) { ShowMessage("Escribe el tipo de equipo.", true); return false; }
            if (string.IsNullOrWhiteSpace(txtModel.Text)) { ShowMessage("Escribe el modelo del equipo.", true); return false; }
            if (!int.TryParse(ddlUsers.SelectedValue, out userId) || userId <= 0) { ShowMessage("Selecciona un usuario.", true); return false; }
            return true;
        }
        private bool TryGetEquipmentId(out int id) { if (!int.TryParse(txtEquipmentId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de equipo válido.", true); return false; } return true; }
        private void ClearForm() { txtEquipmentId.Text = ""; txtEquipmentType.Text = ""; txtModel.Text = ""; if (ddlUsers.Items.Count > 0) ddlUsers.SelectedIndex = 0; }
        private void ShowMessage(string message, bool error) { lblMessage.Text = message; lblMessage.CssClass = error ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
