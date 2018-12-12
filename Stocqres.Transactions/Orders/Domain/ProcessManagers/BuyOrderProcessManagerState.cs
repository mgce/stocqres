namespace Stocqres.Transactions.Orders.Domain.ProcessManagers
{
    public enum BuyOrderProcessManagerState
    {
        NotStarted,
        OrderPlaced,
        InvestorWalletCharged,
        CompanyCharged,
        StockAddedToWallet,
        OrderCompleted,
        CompanyChargeFailed,
        WalletChargeRollbacked,
        OrderFailed
    }
}
