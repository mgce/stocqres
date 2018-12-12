namespace Stocqres.Transactions.Orders.Domain.ProcessManagers
{
    public enum SellOrderProcessManagerState
    {
        NotStarted,
        OrderPlaced,
        StocksTakedOffFromWallet,
        StocksAddedToCompany,
        WalletToppedUp,
    }
}
