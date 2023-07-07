namespace VendingMachine
{
    public interface IVendingConsumer
    {
        void Consume(VendingMachineItemPurchaseEvent vendingMachineItemPurchaseEvent);
    }
}