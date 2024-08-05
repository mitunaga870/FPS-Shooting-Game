using UnityEngine;

namespace ScriptableObjects
{
    // ReSharper disable InconsistentNaming
    [CreateAssetMenu]
    public class TrapObject : ScriptableObject
    {
        [Tooltip("テストトラップの攻撃力")] public float TestTrapAtk;

        [Tooltip("テストトラップのクールダウン")] public float TestTrapCoolDown;

        [Tooltip("テストトラップのコスト")] public int TestTrapCost;

        [Tooltip("テストトラップの高さ")] public float TestTrapHeight;
    }
}