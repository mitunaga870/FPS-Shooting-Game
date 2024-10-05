using UnityEngine;

namespace AClass
{
    public abstract class AReward
    {
        [SerializeField] protected int Value;

        public abstract void GetReward();
    }
}