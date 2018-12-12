using System;

namespace Stocqres.SharedKernel.Stocks
{
    public class Stock 
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }

        public Stock(Guid companyId, string name, string code, int unit, int quantity)
        {
            CompanyId = companyId;
            Name = name;
            Code = code;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
