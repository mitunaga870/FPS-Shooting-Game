using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class DefaultValueObject : ScriptableObject
    {
        [SerializeField] public int defaultWallet;
    }
}