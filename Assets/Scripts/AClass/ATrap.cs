using DataClass;
using InvasionPhase;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace AClass
{
    public abstract class ATrap : MonoBehaviour
    {
        /** トラップ用のデータ用スクリプタブルオブジェクト */
        [FormerlySerializedAs("trapData")]
        [SerializeField]
        protected TrapObject trapObject;

        /**
         * 敵をコントロールしてるクラス
         * これにアクセスして敵に影響を与える
         */
        [CanBeNull]
        protected InvasionEnemyController enemyController = null;

        /**
         * 侵攻準備ができているか
         */
        protected bool IsInvasionReady = false;

        /**
         * トラップの発火
         */
        public abstract void AwakeTrap(TilePosition position);

        /**
         * トラップの高さ取得
         */
        public abstract float GetHeight();

        /**
         * トラップごとの禁止エリア処理
         * - 禁止エリアのときはtrueを返す
         */
        public static bool IsProhibitedArea(int row, int col)
        {
            return false;
        }

        /**
         * トラップデータ用文字列の取得
         */
        public abstract string GetTrapName();

        public void InvasionInitialize(InvasionEnemyController enemyController)
        {
            IsInvasionReady = true;
            this.enemyController = enemyController;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}