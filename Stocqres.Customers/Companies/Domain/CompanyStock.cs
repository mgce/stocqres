using Stocqres.Core;

namespace Stocqres.Customers.Companies.Domain
{
    public class CompanyStock : BaseEntity
    {
        public int Unit { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
