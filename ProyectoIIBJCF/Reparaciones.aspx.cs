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
            if (!IsPostBack) { LoadEquipment(); LoadStatus(); LoadGrid(); }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int equipmentId; DateTime date; string status;
            if (!ValidateData(out equipmentId, out date, out status)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Reparaciones_Agregar", c)) { AddParameters(cmd, equipmentId, date, status); c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("La reparación se agregó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo agregar la reparación.", true); }
        }
        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id, equipmentId; DateTime date; string status;
            if (!TryGetId(out id) || !ValidateData(out equipmentId, out date, out status)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Reparaciones_Modificar", c)) { cmd.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = id; AddParameters(cmd, equipmentId, date, status); c.Open(); cmd.ExecuteNonQuery(); } LoadGrid(); ShowMessage("La reparación se modificó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo modificar la reparación.", true); }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Reparaciones_Eliminar", c)) { cmd.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = id; c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("La reparación se borró correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo borrar la reparación. Revisa si tiene asignaciones o detalles asociados.", true); }
        }
        private void LoadEquipment()
        {
            ddlEquipment.Items.Clear(); ddlEquipment.Items.Add(new ListItem("-- Seleccione un equipo --", ""));
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Equipos_Listar", c)) { c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) while (r.Read()) ddlEquipment.Items.Add(new ListItem(r["TipoEquipo"] + " - " + r["Modelo"], r["EquipoID"].ToString())); } }
            catch (SqlException) { ShowMessage("No se pudieron cargar los equipos.", true); }
        }
        private void LoadStatus() { ddlStatus.Items.Clear(); ddlStatus.Items.Add(new ListItem("-- Seleccione un estado --", "")); ddlStatus.Items.Add(new ListItem("Pendiente", "Pendiente")); ddlStatus.Items.Add(new ListItem("En proceso", "En proceso")); ddlStatus.Items.Add(new ListItem("Finalizada", "Finalizada")); }
        private void LoadById(int id)
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Reparaciones_Consultar", c)) { cmd.Parameters.Add("@ReparacionID", SqlDbType.Int).Value = id; c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) { if (!r.Read()) { ShowMessage("No se encontró una reparación con ese ID.", true); return; } txtRepairId.Text = r["ReparacionID"].ToString(); string equipmentId = r["EquipoID"].ToString(); if (ddlEquipment.Items.FindByValue(equipmentId) != null) ddlEquipment.SelectedValue = equipmentId; txtRequestDate.Text = Convert.ToDateTime(r["FechaSolicitud"]).ToString("yyyy-MM-dd"); string status = r["Estado"].ToString(); if (ddlStatus.Items.FindByValue(status) != null) ddlStatus.SelectedValue = status; } } ShowMessage("Reparación consultada correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo consultar la reparación.", true); }
        }
        private void LoadGrid()
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Reparaciones_Listar", c)) using (SqlDataAdapter a = new SqlDataAdapter(cmd)) { DataTable t = new DataTable(); a.Fill(t); gridData.DataSource = t; gridData.DataBind(); } }
            catch (SqlException) { ShowMessage("No se pudo cargar la lista de reparaciones.", true); }
        }
        private void AddParameters(SqlCommand cmd, int equipmentId, DateTime date, string status) { cmd.Parameters.Add("@EquipoID", SqlDbType.Int).Value = equipmentId; cmd.Parameters.Add("@FechaSolicitud", SqlDbType.Date).Value = date.Date; cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 30).Value = status; }
        private bool ValidateData(out int equipmentId, out DateTime date, out string status) { status = ddlStatus.SelectedValue; if (!int.TryParse(ddlEquipment.SelectedValue, out equipmentId) || equipmentId <= 0) { date = DateTime.MinValue; ShowMessage("Selecciona un equipo.", true); return false; } if (!DateTime.TryParse(txtRequestDate.Text, out date)) { ShowMessage("Selecciona una fecha válida.", true); return false; } if (string.IsNullOrWhiteSpace(status)) { ShowMessage("Selecciona un estado.", true); return false; } return true; }
        private bool TryGetId(out int id) { if (!int.TryParse(txtRepairId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de reparación válido.", true); return false; } return true; }
        private void ClearForm() { txtRepairId.Text = ""; txtRequestDate.Text = ""; if (ddlEquipment.Items.Count > 0) ddlEquipment.SelectedIndex = 0; if (ddlStatus.Items.Count > 0) ddlStatus.SelectedIndex = 0; }
        private void ShowMessage(string m, bool e) { lblMessage.Text = m; lblMessage.CssClass = e ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
