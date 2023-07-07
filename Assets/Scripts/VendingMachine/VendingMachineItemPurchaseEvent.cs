using System;

namespace VendingMachine
{
    public class VendingMachineItemPurchaseEvent : EventArgs, ILimit
    {
        public int amount;
    }
}