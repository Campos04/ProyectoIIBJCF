using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ProyectoIIBJCF
{
    public partial class Reparaciones : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { LoadEquipment(); LoadGrid(); txtRequestDate.Text = DateTime.Today.ToString("yyyy-MM-dd"); }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int equipmentId; DateTime requestDate;
            if (!ValidateData(out equipmentId, out requestDate)) return;
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Reparaciones_Agregar", connection))
                {
                    AddParameters(command, equipmentId, requestDate); connection.Open(); command.ExecuteNonQuery();
                }
                ClearForm(); LoadGrid(); ShowMessage("La reparación se agregó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo agregar la reparación.", true); }
        }

        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id, equipmentId; DateTime requestDate;
            if (!TryGetId(out id) || !ValidateData(out equipmentId, out requestDate)) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Reparaciones_Modificar", connection))
                {
                    command.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = id;
                    AddParameters(command, equipmentId, requestDate); connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró una reparación con ese ID.", true); return; }
                LoadGrid(); ShowMessage("La reparación se modificó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo modificar la reparación.", true); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Reparaciones_Borrar", connection))
                {
                    command.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = id; connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró una reparación con ese ID.", true); return; }
                ClearForm(); LoadGrid(); ShowMessage("La reparación se borró correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo borrar la reparación. Puede tener asignaciones o detalles asociados.", true); }
        }

        private void LoadEquipment()
        {
            ddlEquipment.Items.Clear(); ddlEquipment.Items.Add(new ListItem("-- Seleccione un equipo --", ""));
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Equipos_Listar", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            ddlEquipment.Items.Add(new ListItem(reader["TipoEquipo"] + " - " + reader["Modelo"], reader["EquipoID"].ToString()));
                }
            }
            catch (SqlException) { ShowMessage("No se pudieron cargar los equipos.", true); }
        }

        private void LoadById(int id)
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Reparaciones_Consultar", connection))
                {
                    command.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = id; connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) { ShowMessage("No se encontró una reparación con ese ID.", true); return; }
                        txtRepairId.Text = reader["ReparacionID"].ToString();
                        string equipmentId = reader["EquipoID"].ToString();
                        if (ddlEquipment.Items.FindByValue(equipmentId) != null) ddlEquipment.SelectedValue = equipmentId;
                        txtRequestDate.Text = Convert.ToDateTime(reader["FechaSolicitud"]).ToString("yyyy-MM-dd");
                        txtStatus.Text = reader["Estado"].ToString();
                    }
                }
                ShowMessage("Reparación consultada correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo consultar la reparación.", true); }
        }

        private void LoadGrid() { try { gvRepairs.DataSource = DbHelper.GetTable("sp_Reparaciones_Listar"); gvRepairs.DataBind(); } catch (SqlException) { ShowMessage("No se pudo cargar la lista de reparaciones.", true); } }

        private void AddParameters(SqlCommand command, int equipmentId, DateTime requestDate)
        {
            command.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId;
            command.Parameters.Add("@FechaSolicitud", SqlDbType.Date).Value = requestDate.Date;
            command.Parameters.Add("@Estado", SqlDbType.NVarChar, 30).Value = txtStatus.Text.Trim();
        }

        private bool ValidateData(out int equipmentId, out DateTime requestDate)
        {
            equipmentId = 0; requestDate = DateTime.MinValue;
            if (!int.TryParse(ddlEquipment.SelectedValue, out equipmentId) || equipmentId <= 0) { ShowMessage("Selecciona un equipo.", true); return false; }
            if (!DateTime.TryParse(txtRequestDate.Text, out requestDate)) { ShowMessage("Escribe una fecha válida.", true); return false; }
            if (string.IsNullOrWhiteSpace(txtStatus.Text)) { ShowMessage("Escribe el estado de la reparación.", true); return false; }
            return true;
        }

        private bool TryGetId(out int id) { if (!int.TryParse(txtRepairId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de reparación válido.", true); return false; } return true; }
        private void ClearForm() { txtRepairId.Text = ""; ddlEquipment.SelectedIndex = 0; txtRequestDate.Text = DateTime.Today.ToString("yyyy-MM-dd"); txtStatus.Text = ""; }
        private void ShowMessage(string message, bool isError) { lblMessage.Text = message; lblMessage.CssClass = isError ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
