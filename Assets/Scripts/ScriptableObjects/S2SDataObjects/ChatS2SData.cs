using System;
using UnityEngine;

namespace ScriptableObjects.S2SDataObjects
{
    [CreateAssetMenu(fileName = "ChatS2SData", menuName = "S2SData/ChatS2SData")]
    public class ChatS2SData : AS2SData
    {
        [NonSerialized]
        public bool ShowedOP;
        
        [NonSerialized]
        public bool ShowedFirstBattle;
        
        [NonSerialized]
        public bool ShowedFirstReroll;
        
        [NonSerialized]
        public bool ShowedFirstTurret;
        
        [NonSerialized]
        public bool ShowedFirstShop;
        
        public override string ToString()
        {
            var message = $"ShowedOP: {ShowedOP}\n";
            message += $"ShowedFirstBattle: {ShowedFirstBattle}\n";
            message += $"ShowedFirstReroll: {ShowedFirstReroll}\n";
            message += $"ShowedFirstTurret: {ShowedFirstTurret}\n";
            message += $"ShowedFirstShop: {ShowedFirstShop}\n";

            return message;
        }

        public override void OnAfterDeserialize()
        {
            ShowedOP = false;
            ShowedFirstBattle = false;
            ShowedFirstReroll = false;
            ShowedFirstTurret = false;
            ShowedFirstShop = false;
        }
    }
}