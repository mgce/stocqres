namespace Stocqres.Transactions.Orders.Domain.ProcessManagers
{
    public enum OrderProcessManagerState
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
