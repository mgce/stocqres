using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Stocqres.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private int _transactionCounter = 0;

        public UnitOfWork(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("SqlServer"));
            _connection.Open();
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;

        public void BeginTransaction()
        {
            if (_transactionCounter == 0)
                _transaction = Connection.BeginTransaction(IsolationLevel.Serializable);

            _transactionCounter++;
        }

        public void Commit()
        {
            if (_transactionCounter == 1)
                try
                {
                    Transaction.Commit();
                }
                catch
                {
                    Transaction.Rollback();
                    throw;
                }
                finally
                {
                    Transaction.Dispose();
                }

            _transactionCounter--;
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Transaction.Dispose();
            _transactionCounter = 0;
        }

    }
}
