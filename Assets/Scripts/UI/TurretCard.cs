using AClass;
using UnityEngine;

namespace UI
{
    public class TurretCard : MonoBehaviour
    {
        [SerializeField]
        private ATurret turretPrefab;
        
        public string GetTurretName()
        {
            return turretPrefab.GetTurretName();
        }
    }
}