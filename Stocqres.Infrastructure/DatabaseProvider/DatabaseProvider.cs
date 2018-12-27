using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Serilog;
using Stocqres.Infrastructure.UnitOfWork;

namespace Stocqres.Infrastructure.DatabaseProvider
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        private IDbConnection _connection => _unitOfWork.Connection;
        private IDbTransaction _transaction => _unitOfWork.Transaction;

        public DatabaseProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            try
            {
                return await _connection.QueryAsync<T>(sql, parameters, _transaction);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
                throw;
            }
            
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            try
            {
                return await _connection.ExecuteAsync(sql, parameters, _transaction);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
                throw;
            }
            
        }

        public async Task<object> ExecuteScalarAsync(string sql, object parameters = null)
        {
            try
            {
                return await _connection.ExecuteScalarAsync(sql, parameters, _transaction);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
                throw;
            }
            
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<T>(sql, parameters, _transaction);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
                throw;
            }
            
        }
    }
}
