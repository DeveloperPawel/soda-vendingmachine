using System;
using System.Collections.Generic;
using System.Reflection;
using Controllers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VendingMachine
{
    public class VendingMachine : Controller, IPlayerSOConsumer
    {
        [SerializeField] private List<VendingItemSO> vendingItems;
        [SerializeField] private GameObject buttonPrefab;
        
        private State state;

        private void Awake()
        {
            state = new WaitingPayment();
        }

        protected override void Start()
        {
            base.Start();
            foreach (var vendingItem in vendingItems)
            {
                GameObject buttonGO = Instantiate(buttonPrefab, this.transform);
                Button button = buttonGO.GetComponent<Button>();
                TMP_Text text = buttonGO.GetComponentInChildren<TMP_Text>();
                text.text = $"{vendingItem.GetName()} {vendingItem.GetPrice()}";
                button.onClick.AddListener(() =>
                {
                    string itemID = vendingItem.GetID();
                    Purchase(itemID);
                });
            }
        }

        private void ChangeState()
        {
            switch (state)
            {
                case WaitingPayment waitingPayment:
                    state = new VendItem();
                    break;
                case VendItem vendItem:
                    state = new WaitingPayment();
                    break;
                default:
                    Debug.LogError("Could not find state");
                    break;
            }
        }
        
        private void Purchase(string itemID)
        {
            int price = 0;
            foreach (var vendingItem in vendingItems)
            {
                if (itemID.Equals(vendingItem.GetID()))
                {
                    price = vendingItem.GetPrice();
                }
            }

            if (price == 0) return;
            gameService.Consume(new VendingMachineItemPurchaseEvent{amount = price});
        }

        public void Consume(PlayerCoinUpdateEvent playerCoinUpdateEvent)
        {
            if (playerCoinUpdateEvent.addamount > 0) return;
            Debug.Log($"Vending Machine: recieved {playerCoinUpdateEvent.addamount}");
        }

        public void Consume(PlayerCoinSuccessEvent playerCoinSuccessEvent)
        {
            if (playerCoinSuccessEvent.isSuccess)
            {
                ChangeState();
                state.Action();
                ChangeState();
            }
            state.Action();
        }
    }

    public abstract class State
    {
        public abstract void Action();
    }

    public class WaitingPayment : State
    {
        public override void Action()
        {
            Debug.Log("Waiting for payment");
        }
    }

    public class VendItem : State
    {
        public override void Action()
        {
            Debug.Log("Producing Item");
        }
    }
}