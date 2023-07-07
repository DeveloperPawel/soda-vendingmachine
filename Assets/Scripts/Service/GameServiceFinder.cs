using System;
using System.Runtime.CompilerServices;
using Interfaces;
using UnityEngine;

namespace Service
{
    public class GameServiceFinder : ServiceFinder
    {
        public static GameServiceFinder Instance { get; private set; }

        [SerializeField] protected GameService gameService;
        private bool isInitialized;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Initialize();
            Init?.Invoke();
        }

        public void RegisterProducer(IProducer producer)
        {
            gameService.Subscribe(producer);
        }

        public event Action Init;
        public bool Initialized => isInitialized;
        public override void Initialize()
        {
            isInitialized = true;
        }
    }
}