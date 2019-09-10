using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Polls.Infrastructure.UnitOfWork
{
    public class PollsContext
    {
        public IDbConnection Conn;
        public  IDbTransaction Transaction;

        public PollsContext()
        {
            Conn = Connection.GetConnection();
            Conn.Open();
            Transaction = Conn.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
                Transaction.Connection?.Close();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction?.Dispose();
                Transaction.Connection?.Dispose();
                Transaction = null;
            }
        }
    }
}
