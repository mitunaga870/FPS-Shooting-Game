using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class TurretObject : ScriptableObject
    {
        // ============= 豆ガトリング =============
        [Header("豆ガトリング")]
        public int BeansGatlingDamage;
        public float BeansGatlingHeight = 1;
        public int BeansGatlingInterval = 1;
        
        // ============= ファン =============
        [Header("ファン")]
        public float FanHeight = 0.155f;
        public float FanSlowPercentage = 0.8f;
        public int FanEffectDuration = 100;
        public int FanSlowDuration = 5;
        public int FanInterval = 400;
        
        // ============= ミサイル =============
        [Header("ミサイル")]
        public float MissileHeight = 0.13f;
        public int MissileDamage = 1;
        public int MissileObjectDuration = 10;
        public int MissileIgniteDamage;
        public int MissileIgniteDuration = 100;
        public int MissileInterval = 500;
        
        // ============= サメ =============
        [Header("サメ")]
        public int SharkDamage = 9999;
        public int SharkHeight = 3;
        public int SharkInterval = 1;
        public int SharkInvasionDataAnalysisTimeSpan = 10;
        public int SpawnPossibilityDistribution = 10;
        
        // ============= 屋台 =============
        [Header("屋台")]
        public float StallHeight = 1;
        public int StallInterval = 500;
        public int StallDuration = 200;
        
        // ============= マグロ剣 =============
        [Header("マグロ剣")]
        public int TunaSwordDamage = 1;
        public float TunaSwordHeight = -0.5f;
        public int TunaSwordInterval = 1;
    }
}