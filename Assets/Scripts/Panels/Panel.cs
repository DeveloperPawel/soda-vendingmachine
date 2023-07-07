using System;
using Controllers;
using Interfaces;
using Service;
using UnityEngine;
using UnityEngine.Events;

namespace Panels
{
    public abstract class Panel : MonoBehaviour, IProducer
    {
        private UnityEvent<EventArgs> _event;
        protected EventArgs eventArg;
        protected GameServiceFinder _gameServiceFinder;

        protected virtual void Awake()
        {
            _event = new UnityEvent<EventArgs>();
        }

        protected virtual void Start()
        {
            if (eventArg == null) Debug.LogWarning($"no event args applied to {this.GetType()} - WILL NOT SHOW, ADD EVENT TO eventArg");
            
            UIController.Instance.Register(eventArg.GetType(), this);
            SubscribeGameServiceFinder();
        }

        public UnityEvent<EventArgs> Event => _event;

        protected virtual void RegisterProducer()
        {
            _gameServiceFinder.RegisterProducer(this);
        }

        protected void SubscribeGameServiceFinder()
        {
            _gameServiceFinder = FindObjectOfType<GameServiceFinder>();
            if (_gameServiceFinder.Initialized)
            {
                RegisterProducer();
            }
            _gameServiceFinder.Init += RegisterProducer;
        }

        public void Remove_Disable()
        {
            _event.RemoveAllListeners();
        }

        private void OnDisable()
        {
            Remove_Disable();
        }
    }
}