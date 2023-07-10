using System.Collections;
using NUnit.Framework;
using Player;
using Service;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using VendingMachine;
using Object = UnityEngine.Object;

namespace Tests
{
public class VendingMachineTest
{
    private GameObject vendingGO;
    private VendingMachineMock _vendingMachine;
    private VendingItemSO _vendingMachineItemSO;
    private GameObject buttonGO;
    private Button button;

    public class VendingItemSOMock : VendingItemSO
    {
        public void SetName(string name)
        {
            this._name = name;
        }

        public void SetPrice(int num)
        {
            this._price = num;
        }
    }
    
    public class VendingMachineMock : VendingMachine.VendingMachine
    {
        public int playerCoinIncrementUpdate;
        public int playerCoinSuccessUpdate;

        protected override void Start()
        {
            // do nothing
        }

        public void AddGameService(GameService gameService)
        {
            this.gameService = gameService;
        }

        public void AddVendingItem(VendingItemSO vendingItemSo)
        {
            this.vendingItems.Add(vendingItemSo);
        }

        public void AddButton(GameObject go)
        {
            this.buttonPrefab = go;
        }

        public void SetUp()
        {
            CreateButtons();
        }
        
        public override void Consume(PlayerCoinUpdateEvent playerCoinUpdateEvent)
        {
            playerCoinIncrementUpdate += 1;
        }
        
        public override void Consume(PlayerCoinSuccessEvent playerCoinSuccessEvent)
        {
            playerCoinSuccessUpdate += 1;
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
        buttonGO = GameObject.Instantiate(new GameObject("button"));
        button = buttonGO.AddComponent<Button>();
        _vendingMachineItemSO = ScriptableObject.CreateInstance<VendingItemSOMock>();
        (_vendingMachineItemSO as VendingItemSOMock).SetName("item 1");
        (_vendingMachineItemSO as VendingItemSOMock).SetPrice(10);
        vendingGO = GameObject.Instantiate(new GameObject("vending"));
        _vendingMachine = vendingGO.AddComponent<VendingMachineMock>();
        _vendingMachine.AddButton(buttonGO);
    }
    
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(buttonGO);
        Object.Destroy(_vendingMachineItemSO);
        Object.Destroy(vendingGO);
    }

    [UnityTest]
    public IEnumerator VendingMachine_RecievesPlayerUpdateEvent()
    {
        _vendingMachine.EventUpdate(new PlayerCoinUpdateEvent
        {
            addamount = 5
        });
        yield return null;
        Assert.AreEqual(1, _vendingMachine.playerCoinIncrementUpdate);
    }
    
    [UnityTest]
    public IEnumerator VendingMachine_RecievesSuccessEvent()
    {
        _vendingMachine.EventUpdate(new PlayerCoinSuccessEvent
        {
            isSuccess = true
        });
        yield return null;
        Assert.AreEqual(1, _vendingMachine.playerCoinSuccessUpdate);
    }
}
}