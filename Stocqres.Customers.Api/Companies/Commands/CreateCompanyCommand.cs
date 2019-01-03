using Stocqres.Core.Commands;

namespace Stocqres.Customers.Api.Companies.Commands
{
    public class CreateCompanyCommand : ICommand
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }

        public CreateCompanyCommand(string name, string code, int unit, int quantity)
        {
            Name = name;
            Code = code;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
