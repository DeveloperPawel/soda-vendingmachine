using System;
using Interfaces;
using UnityEngine;

namespace Service
{
    public abstract class ServiceFinder : MonoBehaviour, Initializable
    {
        public event Action Init;
        public bool Initialized { get; }
        public abstract void Initialize();
    }
}