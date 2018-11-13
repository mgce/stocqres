namespace Stocqres.SharedKernel.Stocks
{
    public class Stock 
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }

        public Stock(string name, string code, int unit, int quantity)
        {
            Name = name;
            Code = code;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
