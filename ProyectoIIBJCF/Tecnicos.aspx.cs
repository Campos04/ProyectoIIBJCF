using System;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoIIBJCF
{
    public partial class Tecnicos : SecurePage
    {
        protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) LoadGrid(); }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Tecnicos_Agregar", c)) { AddParameters(cmd); c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("El técnico se agregó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo agregar el técnico.", true); }
        }
        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id) || !ValidateData()) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Tecnicos_Modificar", c)) { cmd.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = id; AddParameters(cmd); c.Open(); cmd.ExecuteNonQuery(); } LoadGrid(); ShowMessage("El técnico se modificó correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo modificar el técnico.", true); }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Tecnicos_Eliminar", c)) { cmd.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = id; c.Open(); cmd.ExecuteNonQuery(); } ClearForm(); LoadGrid(); ShowMessage("El técnico se borró correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo borrar el técnico. Revisa si tiene asignaciones asociadas.", true); }
        }
        private void LoadById(int id)
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Tecnicos_Consultar", c)) { cmd.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = id; c.Open(); using (SqlDataReader r = cmd.ExecuteReader()) { if (!r.Read()) { ShowMessage("No se encontró un técnico con ese ID.", true); return; } txtTechnicianId.Text = r["TecnicoID"].ToString(); txtName.Text = r["Nombre"].ToString(); txtSpecialty.Text = r["Especialidad"].ToString(); } } ShowMessage("Técnico consultado correctamente.", false); }
            catch (SqlException) { ShowMessage("No se pudo consultar el técnico.", true); }
        }
        private void LoadGrid()
        {
            try { using (SqlConnection c = DbHelper.CreateConnection()) using (SqlCommand cmd = DbHelper.CreateStoredProcedureCommand("sp_Tecnicos_Listar", c)) using (SqlDataAdapter a = new SqlDataAdapter(cmd)) { DataTable t = new DataTable(); a.Fill(t); gridData.DataSource = t; gridData.DataBind(); } }
            catch (SqlException) { ShowMessage("No se pudo cargar la lista de técnicos.", true); }
        }
        private void AddParameters(SqlCommand cmd) { cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim(); cmd.Parameters.Add("@Especialidad", SqlDbType.NVarChar, 100).Value = txtSpecialty.Text.Trim(); }
        private bool ValidateData() { if (string.IsNullOrWhiteSpace(txtName.Text)) { ShowMessage("Escribe el nombre del técnico.", true); return false; } if (string.IsNullOrWhiteSpace(txtSpecialty.Text)) { ShowMessage("Escribe la especialidad.", true); return false; } return true; }
        private bool TryGetId(out int id) { if (!int.TryParse(txtTechnicianId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de técnico válido.", true); return false; } return true; }
        private void ClearForm() { txtTechnicianId.Text = ""; txtName.Text = ""; txtSpecialty.Text = ""; }
        private void ShowMessage(string m, bool e) { lblMessage.Text = m; lblMessage.CssClass = e ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
