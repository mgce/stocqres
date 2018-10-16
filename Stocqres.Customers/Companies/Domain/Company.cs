using System;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Customers.Companies.Domain.Events;
using Stocqres.SharedKernel.Stocks;

namespace Stocqres.Customers.Companies.Domain
{
    public class Company : AggregateRoot
    {
        public string Name { get; protected set; }
        public Stock Stock { get; protected set; }

        public Company(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new StocqresException("Comapny name cannot be null or empty");

            Publish(new CompanyCreatedEvent(Guid.NewGuid(), name));
        }

        public void CreateCompanyStock(string code, int unit, int quantity)
        {
            if (string.IsNullOrEmpty(code))
                throw new StocqresException("Stock code cannot be null or empty");

            if (unit <= 0)
                throw new StocqresException("Stock unit must be greater than zero");

            if (quantity <= 0)
                throw new StocqresException("Stock quantity must be greater than zero");

            Publish(new CompanyStockCreatedEvent(Id, code, unit, quantity));
        }

        public void Apply(CompanyCreatedEvent @event)
        {
            Id = @event.AggregateId;
            Name = @event.Name;
        }

        public void Apply(CompanyStockCreatedEvent @event)
        {
            Id = @event.AggregateId;
            Stock = new Stock(Name, @event.Code, @event.Unit, @event.Quantity);
        }
    }
}
