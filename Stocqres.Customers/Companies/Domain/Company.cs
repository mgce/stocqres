using Stocqres.Core.Domain;

namespace Stocqres.Customers.Companies.Domain
{
    public class Company : AggregateRoot
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public CompanyStock CompanyStock { get; set; }
    }
}
