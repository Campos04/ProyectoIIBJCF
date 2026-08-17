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
            if (!ValidateTechnicianData()) return;
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Tecnicos_Agregar", connection))
                {
                    AddParameters(command); connection.Open(); command.ExecuteNonQuery();
                }
                ClearForm(); LoadGrid(); ShowMessage("El técnico se agregó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo agregar el técnico.", true); }
        }

        protected void btnSearch_Click(object sender, EventArgs e) { int id; if (TryGetId(out id)) LoadById(id); }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id;
            if (!TryGetId(out id) || !ValidateTechnicianData()) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Tecnicos_Modificar", connection))
                {
                    command.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = id;
                    AddParameters(command); connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró un técnico con ese ID.", true); return; }
                LoadGrid(); ShowMessage("El técnico se modificó correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo modificar el técnico.", true); }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id; if (!TryGetId(out id)) return;
            try
            {
                int rows;
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Tecnicos_Borrar", connection))
                {
                    command.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = id;
                    connection.Open(); rows = Convert.ToInt32(command.ExecuteScalar());
                }
                if (rows == 0) { ShowMessage("No se encontró un técnico con ese ID.", true); return; }
                ClearForm(); LoadGrid(); ShowMessage("El técnico se borró correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo borrar el técnico. Puede tener asignaciones asociadas.", true); }
        }

        private void LoadById(int id)
        {
            try
            {
                using (SqlConnection connection = DbHelper.CreateConnection())
                using (SqlCommand command = DbHelper.CreateProcedure("sp_Tecnicos_Consultar", connection))
                {
                    command.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = id; connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) { ShowMessage("No se encontró un técnico con ese ID.", true); return; }
                        txtTechnicianId.Text = reader["TecnicoID"].ToString();
                        txtName.Text = reader["Nombre"].ToString();
                        txtSpecialty.Text = reader["Especialidad"].ToString();
                    }
                }
                ShowMessage("Técnico consultado correctamente.", false);
            }
            catch (SqlException) { ShowMessage("No se pudo consultar el técnico.", true); }
        }

        private void LoadGrid()
        {
            try { gvTechnicians.DataSource = DbHelper.GetTable("sp_Tecnicos_Listar"); gvTechnicians.DataBind(); }
            catch (SqlException) { ShowMessage("No se pudo cargar la lista de técnicos.", true); }
        }

        private void AddParameters(SqlCommand command)
        {
            command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
            command.Parameters.Add("@Especialidad", SqlDbType.NVarChar, 100).Value = txtSpecialty.Text.Trim();
        }
        private bool ValidateTechnicianData()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) { ShowMessage("Escribe el nombre del técnico.", true); return false; }
            if (string.IsNullOrWhiteSpace(txtSpecialty.Text)) { ShowMessage("Escribe la especialidad del técnico.", true); return false; }
            return true;
        }
        private bool TryGetId(out int id) { if (!int.TryParse(txtTechnicianId.Text.Trim(), out id) || id <= 0) { ShowMessage("Escribe un ID de técnico válido.", true); return false; } return true; }
        private void ClearForm() { txtTechnicianId.Text = ""; txtName.Text = ""; txtSpecialty.Text = ""; }
        private void ShowMessage(string message, bool isError) { lblMessage.Text = message; lblMessage.CssClass = isError ? "message error" : "message success"; lblMessage.Visible = true; }
    }
}
