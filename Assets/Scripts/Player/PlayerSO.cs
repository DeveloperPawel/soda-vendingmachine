using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using VendingMachine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "PlayerSO", order = 0)]
    public class PlayerSO : ScriptableObject, IProducer, IVendingConsumer
    {
        [SerializeField] protected int coin;
        protected UnityEvent<EventArgs> _event;
        public UnityEvent<EventArgs> Event => _event;

        protected virtual void OnEnable()
        {
            _event = new UnityEvent<EventArgs>();
        }

        public int GetCoinAmount()
        {
            return coin;
        }

        private void AddCoin(int amount)
        {
            coin += amount;
            var updateCoinEvent = new PlayerCoinUpdateEvent
            {
                addamount = amount
            };
            _event.Invoke(updateCoinEvent);
        }

        protected void RemoveCoin(int amount)
        {
            if (!CanSpendCoin(amount))
            {
                var playerCoinSuccessEvent = new PlayerCoinSuccessEvent
                {
                    isSuccess = false
                };
                _event?.Invoke(playerCoinSuccessEvent);
                return;
            }

            coin -= amount;
            var updateCoinEvent = new PlayerCoinUpdateEvent
            {
                addamount = amount
            };
            _event?.Invoke(updateCoinEvent);
            var SuccessEvent = new PlayerCoinSuccessEvent
            {
                isSuccess = true
            };
            _event?.Invoke(SuccessEvent);
        }

        protected bool CanSpendCoin(int amount)
        {
            return coin - amount >= 0;
        }

        public virtual void Consume(VendingMachineItemPurchaseEvent vendingMachineItemPurchaseEvent)
        {
            RemoveCoin(vendingMachineItemPurchaseEvent.amount);
        }
    }
}