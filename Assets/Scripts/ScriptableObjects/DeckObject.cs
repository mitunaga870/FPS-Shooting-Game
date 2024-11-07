using System.Collections.Generic;
using AClass;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class DeckObject : ScriptableObject
    {
        [SerializeField]
        public List<ATrap> DefaultTraps;

        [SerializeField]
        public List<ASkill> DefaultSkills;

        [SerializeField]
        public List<ATurret> DefaultTurrets;
    }
}