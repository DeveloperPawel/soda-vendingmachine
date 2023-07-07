using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace ActionRunner
{
    public class ActionRunner : MonoBehaviour
    {
        public static ActionRunner Instance { get; private set; }
        private Dictionary<float, Action> _timerActions;
        private List<float> removeList;
        private float timer;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _timerActions = new Dictionary<float, Action>();
            removeList = new List<float>();
        }

        private void LateUpdate()
        {
            if (_timerActions.Count == 0) return;
            timer += Time.deltaTime;
            foreach (var keyVal in _timerActions)
            {
                if (timer > keyVal.Key)
                {
                    keyVal.Value();
                }
            }
            RemoveTimerAction(timer);
        }
        
        private void RemoveTimerAction(float time)
        {
            foreach (var num in removeList)
            {
                if (time > num)
                {
                    _timerActions.Remove(num);
                }
            }
        }

        public void AddAction(float time, Action action)
        {
            timer = Time.time;
            // _timerActions.Add(time, action);
            if (!_timerActions.TryAdd(time, action))
            {
                return;
            }
            removeList.Add(time);
        }
    }
}