using AClass;
using UI.Abstract;
using UnityEngine;

namespace UI.Icon
{
    public class GeneralTrapIcon : AGeneralIcon
    {
        [SerializeField]
        private ATrap trap;
        
        public string GetTrapName()
        {
            return trap.GetTrapName();
        }
   }
}