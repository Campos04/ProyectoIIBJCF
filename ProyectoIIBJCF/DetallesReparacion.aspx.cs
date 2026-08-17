using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ProyectoIIBJCF
{
    public partial class DetallesReparacion : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) { LoadRepairs(); LoadGrid(); } }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int repairId; DateTime? startDate, endDate; if (!ValidateData(out repairId, out startDate, out endDate)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_DetallesReparacion_Agregar", c)) { AddParameters(cmd, repairId, startDate, endDate); c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("El detalle se agregó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo agregar el detalle.", true); }
        }
        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id, repairId; DateTime? startDate, endDate; if (!TryGetId(out id) || !ValidateData(out repairId, out startDate, out endDate)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_DetallesReparacion_Modificar", c)) { cmd.Parameters.Add("@DetalleID", SqlDbType.Int).Value = id; AddParameters(cmd, repairId, startDate, endDate); c.Open(); cmd.ExecuteNonQuery(); } LoadGrid(); ShowMessage("El detalle se modificó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo modificar el detalle.", true); }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_DetallesReparacion_Eliminar", c)) { cmd.Parameters.Add("@DetalleID", SqlDbType.Int).Value = id; c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("El detalle se borró correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo borrar el detalle.", true); }
        }
        private void LoadRepairs()
        {
            ddlRepair.Items.Clear(); ddlRepair.Items.Add(new ListItem("-- Seleccione una reparación --", ""));
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Reparaciones_Listar", c)) { c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) while (r.Read()) ddlRepair.Items.Add(new ListItem("#" + r["ReparacionID"] + " - " + r["Equipo"], r["ReparacionID"].ToString())); } }
            catch (SqlException) { ShowMessage("No se pudieron cargar las reparaciones.", true); }
        }
        private void LoadById(int id)
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_DetallesReparacion_Consultar", c)) { cmd.Parameters.Add("@DetalleID", SqlDbType.Int).Value = id; c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) { if (!r.Read()) { ShowMessage("No se encontró un detalle con ese ID.", true); return; } txtDetailId.Text = r["DetalleID"].ToString(); string repairId = r["ReparacionID"].ToString(); if (ddlRepair.Items.FindByValue(repairId) != null) ddlRepair.SelectedValue = repairId; txtDescription.Text = r["Descripcion"].ToString(); txtStartDate.Text = r["FechaInicio"] == DBNull.Value ? "" : Convert.ToDateTime(r["FechaInicio"]).ToString("yyyy-MM-dd"); txtEndDate.Text = r["FechaFin"] == DBNull.Value ? "" : Convert.ToDateTime(r["FechaFin"]).ToString("yyyy-MM-dd"); } } ShowMessage("Detalle consultado correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo consultar el detalle.", true); }
        }
        private void LoadGrid()
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_DetallesReparacion_Listar", c)) using (SqlDataAdapter a = new SqlDataAdapter(cmd)) { DataTable t = new DataTable(); a.Fill(t); gridData.DataSource = t; gridData.DataBind(); } }
            catch (SqlException) { ShowMessage("No se pudo cargar la lista de detalles.", true); }
        }
        private void AddParameters(SqlCommand cmd, int repairId, DateTime? startDate, DateTime? endDate) { cmd.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = repairId; cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = txtDescription.Text.Trim(); cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = startDate.HasValue ? (object)startDate.Value.Date : DBNull.Value; cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = endDate.HasValue ? (object)endDate.Value.Date : DBNull.Value; }
        private bool ValidateData(out int repairId, out DateTime? startDate, out DateTime? endDate)
        {
            startDate = null; endDate = null;
            if (!int.TryParse(ddlRepair.SelectedValue, out repairId) || repairId <= 0) { ShowMessage("Selecciona una reparación.", true); return false; }
            if (string.IsNullOrWhiteSpace(txtDescription.Text)) { ShowMessage("Escribe la descripción del trabajo.", true); return false; }
            DateTime temp;
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text)) { if (!DateTime.TryParse(txtStartDate.Text, out temp)) { ShowMessage("La fecha de inicio no es válida.", true); return false; } startDate = temp; }
            if (!string.IsNullOrWhiteSpace(txtEndDate.Text)) { if (!DateTime.TryParse(txtEndDate.Text, out temp)) { ShowMessage("La fecha de fin no es válida.", true); return false; } endDate = temp; }
            if (startDate.HasValue && endDate.HasValue && endDate.Value.Date < startDate.Value.Date) { ShowMessage("La fecha de fin no puede ser anterior a la fecha de inicio.", true); return false; }
            return true;
        }
        private bool TryGetId(out int id) { if (!int.TryParse(txtDetailId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de detalle válido.", true); return false; } return true; }
        private void ClearForm() { txtDetailId.Text = ""; txtDescription.Text = ""; txtStartDate.Text = ""; txtEndDate.Text = ""; if (ddlRepair.Items.Count > 0) ddlRepair.SelectedIndex = 0; }
        private void ShowMessage(string m, bool e) { lblMessage.Text = m; lblMessage.CssClass = e ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
