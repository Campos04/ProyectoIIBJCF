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
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Equipos_Agregar", connection))
                {
                    AddEquipmentParameters(command, userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                ClearForm(); LoadGrid(); ShowMessage("El equipo se agregó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo agregar el equipo.", true); }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int equipmentId;
            if (!TryGetEquipmentId(out equipmentId)) return;
            LoadEquipmentById(equipmentId);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int equipmentId, userId;
            if (!TryGetEquipmentId(out equipmentId) || !ValidateEquipmentData(out userId)) return;

            try
            {
                int affectedRows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Equipos_Modificar", connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId;
                    AddEquipmentParameters(command, userId);
                    connection.Open();
                    affectedRows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (affectedRows == 0) { ShowMessage("No se encontró un equipo con ese ID.", true); return; }
                LoadGrid(); ShowMessage("El equipo se modificó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo modificar el equipo.", true); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int equipmentId;
            if (!TryGetEquipmentId(out equipmentId)) return;

            try
            {
                int affectedRows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Equipos_Borrar", connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId;
                    connection.Open();
                    affectedRows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (affectedRows == 0) { ShowMessage("No se encontró un equipo con ese ID.", true); return; }
                ClearForm(); LoadGrid(); ShowMessage("El equipo se borró correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo borrar el equipo. Puede tener reparaciones asociadas.", true); }
        }

        private void LoadUsers()
        {
            ddlUsers.Items.Clear();
            ddlUsers.Items.Add(new ListItem("-- Seleccione un usuario --", ""));
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Usuarios_Listar", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            ddlUsers.Items.Add(new ListItem(reader["Nombre"].ToString(), reader["UsuarioID"].ToString()));
                }
            }
            catch (SqlException) { ShowMessage("No se pudieron cargar los usuarios.", true); }
        }

        private void LoadEquipmentById(int equipmentId)
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Equipos_Consultar", connection))
                {
                    command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) { ShowMessage("No se encontró un equipo con ese ID.", true); return; }
                        txtEquipmentId.Text = reader["EquipoID"].ToString();
                        txtEquipmentType.Text = reader["TipoEquipo"].ToString();
                        txtModel.Text = reader["Modelo"].ToString();
                        string userId = reader["UsuarioID"].ToString();
                        if (ddlUsers.Items.FindByValue(userId) != null) ddlUsers.SelectedValue = userId;
                    }
                }
                ShowMessage("Equipo consultado correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo consultar el equipo.", true); }
        }

        private void LoadGrid()
        {
            try { gvEquipment.DataSource = DbHelper.GetTable("sp_Equipos_Listar"); gvEquipment.DataBind(); }
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

        private bool TryGetEquipmentId(out int equipmentId)
        {
            if (!int.TryParse(txtEquipmentId.Text.Trim(), out equipmentId) || equipmentId <= 0) { ShowMessage("Escribe un ID de equipo válido.", true); return false; }
            return true;
        }

        private void ClearForm() { txtEquipmentId.Text = ""; txtEquipmentType.Text = ""; txtModel.Text = ""; ddlUsers.SelectedIndex = 0; }
        private void ShowMessage(string message, bool isError) { lblMessage.Text = message; lblMessage.CssClass = isError ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
