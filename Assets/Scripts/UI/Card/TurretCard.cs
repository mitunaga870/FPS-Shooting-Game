using AClass;
using UI.Abstract;
using UnityEngine;

namespace UI.Card
{
    public class TurretCard : AGeneralCard
    {
        [SerializeField]
        private ATurret turretPrefab;
        
        public string GetTurretName()
        {
            return turretPrefab.GetTurretName();
        }
    }
}