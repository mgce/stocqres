using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Microsoft.Extensions.Configuration;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Api.Companies.Commands;
using Stocqres.Customers.Companies.Domain;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Domain.Commands;

namespace Stocqres.Customers
{
    public class Seeder
    {
        private readonly IDispatcher _dispatcher;
        private Random _random;
        private readonly string _connectionString;

        public Seeder(IDispatcher dispatcher, IConfiguration configuration)
        {
            _dispatcher = dispatcher;
            _random = new Random();
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        public void Seed()
        {
            SeedCompany();
            SeedInvestor();
        }

        private void SeedInvestor()
        {
            if (UserExist())
                return;

            var createUserCommand = new CreateInvestorCommand
            {
                Email = "test@test.pl",
                Password = "test123",
                Username = "Tester",
                FirstName = "Adam",
                LastName = "Test"
            };

            _dispatcher.SendAsync(createUserCommand);
        }

        private void SeedCompany()
        {
            if (CompanyExist())
                return;

            CreateCompany("Future Processing", "FP", 1);
            CreateCompany("FP Lab", "FPL", 100);
            CreateCompany("Progress Bar", "PGB", 1);
            CreateCompany("FP Coin", "FPC", 50);
            CreateCompany("FP Advanture", "FPA", 50);
            CreateCompany("Dadline 24", "DL24", 100);
        }

        private void CreateCompany(string name, string code, int unit)
        {
            var command = new CreateCompanyCommand(name, code, unit, GetRandom);
            _dispatcher.SendAsync(command);
        }

        private bool CompanyExist()
        {
            bool exist = false;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    exist =  conn.QueryFirstOrDefault<bool>("SELECT count(1) FROM Customers.CompanyEvents");
                }
                scope.Complete();
            }

            return exist;
        }

        private bool UserExist()
        {
            bool userExist = false;
            bool investorExist = false;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    userExist = conn.ExecuteScalar<bool>("SELECT count(1) FROM [Identity].[User]");
                    investorExist = conn.ExecuteScalar<bool>("SELECT count(1) FROM [Customers].[CompanyEvents]");
                }
                scope.Complete();
            }

            return userExist && investorExist;
        }

        public int GetRandom => _random.Next(100, 2000);
    }
}
