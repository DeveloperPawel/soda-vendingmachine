using System.Collections;
using NUnit.Framework;
using Player;
using Service;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VendingMachine;
using Object = UnityEngine.Object;

namespace Tests
{
    public class PlayerPanelTest
    {
        private PlayerSOTest.PlayerSOMock _player;
        private PlayerPanelMock _playerPanelMock;
        private GameObject playerPanelGO;
        
        public class PlayerPanelMock : PlayerPanel
        {
            public int playerCoinUpdate;
            public int playerCoinSuccess;
            public int vendingMachinePurchase;

            protected override void Start()
            {
                
            }

            public void AddPlayerSO(PlayerSO player)
            {
                this.playerSo = player;
            }

            public void AddGameService(GameService gameService)
            {
                this.gameService = gameService;
            }
            
            public override void Consume(PlayerCoinUpdateEvent playerCoinUpdateEvent)
            {
                playerCoinUpdate++;
            }
            
            public override void Consume(PlayerCoinSuccessEvent playerCoinSuccessEvent)
            {
                playerCoinSuccess++;
            }
            
            public override void Consume(VendingMachineItemPurchaseEvent vendingMachineItemPurchaseEvent)
            {
                vendingMachinePurchase++;
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
            _player = ScriptableObject.CreateInstance<PlayerSOTest.PlayerSOMock>();
            playerPanelGO = GameObject.Instantiate(new GameObject("panel"));
            _playerPanelMock = playerPanelGO.AddComponent<PlayerPanelMock>();
            _playerPanelMock.AddPlayerSO(_player);
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_player);
            Object.Destroy(playerPanelGO);
        }
        
        [UnityTest]
        public IEnumerator PlayerPanel_RecievesSuccessEvent()
        {
            yield return null;
            _playerPanelMock.EventUpdate(new PlayerCoinSuccessEvent
            {
                isSuccess = true
            });
            Assert.AreEqual(1, _playerPanelMock.playerCoinSuccess);
        }
        
        [UnityTest]
        public IEnumerator PlayerPanel_RecievesVendingMachineEvent()
        {
            yield return null;
            _playerPanelMock.EventUpdate(new VendingMachineItemPurchaseEvent
            {
                amount = 5
            });
            Assert.AreEqual(1, _playerPanelMock.vendingMachinePurchase);
        }
        
        [UnityTest]
        public IEnumerator PlayerPanel_RecievesCoinUpdate()
        {
            yield return null;
            _playerPanelMock.EventUpdate(new PlayerCoinUpdateEvent
            {
                addamount = 5
            });
            Assert.AreEqual(1, _playerPanelMock.playerCoinUpdate);
        }
    }
}