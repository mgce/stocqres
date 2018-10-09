using Stocqres.Core.Exceptions;

namespace Stocqres.Domain.Exceptions.Stock
{
    public class StockDoesNotExistException : StocqresException
    {
        private static string code = Codes.StockCodes.StockDoesNotExist;
        private static string message = "StockCodes does not exist";

        public StockDoesNotExistException() : base(code, message)
        {
        }
    }
}
