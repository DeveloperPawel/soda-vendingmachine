using System;

namespace VendingMachine
{
    public class VendingMachineItemPurchaseEvent : EventArgs
    {
        public int amount;
    }
}