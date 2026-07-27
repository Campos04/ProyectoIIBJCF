using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoIIBJCF
{
    public partial class Tecnicos : System.Web.UI.Page
    {
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateTechnicianData())
            {
                return;
            }

            const string query = @"INSERT INTO Tecnicos (Nombre, Especialidad)
                                   VALUES (@Nombre, @Especialidad);";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddTechnicianParameters(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                ClearForm();
                ShowMessage("El técnico se agregó correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo agregar el técnico.", true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int technicianId;
            if (!TryGetTechnicianId(out technicianId))
            {
                return;
            }

            LoadTechnicianById(technicianId);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int technicianId;
            if (!TryGetTechnicianId(out technicianId) || !ValidateTechnicianData())
            {
                return;
            }

            const string query = @"UPDATE Tecnicos
                                   SET Nombre = @Nombre,
                                       Especialidad = @Especialidad
                                   WHERE TecnicoID = @TecnicoID;";

            try
            {
                int affectedRows;

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddTechnicianParameters(command);
                    command.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = technicianId;
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    ShowMessage("No se encontró un técnico con ese ID.", true);
                    return;
                }

                ShowMessage("El técnico se modificó correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo modificar el técnico.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int technicianId;
            if (!TryGetTechnicianId(out technicianId))
            {
                return;
            }

            const string query = "DELETE FROM Tecnicos WHERE TecnicoID = @TecnicoID;";

            try
            {
                int affectedRows;

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = technicianId;
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    ShowMessage("No se encontró un técnico con ese ID.", true);
                    return;
                }

                ClearForm();
                ShowMessage("El técnico se borró correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo borrar el técnico.", true);
            }
        }

        private void LoadTechnicianById(int technicianId)
        {
            const string query = @"SELECT TecnicoID, Nombre, Especialidad
                                   FROM Tecnicos
                                   WHERE TecnicoID = @TecnicoID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TecnicoID", SqlDbType.Int).Value = technicianId;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowMessage("No se encontró un técnico con ese ID.", true);
                            return;
                        }

                        txtTechnicianId.Text = reader["TecnicoID"].ToString();
                        txtName.Text = reader["Nombre"].ToString();
                        txtSpecialty.Text = reader["Especialidad"].ToString();
                    }
                }

                ShowMessage("Técnico consultado correctamente.", false);
            }
            catch (SqlException)
            {
                ShowMessage("No se pudo consultar el técnico.", true);
            }
        }

        private void AddTechnicianParameters(SqlCommand command)
        {
            command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
            command.Parameters.Add("@Especialidad", SqlDbType.NVarChar, 100).Value = txtSpecialty.Text.Trim();
        }

        private bool ValidateTechnicianData()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowMessage("Escribe el nombre del técnico.", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSpecialty.Text))
            {
                ShowMessage("Escribe la especialidad del técnico.", true);
                return false;
            }

            return true;
        }

        private bool TryGetTechnicianId(out int technicianId)
        {
            if (!int.TryParse(txtTechnicianId.Text.Trim(), out technicianId) || technicianId <= 0)
            {
                ShowMessage("Escribe un ID de técnico válido.", true);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtTechnicianId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSpecialty.Text = string.Empty;
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "message error" : "message success";
            lblMessage.Visible = true;
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["RepairDb"].ConnectionString;
        }
    }
}
