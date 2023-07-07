using System;
using System.Reflection;
using Interfaces;
using Service;
using Service.Events;
using UnityEngine;

namespace Controllers
{
    public abstract class Controller : MonoBehaviour, IConsumer
    {
        [SerializeField] protected GameService gameService;

        protected virtual void Start()
        {
            Subscribe(gameService);
            if (gameService.Initialized)
            {
                EventUpdate(gameService.GetEvent());
            }

            gameService.Init += () =>
            {
                EventUpdate(gameService.GetEvent());
            };
        }

        public virtual void EventUpdate(EventArgs args)
        {
            Type type = args.GetType();
            var signature = new[] {type};
            MethodInfo methodInfo = GetType().GetMethod("Consume", signature);
            if (methodInfo == null) return;
            methodInfo.Invoke(this, new[] {args});
        }

        public virtual void Subscribe(IProducer producer)
        {
            producer.Event.AddListener(EventUpdate);
        }

        public virtual void Consume(GameServiceStart gameServiceStart)
        {
            
        }

        public virtual void Consume(GameServiceEnd gameServiceEnd)
        {
            
        }
    }
}