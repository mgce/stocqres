using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Stocqres.Core.Mongo;

namespace Stocqres.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IClientSessionHandle _mongoSession;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private readonly MongoOptions _mongoOptions;

        private int _transactionCounter = 0;

        public UnitOfWork(IConfiguration configuration, IClientSessionHandle mongoSession, MongoOptions mongoOptions)
        {
            _mongoSession = mongoSession;
            _mongoOptions = mongoOptions;
            _connection = new SqlConnection(configuration.GetConnectionString("SqlServer"));
            _connection.Open();
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;
        public IMongoDatabase MongoDatabase => _mongoSession.Client.GetDatabase(_mongoOptions.Database);

        public void BeginTransaction()
        {
            if (_transactionCounter == 0)
            {
                _transaction = Connection.BeginTransaction(IsolationLevel.Serializable);
                _mongoSession.StartTransaction();
            }

            _transactionCounter++;
        }

        public void Commit()
        {
            if (_transactionCounter == 1)
                try
                {
                    Transaction.Commit();
                    _mongoSession.CommitTransaction();
                }
                catch
                {
                    Transaction.Rollback();
                    _mongoSession.AbortTransaction();
                    throw;
                }
                finally
                {
                    Transaction.Dispose();
                    _mongoSession.Dispose();
                }

            _transactionCounter--;
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Transaction.Dispose();
            _mongoSession.AbortTransaction();
            _mongoSession.Dispose();

            _transactionCounter = 0;
        }

    }
}
