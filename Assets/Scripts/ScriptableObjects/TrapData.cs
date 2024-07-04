using UnityEngine;

namespace ScriptableObjects
{
    // ReSharper disable InconsistentNaming
    [CreateAssetMenu]
    public class TrapData : ScriptableObject
    {
        [Tooltip("テストトラップの攻撃力")] public float TestTrapAtk;

        [Tooltip("テストトラップのクールダウン")] public float TestTrapCoolDown;

        [Tooltip("テストトラップのコスト")] public int TestTrapCost;
    }
}