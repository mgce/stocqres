namespace Stocqres.Domain
{
    public static class Codes
    {
        public static class User
        {
            public static string UserExist = "user_exist";
            public static string UserDoesNotExist = "user_does_not_exist";
        }
        public static class Wallet
        {
            public static string WalletExist = "wallet_exist";
        }
        public static class Stock
        {
            public static string StockDoesNotExist = "stock_does_not_exist";
        }
        public static class StockExchange
        {
            public static string StockExchangeDoesNotExist = "stock_exchange_does_not_exist";
            public static string StockExchangeDoesNotHaveEnoughtStocks = "not_have_enought_stocks";
        }
    }
}
