using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Dispatcher;
using Stocqres.Customers.Companies.Domain.Commands;

namespace Stocqres.Customers
{
    public class CustomerSeed
    {
        private readonly IDispatcher _dispatcher;
        private Random _random;

        public CustomerSeed(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _random = new Random();
        }

        public void Seed()
        {
            CreateCompany("Future Processing", "FP", 1);
            CreateCompany("FP Lab", "FPL", 100);
            CreateCompany("Progress Bar", "PGB", 1);
            CreateCompany("FP Coin", "FPC", 50);
            CreateCompany("FP Advanture", "FPA", 50);
            CreateCompany("Dadline 24", "DL24", 100);
        }

        public void CreateCompany(string name, string code, int unit)
        {
            var command = new CreateCompanyCommand(name, code, unit, GetRandom);
            _dispatcher.SendAsync(command);
        }

        public int GetRandom => _random.Next(100, 2000);
    }
}
