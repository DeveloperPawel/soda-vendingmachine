using System;
using Service.Events;

namespace Panels
{
    public class StartMenu : Panel
    {
        protected override void Awake()
        {
            base.Awake();
            eventArg = new GameServiceStart();
        }
    }
}