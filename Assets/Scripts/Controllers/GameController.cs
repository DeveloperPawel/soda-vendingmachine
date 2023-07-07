using System;
using System.Reflection;
using Interfaces;
using Service;
using Service.Events;
using UnityEngine;

namespace Controllers
{
    public class GameController : Controller
    {
        public override void Consume(GameServiceStart gameServiceStart)
        {
            
        }

        public override void Consume(GameServiceEnd gameServiceEnd)
        {
            
        }
    }
}