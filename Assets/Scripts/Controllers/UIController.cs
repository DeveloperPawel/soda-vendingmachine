using System;
using System.Collections.Generic;
using Panels;
using Service.Events;
using UnityEngine;

namespace Controllers
{
    public class UIController : Controller
    {
        public static UIController Instance { get; private set; }
        protected Dictionary<System.Type, List<GameObject>> typePanelDictionary;

        private void Awake()
        {
            Instance = this;
            typePanelDictionary = new Dictionary<Type, List<GameObject>>();
        }

        public void Register(Type type, Panel panel)
        {
            if (!typePanelDictionary.TryGetValue(type, out List<GameObject> panelList))
            {
                typePanelDictionary.Add(type, new List<GameObject>(){panel.gameObject});
                return;
            }
            panelList.Add(panel.gameObject);
        }

        protected void Respond(Type type)
        {
            foreach (var typeKey in typePanelDictionary.Keys)
            {
                if (type == typeKey)
                {
                    foreach (var go in typePanelDictionary[typeKey])
                    {
                        go.SetActive(true);
                    }
                    continue;
                }
                foreach (var go in typePanelDictionary[typeKey])
                {
                    go.SetActive(false);
                }
            }
        }
        
        public override void Consume(GameServiceStart gameServiceStart)
        {
            Respond(gameServiceStart.GetType());
        } 
        
        public override void Consume(GameServiceEnd gameServiceEnd)
        {
            Respond(gameServiceEnd.GetType());
        }
    }
}