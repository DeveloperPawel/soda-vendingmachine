using System;
using UnityEngine.Events;

namespace Interfaces
{
    public interface IProducer
    {
        UnityEvent<EventArgs> Event { get; }
    }
}