using System;

namespace Interfaces
{
    public interface IConsumer
    {
        void EventUpdate(EventArgs args);
        void Subscribe(IProducer producer);
    }
}