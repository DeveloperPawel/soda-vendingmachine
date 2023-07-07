using System;
using System.Collections.Generic;
using System.Reflection;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using VendingMachine;

namespace Service
{
    public abstract class Service : ScriptableObject, IConsumer
    {
        protected UnityEvent<EventArgs> _event;
        
        protected virtual void OnEnable()
        {
            if (_event == null) _event = new UnityEvent<EventArgs>();
        }

        public void EventUpdate(EventArgs args)
        {
            Type type = args.GetType();
            var signature = new[] {type};
            MethodInfo methodInfo = GetType().GetMethod("Consume", signature);
            if (methodInfo == null)
            {
                Debug.Log($"method not found on {this.GetType()}: {args.GetType()}");
                return;
            }
            
            methodInfo.Invoke(this, new[] {args});
        }

        public virtual void Subscribe(IProducer producer)
        {
            producer.Event.AddListener(EventUpdate);
        }
    }
}