using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PersonManagement.DAL.Repositories
{
    public class PersonRepository
    {
        private readonly string _connectionString;
        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
