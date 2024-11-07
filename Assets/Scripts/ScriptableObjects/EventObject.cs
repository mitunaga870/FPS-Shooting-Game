using System.Collections.Generic;
using DataClass;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class EventObject : ScriptableObject
    {
        [SerializeField]
        public List<EventData> EventData = new();
        
        public EventData GetRandomEventData()
        {
            return EventData[Random.Range(0, EventData.Count)];
        }
    }
}