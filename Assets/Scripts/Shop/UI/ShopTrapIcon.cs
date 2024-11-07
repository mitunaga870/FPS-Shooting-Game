using AClass;
using UnityEngine;

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
    }
}