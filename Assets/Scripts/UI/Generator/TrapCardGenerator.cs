using System.Collections.Generic;
using Shop.UI;
using UnityEngine;

namespace UI.Generator
{
    public class TrapCardGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<TrapCard> trapIcons = new ();
        
        public TrapCard GetTrapIcon(string trapName)
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