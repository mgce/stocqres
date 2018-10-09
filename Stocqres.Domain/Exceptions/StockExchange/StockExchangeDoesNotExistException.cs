using Stocqres.Core.Exceptions;

namespace Stocqres.Domain.Exceptions.StockExchange
{
    public class StockExchangeDoesNotExistException : StocqresException
    {
        private static string code = Codes.StockExchangeCodes.StockExchangeDoesNotExist;
        private static string message = "StockCodes exchange does not exist";

        public StockExchangeDoesNotExistException() : base(code, message)
        {
        }
    }
}
