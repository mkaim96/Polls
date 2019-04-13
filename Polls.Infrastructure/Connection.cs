using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Polls.Infrastructure
{
    public static class Connection
    {
        public static IDbConnection GetConnection()
        {
            return new SqlConnection("Server=(LocalDb)\\MSSQLLocalDB;Database=Polls_Db;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
    }
}
