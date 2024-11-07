using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    // ReSharper disable InconsistentNaming
    [CreateAssetMenu]
    public class TrapObject : ScriptableObject
    {
        // ================== ワニ ============================
        [Header("ワニ")]
        public int AlligatorDamage = 1;
        public float AlligatorHeight = 0.04f;
        public int AlligatorCoolDown = 100;
        public int AlligatorKnockBack = 2;
        public int AlligatorSetRange = 1;
        
        // ================== 車 ============================
        [Header("車")]
        public int CarDamage = 1;
        public float CarHeight = 0.18f;
        public int CarCoolDown = 5000;
        public int CarAttackRange = 4;
        public int CarSetRange = 1;
        
        // ================== 地雷 ============================
        [Header("地雷")]
        public int LandMineDamage = 10;
        public float LandMineHeight = 0.05f;
        public int LandMineCoolDown = 100;
        public int LandMineSetRange = 1;
        
        // ================== 毒沼 ============================
        [Header("毒沼")]
        public int PoisonSwampDamage = 1;
        public float PoisonSwampHeight = 0.5f;
        public int PoisonSwampDuration = 100;
        public int PoisonSwampSetRange = 3;
        
        // ================== 東京タワー ============================
        [Header("東京タワー")]
        public int TokyoTowerDamage = 1;
        public float TokyoTowerCreateHeight = 0.15f;
        public float TokyoTowerInvadeHeight = -2.7f;
        public float TokyoTowerMovedHeight = 0.15f;
        public int TokyoTowerKnockBack = 5;
        public int TokyoTowerCoolDown = 500;
        public int TokyoTowerSetRange = 2;
        
        // ================== トランポリン ============================
        [Header("トランポリン")]
        public int TrampolineJumpHeight = 10;
        public int TrampolineDamage = 10;
        public int TrampolineCoolDown = 100;
        public int TrampolineSetRange = 1;
        public float TrampolineHeight = 0.04f;
    }
}