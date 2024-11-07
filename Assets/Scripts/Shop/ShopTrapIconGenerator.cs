using System.Collections.Generic;
using Shop.UI;
using UnityEngine;

namespace Shop
{
    public class ShopTrapIconGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<ShopTrapIcon> trapIcons = new ();
        
        public ShopTrapIcon GetTrapIcon(string trapName)
        {
            foreach (var trapIcon in trapIcons)
            {
                if (trapIcon.GetTrapName() == trapName)
                {
                    return trapIcon;
                }
            }

            return null;
        }
    }
}