using AClass;
using UI.Abstract;
using UnityEngine;

namespace UI.Icon
{
    public class GeneralTurretIcon : AGeneralIcon
    {
        [SerializeField]
        private ATurret turret;
        
        public string GetTurretName()
        {
            return turret.GetTurretName();
        }
    }
}