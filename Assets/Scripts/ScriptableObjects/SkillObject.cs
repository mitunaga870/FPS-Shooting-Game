using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class SkillObject : ScriptableObject
    {
        // ============ バナナ ============
        [Header("バナナ")]
        public int BananaDistance = 2;
        public int BananaStunTime = 100;
        
        // ============ 旗 ============
        [Header("旗")]
        public int FlagDuration = 500;
        public int FlagAddDamage = 2;
        
        // ============ 鉄道踏切 ============
        [Header("鉄道踏切")]
        public int RailwayCrossingDuration = 500;
        
        // ============ 蜘蛛の巣 ============
        [Header("蜘蛛の巣")]
        public int SpiderWebRange = 5;
        public int SpiderWebDuration = 500;
        public float SpiderWebSlowPower = 0.6f;
        
        // ============ スタンプ ============
        [Header("スタンプ")]
        public int StampDamage = 5;
        public int StampDuration = 100;
        
        // ============ テレポート ============
        [Header("テレポート")]
        public int TeleportDuration = 500;
        
        // ============ 三角コーン ============
        [Header("三角コーン")]
        public int TriangularConeDuration = 500;
    }
}