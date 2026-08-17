using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ProyectoIIBJCF
{
    public partial class Asignaciones : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { LoadRepairs(); LoadTechnicians(); LoadGrid(); txtAssignmentDate.Text = DateTime.Today.ToString("yyyy-MM-dd"); }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int repairId, technicianId; DateTime date;
            if (!ValidateData(out repairId, out technicianId, out date)) return;
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Asignaciones_Agregar", connection))
                {
                    AddParameters(command, repairId, technicianId, date); connection.Open(); command.ExecuteNonQuery();
                }
                ClearForm(); LoadGrid(); ShowMessage("La asignación se agregó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo agregar la asignación.", true); }
        }

        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id, repairId, technicianId; DateTime date;
            if (!TryGetId(out id) || !ValidateData(out repairId, out technicianId, out date)) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Asignaciones_Modificar", connection))
                {
                    command.Parameters.Add("@AsignacionID", SqlDbType.Int).Value = id;
                    AddParameters(command, repairId, technicianId, date); connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró una asignación con ese ID.", true); return; }
                LoadGrid(); ShowMessage("La asignación se modificó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo modificar la asignación.", true); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Asignaciones_Borrar", connection))
                {
                    command.Parameters.Add("@AsignacionID", SqlDbType.Int).Value = id; connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró una asignación con ese ID.", true); return; }
                ClearForm(); LoadGrid(); ShowMessage("La asignación se borró correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo borrar la asignación.", true); }
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

        private void LoadTechnicians()
        {
            ddlTechnicians.Items.Clear(); ddlTechnicians.Items.Add(new ListItem("-- Seleccione un técnico --", ""));
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Tecnicos_Listar", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            ddlTechnicians.Items.Add(new ListItem(reader["Nombre"].ToString(), reader["TecnicoID"].ToString()));
                }
            }
            catch (SqlException) { ShowMessage("No se pudieron cargar los técnicos.", true); }
        }

        private void LoadById(int id)
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Asignaciones_Consultar", connection))
                {
                    command.Parameters.Add("@AsignacionID", SqlDbType.Int).Value = id; connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) { ShowMessage("No se encontró una asignación con ese ID.", true); return; }
                        txtAssignmentId.Text = reader["AsignacionID"].ToString();
                        string repairId = reader["ReparacionID"].ToString();
                        string technicianId = reader["TecnicoID"].ToString();
                        if (ddlRepairs.Items.FindByValue(repairId) != null) ddlRepairs.SelectedValue = repairId;
                        if (ddlTechnicians.Items.FindByValue(technicianId) != null) ddlTechnicians.SelectedValue = technicianId;
                        txtAssignmentDate.Text = Convert.ToDateTime(reader["FechaAsignacion"]).ToString("yyyy-MM-dd");
                    }
                }
                ShowMessage("Asignación consultada correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo consultar la asignación.", true); }
        }

        private void LoadGrid() { try { gvAssignments.DataSource = DbHelper.GetTable("sp_Asignaciones_Listar"); gvAssignments.DataBind(); } catch (SqlException) { ShowMessage("No se pudo cargar la lista de asignaciones.", true); } }
        private void AddParameters(SqlCommand command, int repairId, int technicianId, DateTime date)
        {
            command.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = repairId;
            command.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = technicianId;
            command.Parameters.Add("@FechaAsignacion", SqlDbType.Date).Value = date.Date;
        }
        private bool ValidateData(out int repairId, out int technicianId, out DateTime date)
        {
            repairId = 0; technicianId = 0; date = DateTime.MinValue;
            if (!int.TryParse(ddlRepairs.SelectedValue, out repairId) || repairId <= 0) { ShowMessage("Selecciona una reparación.", true); return false; }
            if (!int.TryParse(ddlTechnicians.SelectedValue, out technicianId) || technicianId <= 0) { ShowMessage("Selecciona un técnico.", true); return false; }
            if (!DateTime.TryParse(txtAssignmentDate.Text, out date)) { ShowMessage("Escribe una fecha válida.", true); return false; }
            return true;
        }
        private bool TryGetId(out int id) { if (!int.TryParse(txtAssignmentId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de asignación válido.", true); return false; } return true; }
        private void ClearForm() { txtAssignmentId.Text = ""; ddlRepairs.SelectedIndex = 0; ddlTechnicians.SelectedIndex = 0; txtAssignmentDate.Text = DateTime.Today.ToString("yyyy-MM-dd"); }
        private void ShowMessage(string message, bool isError) { lblMessage.Text = message; lblMessage.CssClass = isError ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
