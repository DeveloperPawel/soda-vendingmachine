using System;
using Service.Events;

namespace Panels
{
    public class EndMenu : Panel
    {
        protected override void Awake()
        {
            base.Awake();
            eventArg = new GameServiceEnd();
        }
    }
}