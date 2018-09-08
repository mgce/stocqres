using Stocqres.Core.Exceptions;

namespace Stocqres.Domain.Exceptions.StockExchange
{
    public class StockExchangeDoesNotExistException : StocqresException
    {
        private static string code = Codes.StockExchange.StockExchangeDoesNotExist;
        private static string message = "Stock exchange does not exist";

        public StockExchangeDoesNotExistException() : base(code, message)
        {
        }
    }
}
