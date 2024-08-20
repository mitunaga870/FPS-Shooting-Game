using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace AClass
{
    public abstract class ATrap : MonoBehaviour
    {
        /** トラップ用のデータ用スクリプタブルオブジェクト */
        [FormerlySerializedAs("trapData")] [SerializeField]
        protected TrapObject trapObject;

        /**
         * トラップの発火
         */
        public abstract void AwakeTrap();

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

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}