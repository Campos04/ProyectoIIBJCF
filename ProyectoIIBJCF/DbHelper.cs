using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoIIBJCF
{
    public static class DbHelper
    {
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["RepairDb"].ConnectionString);
        }

        public static SqlCommand CreateProcedure(string procedureName, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

        public static DataTable GetTable(string procedureName)
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = CreateProcedure(procedureName, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(table);
            }

            return table;
        }
    }
}
