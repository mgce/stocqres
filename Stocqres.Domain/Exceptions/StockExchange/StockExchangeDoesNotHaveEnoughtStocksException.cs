using Stocqres.Core.Exceptions;


namespace Stocqres.Domain.Exceptions.StockExchange
{
    public class StockExchangeDoesNotHaveEnoughtStocksException : StocqresException
    {
        private static string code = Codes.StockExchange.StockExchangeDoesNotExist;
        private static string message = "Stock exchange does not have enought stocks.";

        public StockExchangeDoesNotHaveEnoughtStocksException() : base(code, message)
        {
        }
    }
}
