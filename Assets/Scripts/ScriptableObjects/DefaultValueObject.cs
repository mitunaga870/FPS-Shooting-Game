using UnityEngine;

namespace ScriptableObjects
{
    public class DefaultValueObject : ScriptableObject
    {
        [SerializeField] public int defaultWallet { get; private set; }
    }
}