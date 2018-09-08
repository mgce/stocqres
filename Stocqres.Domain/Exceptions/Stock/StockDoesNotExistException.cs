using Stocqres.Core.Exceptions;

namespace Stocqres.Domain.Exceptions.Stock
{
    public class StockDoesNotExistException : StocqresException
    {
        private static string code = Codes.Stock.StockDoesNotExist;
        private static string message = "Stock does not exist";

        public StockDoesNotExistException() : base(code, message)
        {
        }
    }
}
