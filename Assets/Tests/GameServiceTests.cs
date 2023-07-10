using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using System;
using System.Collections.Generic;
using Interfaces;
using NUnit.Framework;
using Service;
using Service.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests
{ 
    public class GameServiceTests
    {
        private GameServiceMock gameService;
        private GameObject producer_GO;
        private IProducer _producer;
        
        public class TestEvent<T> : UnityEvent<EventArgs>
        {
            public int listenerCount = 0;
            
            public void AddListener(UnityAction<EventArgs> action)
            {
                base.AddListener(action);
                listenerCount++;
            }
            
            public void RemoveListener(UnityAction<EventArgs> action)
            {
                base.RemoveListener(action);
                listenerCount--;
            }
        }
        
        public class GameServiceMock : GameService
        {
            public int numTimesCalled = 0;
            protected override void OnEnable()
            {
                _event = new TestEvent<EventArgs>();
                isInitialized = true;
            }
        
            public override void Subscribe(IProducer producer)
            {
                (producer.Event as TestEvent<EventArgs>).AddListener(EventUpdate);
            }
        
            public void CallGameStart()
            {
                numTimesCalled++;
                _event?.Invoke(new GameServiceStart());
            }
        }
        
        public class TestProducer : MonoBehaviour, IProducer
        {
            private UnityEvent<EventArgs> _event;
            public UnityEvent<EventArgs> Event => _event;
        
            private void Awake()
            {
                _event = new TestEvent<EventArgs>();
            }
        }
        
        [OneTimeSetUp]
        public void OnTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/Tests/Scenes/GameServiceScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }
        
        [SetUp]
        public void Setup()
        {
            gameService = ScriptableObject.CreateInstance<GameServiceMock>();
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(gameService);
        }
        
        [UnityTest]
        public IEnumerator GameService_Playmode_OnCreateGameService()
        {
            yield return null;
            Assert.IsNotNull(Object.FindObjectOfType<GameService>());
        }
        
        [UnityTest]
        public IEnumerator GameService_Playmode_IsInitialized()
        {
            yield return null;
            Assert.AreEqual(true, gameService.Initialized);
        }
        
        [UnityTest]
        public IEnumerator GameService_Playmode_OnFireEvent()
        {
            bool flag = false;
            
            yield return null;
            gameService.Event.AddListener((eventArgs) =>
            {
                flag = true;
            });
            
            gameService.CallGameStart();
        
            yield return null;
            Assert.AreEqual(true, flag);
        }
        
        [UnityTest]
        public IEnumerator GameService_Playmode_OnSubscribeToProducer()
        {
            producer_GO = GameObject.Instantiate(new GameObject("provider"));
            _producer = producer_GO.AddComponent<TestProducer>();
            
            yield return null;
            gameService.Subscribe(_producer);
            
            yield return null;
            Assert.AreEqual(1, (_producer.Event as TestEvent<EventArgs>).listenerCount);
            Object.Destroy(producer_GO);
        }
    }
}