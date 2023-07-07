using System;
using Interfaces;
using Player;
using Service.Events;
using UnityEngine;
using UnityEngine.Events;
using VendingMachine;

namespace Service
{
    [CreateAssetMenu(fileName = "GameService", menuName = "Service/Game", order = 0)]
    public class GameService : Service, IProducer, Initializable, IPlayerSOConsumer, IVendingConsumer
    {
        protected bool isInitialized;
        private EventArgs _eventArgs;
        protected override void OnEnable()
        {
            base.OnEnable();
            Initialize();
            Init?.Invoke();
        }

        public UnityEvent<EventArgs> Event => _event;
        public event Action Init;
        public bool Initialized => isInitialized;
        public void Initialize()
        {
            GameServiceStart gameServiceStart = new GameServiceStart();
            InvokeEvent(gameServiceStart);
            isInitialized = true;
        }

        private void InvokeEvent(EventArgs args)
        {
            _event?.Invoke(args);
            SetEvent(args);
        }

        private void SetEvent(EventArgs args)
        {
            _eventArgs = args;
        }

        public EventArgs GetEvent()
        {
            return _eventArgs;
        }

        public void Consume(PlayerCoinUpdateEvent playerCoinUpdateEvent)
        {
            _event.Invoke(playerCoinUpdateEvent);
        }

        public void Consume(PlayerCoinSuccessEvent playerCoinSuccessEvent)
        {
            _event.Invoke(playerCoinSuccessEvent);
        }

        public void Consume(VendingMachineItemPurchaseEvent vendingMachineItemPurchaseEvent)
        {
            _event.Invoke(vendingMachineItemPurchaseEvent);
        }
    }
}