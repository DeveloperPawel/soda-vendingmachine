using System;

namespace Interfaces
{
    public interface Initializable
    {
        event Action Init;
        bool Initialized { get; }
        void Initialize();
    }
}