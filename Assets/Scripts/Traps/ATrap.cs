using ScriptableObjects;
using UnityEngine;

namespace Traps
{
    public abstract class ATrap : MonoBehaviour
    {
        /** トラップ用のデータ用スクリプタブルオブジェクト */
        [SerializeField] protected TrapData trapData;

        /**
         * トラップの発火
         */
        public abstract void Awake();

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

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}