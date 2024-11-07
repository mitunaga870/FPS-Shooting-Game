using System.Collections.Generic;
using CreatePhase;
using UnityEngine;

namespace Map.UI.lib
{
    public class TurretIconGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<TurretIcon> _icons;
        
        public TurretIcon getIcon(string turretName)
        {
            foreach (var icon in _icons)
            {
                if (icon.GetTurretName() == turretName)
                {
                    return icon;
                }
            }

            return null;
        }
    }
}