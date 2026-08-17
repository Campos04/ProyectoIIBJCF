using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ProyectoIIBJCF
{
    public partial class DetallesReparacion : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { LoadRepairs(); LoadGrid(); }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int repairId; DateTime? startDate, endDate;
            if (!ValidateData(out repairId, out startDate, out endDate)) return;
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Detalles_Agregar", connection))
                {
                    AddParameters(command, repairId, startDate, endDate); connection.Open(); command.ExecuteNonQuery();
                }
                ClearForm(); LoadGrid(); ShowMessage("El detalle se agregó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo agregar el detalle.", true); }
        }

        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id, repairId; DateTime? startDate, endDate;
            if (!TryGetId(out id) || !ValidateData(out repairId, out startDate, out endDate)) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Detalles_Modificar", connection))
                {
                    command.Parameters.Add("@DetalleID", SqlDbType.Int).Value = id;
                    AddParameters(command, repairId, startDate, endDate); connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró un detalle con ese ID.", true); return; }
                LoadGrid(); ShowMessage("El detalle se modificó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo modificar el detalle.", true); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Detalles_Borrar", connection))
                {
                    command.Parameters.Add("@DetalleID", SqlDbType.Int).Value = id; connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró un detalle con ese ID.", true); return; }
                ClearForm(); LoadGrid(); ShowMessage("El detalle se borró correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo borrar el detalle.", true); }
        }

        private void LoadRepairs()
        {
            ddlRepairs.Items.Clear(); ddlRepairs.Items.Add(new ListItem("-- Seleccione una reparación --", ""));
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Reparaciones_Listar", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            ddlRepairs.Items.Add(new ListItem("Reparación #" + reader["ReparacionID"] + " - " + reader["Equipo"], reader["ReparacionID"].ToString()));
                }
            }
            catch (SqlException) { ShowMessage("No se pudieron cargar las reparaciones.", true); }
        }

        private void LoadById(int id)
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Detalles_Consultar", connection))
                {
                    command.Parameters.Add("@DetalleID", SqlDbType.Int).Value = id; connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) { ShowMessage("No se encontró un detalle con ese ID.", true); return; }
                        txtDetailId.Text = reader["DetalleID"].ToString();
                        string repairId = reader["ReparacionID"].ToString();
                        if (ddlRepairs.Items.FindByValue(repairId) != null) ddlRepairs.SelectedValue = repairId;
                        txtDescription.Text = reader["Descripcion"].ToString();
                        txtStartDate.Text = reader["FechaInicio"] == DBNull.Value ? "" : Convert.ToDateTime(reader["FechaInicio"]).ToString("yyyy-MM-dd");
                        txtEndDate.Text = reader["FechaFin"] == DBNull.Value ? "" : Convert.ToDateTime(reader["FechaFin"]).ToString("yyyy-MM-dd");
                    }
                }
                ShowMessage("Detalle consultado correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo consultar el detalle.", true); }
        }

        private void LoadGrid() { try { gvDetails.DataSource = DbHelper.GetTable("sp_Detalles_Listar"); gvDetails.DataBind(); } catch (SqlException) { ShowMessage("No se pudo cargar la lista de detalles.", true); } }

        private void AddParameters(SqlCommand command, int repairId, DateTime? startDate, DateTime? endDate)
        {
            command.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = repairId;
            command.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = txtDescription.Text.Trim();
            command.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = startDate.HasValue ? (object)startDate.Value.Date : DBNull.Value;
            command.Parameters.Add("@FechaFin", SqlDbType.Date).Value = endDate.HasValue ? (object)endDate.Value.Date : DBNull.Value;
        }

        private bool ValidateData(out int repairId, out DateTime? startDate, out DateTime? endDate)
        {
            repairId = 0; startDate = null; endDate = null;
            if (!int.TryParse(ddlRepairs.SelectedValue, out repairId) || repairId <= 0) { ShowMessage("Selecciona una reparación.", true); return false; }
            if (string.IsNullOrWhiteSpace(txtDescription.Text)) { ShowMessage("Escribe la descripción del trabajo.", true); return false; }

            DateTime date;
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                if (!DateTime.TryParse(txtStartDate.Text, out date)) { ShowMessage("La fecha de inicio no es válida.", true); return false; }
                startDate = date;
            }
            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                if (!DateTime.TryParse(txtEndDate.Text, out date)) { ShowMessage("La fecha de fin no es válida.", true); return false; }
                endDate = date;
            }
            if (startDate.HasValue && endDate.HasValue && endDate.Value.Date < startDate.Value.Date) { ShowMessage("La fecha de fin no puede ser anterior a la fecha de inicio.", true); return false; }
            return true;
        }

        private bool TryGetId(out int id) { if (!int.TryParse(txtDetailId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de detalle válido.", true); return false; } return true; }
        private void ClearForm() { txtDetailId.Text = ""; ddlRepairs.SelectedIndex = 0; txtDescription.Text = ""; txtStartDate.Text = ""; txtEndDate.Text = ""; }
        private void ShowMessage(string message, bool isError) { lblMessage.Text = message; lblMessage.CssClass = isError ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
