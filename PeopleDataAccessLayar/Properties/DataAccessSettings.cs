using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PersonManagement.DAL.Repositories
{
    public class DataAccessSettings
    {
        public static string _connectionString = string.Empty;
       

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
