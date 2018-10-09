namespace Stocqres.Domain
{
    public static class Codes
    {
        public static class UserCodes
        {
            public static string UserExist = "user_exist";
            public static string UserDoesNotExist = "user_does_not_exist";
        }
        public static class WalletCodes
        {
            public static string WalletExist = "wallet_exist";
        }
        public static class StockCodes
        {
            public static string StockDoesNotExist = "stock_does_not_exist";
        }
        public static class StockExchangeCodes
        {
            public static string StockExchangeDoesNotExist = "stock_exchange_does_not_exist";
            public static string StockExchangeDoesNotHaveEnoughtStocks = "not_have_enought_stocks";
        }
    }
}
