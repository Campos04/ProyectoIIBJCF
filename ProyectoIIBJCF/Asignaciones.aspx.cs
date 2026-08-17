using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ProyectoIIBJCF
{
    public partial class Asignaciones : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) { LoadRepairs(); LoadTechnicians(); LoadGrid(); } }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int repairId, technicianId; DateTime date; if (!ValidateData(out repairId, out technicianId, out date)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Asignaciones_Agregar", c)) { AddParameters(cmd, repairId, technicianId, date); c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("La asignación se agregó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo agregar la asignación.", true); }
        }
        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id, repairId, technicianId; DateTime date; if (!TryGetId(out id) || !ValidateData(out repairId, out technicianId, out date)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Asignaciones_Modificar", c)) { cmd.Parameters.Add("@AsignacionID", SqlDbType.Int).Value = id; AddParameters(cmd, repairId, technicianId, date); c.Open(); cmd.ExecuteNonQuery(); } LoadGrid(); ShowMessage("La asignación se modificó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo modificar la asignación.", true); }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Asignaciones_Eliminar", c)) { cmd.Parameters.Add("@AsignacionID", SqlDbType.Int).Value = id; c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("La asignación se borró correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo borrar la asignación.", true); }
        }
        private void LoadRepairs()
        {
            ddlRepair.Items.Clear(); ddlRepair.Items.Add(new ListItem("-- Seleccione una reparación --", ""));
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Reparaciones_Listar", c)) { c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) while (r.Read()) ddlRepair.Items.Add(new ListItem("#" + r["ReparacionID"] + " - " + r["Equipo"], r["ReparacionID"].ToString())); } }
            catch (SqlException) { ShowMessage("No se pudieron cargar las reparaciones.", true); }
        }
        private void LoadTechnicians()
        {
            ddlTechnician.Items.Clear(); ddlTechnician.Items.Add(new ListItem("-- Seleccione un técnico --", ""));
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Tecnicos_Listar", c)) { c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) while (r.Read()) ddlTechnician.Items.Add(new ListItem(r["Nombre"].ToString(), r["TecnicoID"].ToString())); } }
            catch (SqlException) { ShowMessage("No se pudieron cargar los técnicos.", true); }
        }
        private void LoadById(int id)
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Asignaciones_Consultar", c)) { cmd.Parameters.Add("@AsignacionID", SqlDbType.Int).Value = id; c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) { if (!r.Read()) { ShowMessage("No se encontró una asignación con ese ID.", true); return; } txtAssignmentId.Text = r["AsignacionID"].ToString(); string repairId = r["ReparacionID"].ToString(); string technicianId = r["TecnicoID"].ToString(); if (ddlRepair.Items.FindByValue(repairId) != null) ddlRepair.SelectedValue = repairId; if (ddlTechnician.Items.FindByValue(technicianId) != null) ddlTechnician.SelectedValue = technicianId; txtAssignmentDate.Text = Convert.ToDateTime(r["FechaAsignacion"]).ToString("yyyy-MM-dd"); } } ShowMessage("Asignación consultada correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo consultar la asignación.", true); }
        }
        private void LoadGrid()
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Asignaciones_Listar", c)) using (SqlDataAdapter a = new SqlDataAdapter(cmd)) { DataTable t = new DataTable(); a.Fill(t); gridData.DataSource = t; gridData.DataBind(); } }
            catch (SqlException) { ShowMessage("No se pudo cargar la lista de asignaciones.", true); }
        }
        private void AddParameters(SqlCommand cmd, int repairId, int technicianId, DateTime date) { cmd.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = repairId; cmd.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = technicianId; cmd.Parameters.Add("@FechaAsignacion", SqlDbType.Date).Value = date.Date; }
        private bool ValidateData(out int repairId, out int technicianId, out DateTime date) { if (!int.TryParse(ddlRepair.SelectedValue, out repairId) || repairId <= 0) { technicianId = 0; date = DateTime.MinValue; ShowMessage("Selecciona una reparación.", true); return false; } if (!int.TryParse(ddlTechnician.SelectedValue, out technicianId) || technicianId <= 0) { date = DateTime.MinValue; ShowMessage("Selecciona un técnico.", true); return false; } if (!DateTime.TryParse(txtAssignmentDate.Text, out date)) { ShowMessage("Selecciona una fecha válida.", true); return false; } return true; }
        private bool TryGetId(out int id) { if (!int.TryParse(txtAssignmentId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de asignación válido.", true); return false; } return true; }
        private void ClearForm() { txtAssignmentId.Text = ""; txtAssignmentDate.Text = ""; if (ddlRepair.Items.Count > 0) ddlRepair.SelectedIndex = 0; if (ddlTechnician.Items.Count > 0) ddlTechnician.SelectedIndex = 0; }
        private void ShowMessage(string m, bool e) { lblMessage.Text = m; lblMessage.CssClass = e ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
