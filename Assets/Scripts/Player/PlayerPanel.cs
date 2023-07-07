using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Interfaces;
using Service;
using Service.Events;
using TMPro;
using UnityEngine;
using VendingMachine;
using ActionRunner;

namespace Player
{
    public class PlayerPanel : Controller, IPlayerSOConsumer, IVendingConsumer
    {
        [SerializeField] private PlayerSO playerSo;
        [SerializeField] private TMP_Text coin_display;
        [SerializeField] private TMP_Text success_text;
        
        private float timer;
        protected override void Start()
        {
            base.Start();
            gameService.Subscribe(playerSo);
            success_text.gameObject.SetActive(false);
        }

        public override void Consume(GameServiceStart gameServiceStart)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            coin_display.text = playerSo.GetCoinAmount().ToString();
        }

        public void Consume(PlayerCoinUpdateEvent playerCoinUpdateEvent)
        {
            UpdateUI();
        }

        public void Consume(PlayerCoinSuccessEvent playerCoinSuccessEvent)
        {
            if (!playerCoinSuccessEvent.isSuccess)
            {
                success_text.text = "Failed";
            }
            else
            {
                success_text.text = "Success";
            }
            ActionRunner.ActionRunner.Instance.AddAction(timer, () =>
            {
                success_text.gameObject.SetActive(true);
            });
            timer = Time.time;
            ActionRunner.ActionRunner.Instance.AddAction(timer+5, () =>
            {
                success_text.gameObject.SetActive(false);
            });
        }

        public void Consume(VendingMachineItemPurchaseEvent vendingMachineItemPurchaseEvent)
        {
            playerSo.Consume(vendingMachineItemPurchaseEvent);
        }
    }
}