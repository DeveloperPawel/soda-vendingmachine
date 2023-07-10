using System;
using System.Collections;
using Controllers;
using Interfaces;
using NUnit.Framework;
using Player;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VendingMachine;
using Object = UnityEngine.Object;

namespace Tests
{
    public class PlayerSOTest
    {
        private PlayerSOMock _player;

        public class PlayerSOMock : PlayerSO
        {
            public int vendingMachineEvent;
            protected override void OnEnable()
            {
                this._event = new GameServiceTests.TestEvent<EventArgs>();
            }

            public void AddCoin(int amount)
            {
                this.coin = amount;
            }

            public void Remove_Coin(int amount)
            {
                this.RemoveCoin(amount);
            }

            public bool CouldSpendCoin(int amount)
            {
                return this.CanSpendCoin(amount);
            }

            public override void Consume(VendingMachineItemPurchaseEvent vendingMachineItemPurchaseEvent)
            {
                vendingMachineEvent++;
            }

            public void FireCoinUpdateEvent()
            {
                _event.Invoke(new PlayerCoinUpdateEvent
                {
                    addamount = 0
                });
            }

            public void FireSuccessUpdateEvent()
            {
                _event.Invoke(new PlayerCoinSuccessEvent
                {
                    isSuccess = true
                });
            }
        }
        
        [OneTimeSetUp]
        public void OnTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/Tests/Scenes/PlayerSOTests.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }
        
        [SetUp]
        public void Setup()
        {
            _player = ScriptableObject.CreateInstance<PlayerSOMock>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_player);
        }
        
        [UnityTest]
        public IEnumerator PlayerSO_Playmode_OnCreatePlayerSO()
        {
            yield return null;
            Assert.IsNotNull(Object.FindObjectOfType<PlayerSO>());
        }
        
        [UnityTest]
        public IEnumerator PlayerSO_Playmode_ReceivesVendingMachineEvent()
        {
            yield return null;
            _player.Consume(new VendingMachineItemPurchaseEvent()
            {
                amount = 10
            });
            Assert.AreEqual(1, _player.vendingMachineEvent);
        }
        
        [UnityTest]
        public IEnumerator PlayerSO_Playmode_CanSpendCoin()
        {
            yield return null;
            Assert.AreEqual(false, _player.CouldSpendCoin(34));
        }
        
        [UnityTest]
        public IEnumerator PlayerSO_Playmode_RemoveCoin()
        {
            yield return null;
            _player.AddCoin(100);
            _player.Remove_Coin(50);
            Assert.AreEqual(50, _player.GetCoinAmount());
        }
        
        [UnityTest]
        public IEnumerator PlayerSO_ConsumerRecievesSuccessEvent()
        {
            bool _bool = false;
            yield return null;
            _player.Event.AddListener((eventargs) =>
            {
                _bool = true;
            });
            yield return null;
            _player.FireSuccessUpdateEvent();
            Assert.AreEqual(true, _bool);
        }
        
        [UnityTest]
        public IEnumerator PlayerSO_ConsumerRecievesUpdateEvent()
        {
            bool _bool = false;
            yield return null;
            _player.Event.AddListener(arg0 =>
            {
                _bool = true;
            });
            yield return null;
            _player.FireCoinUpdateEvent();
            Assert.AreEqual(true, _bool);
        }
    }
}