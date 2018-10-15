using Stocqres.Core.Domain;
using Stocqres.Domain;

namespace Stocqres.Customers.Companies.Domain
{
    public class Company : AggregateRoot
    {
        public string Name { get; set; }
        public Stock Stock { get; set; }

        public Company(string name, Stock stock)
        {
            Name = name;
            Stock = stock;
        }
    }
}
