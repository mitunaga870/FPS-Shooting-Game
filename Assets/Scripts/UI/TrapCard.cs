using AClass;
using UnityEngine;

namespace Shop.UI
{
    public class TrapCard : MonoBehaviour
    {
        [SerializeField]
        private ATrap trapPrefab;
        
        public string GetTrapName()
        {
            return trapPrefab.GetTrapName();
        }
    }
}