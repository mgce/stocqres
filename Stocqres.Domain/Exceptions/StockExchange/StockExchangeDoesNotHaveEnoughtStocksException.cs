using Stocqres.Core.Exceptions;


namespace Stocqres.Domain.Exceptions.StockExchange
{
    public class StockExchangeDoesNotHaveEnoughtStocksException : StocqresException
    {
        private static string code = Codes.StockExchangeCodes.StockExchangeDoesNotExist;
        private static string message = "StockCodes exchange does not have enought stocks.";

        public StockExchangeDoesNotHaveEnoughtStocksException() : base(code, message)
        {
        }
    }
}
