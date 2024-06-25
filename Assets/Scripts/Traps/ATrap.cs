using UnityEngine;

namespace Traps
{
    public abstract class ATrap : MonoBehaviour
    {
        /**
         * トラップの発火
         */
        public abstract void Awake();
    }
}