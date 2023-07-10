using System;
using UnityEngine;

namespace VendingMachine
{
    [CreateAssetMenu(fileName = "VendingItem", menuName = "VendItem", order = 0)]
    public class VendingItemSO : ScriptableObject
    {
        [SerializeField] protected string _name;
        [SerializeField] protected int _price;
        private Guid guid = Guid.NewGuid();
        public string GetName()
        {
            return _name;
        }

        public int GetPrice()
        {
            return _price;
        }

        public string GetID()
        {
            return guid.ToString();
        }
    }
}