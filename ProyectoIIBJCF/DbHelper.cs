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

        public static SqlCommand CreateStoredProcedureCommand(string procedureName, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }
    }
}
