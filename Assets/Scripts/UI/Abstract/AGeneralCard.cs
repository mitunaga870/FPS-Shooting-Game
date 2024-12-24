using UnityEngine;

namespace UI.Abstract
{
    public abstract class AGeneralCard : MonoBehaviour
    {
        [SerializeField]
        private AGeneralIcon generalIcon;
        
        public void SetAmount(int amount)
        {
            generalIcon.SetAmount(amount);
        }
        
        public void IncrementAmount()
        {
            generalIcon.IncrementAmount();
        }
    }
}