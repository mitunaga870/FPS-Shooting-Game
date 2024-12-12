using AClass;
using UI.Abstract;
using UnityEngine;

namespace UI.Card
{
    public class TrapCard : AGeneralCard
    {
        [SerializeField]
        private ATrap trapPrefab;
        
        public string GetTrapName()
        {
            return trapPrefab.GetTrapName();
        }
    }
}