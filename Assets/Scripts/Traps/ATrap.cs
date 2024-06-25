using UnityEngine;

namespace Traps
{
    public abstract class ATrap : MonoBehaviour
    {
        /**
         * トラップの発火
         */
        public abstract void Awake();

        /**
         * トラップごとの禁止エリア処理
         * - 禁止エリアのときはtrueを返す
         */
        public bool IsProhibitedArea(int row, int col)
        {
            return false;
        }
    }
}