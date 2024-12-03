using System;
using AClass;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.UI
{
    public class ShopTrapIcon : MonoBehaviour
    {
        [SerializeField]
        private ATrap trapPrefab;
        
        public string GetTrapName()
        {
            return trapPrefab.GetTrapName();
        }
        
        public void SetButtonAction(Action action)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => action());
        }
    }
}