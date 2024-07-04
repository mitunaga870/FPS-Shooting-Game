using System;
using UnityEngine;

namespace S2SData
{
    public class CreateToInvasionData : ScriptableObject, ISerializationCallbackReceiver
    {
        [NonSerialized] public Tile[][] Tiles;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}