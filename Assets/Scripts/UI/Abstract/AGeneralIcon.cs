using System;
using UnityEngine;

namespace UI.Abstract
{
    public abstract class AGeneralIcon : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI amountText;
        
        private int _amount = 1;

        private void Update()
        {
            amountText.text = _amount.ToString();
        }
        
        public void SetAmount(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }
            
            _amount = amount;
        }
        
        public void IncrementAmount()
        {
            _amount++;
        }

        public void SetClickAction(Action clickAction)
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => clickAction());
        }
    }
}