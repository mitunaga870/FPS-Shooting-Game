using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AClass;
using Map;
using UnityEngine;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * 基本的なフェーズ間でのデータ受け渡しを行うクラス
     */
    [CreateAssetMenu(fileName = "GeneralS2SData", menuName = "S2SData/GeneralS2SData")]
    public class GeneralS2SData : AS2SData
    {
        [SerializeField]
        private DefaultValueObject defaultValueObject;
        
        [NonSerialized]
        private int mapNumber = -1;

        [NonSerialized]
        private int currentMapRow = -1;

        [NonSerialized]
        private int currentMapColumn = -1;

        [NonSerialized]
        private int playerHp = -1;
        
        [NonSerialized]
        private int wallet = -1;
        
        [NonSerialized]
        private int score = -1;
        
        [NonSerialized]
        public int MapNumber;

        [NonSerialized]
        public int CurrentMapRow;

        [NonSerialized]
        public int CurrentMapColumn;

        [NonSerialized]
        public int PlayerHp;
        
        [NonSerialized]
        public int Wallet;
        
        [NonSerialized]
        public int Score;

        [NonSerialized]
        [AllowNull]
        public MapWrapper[] Maps;
        
        [NonSerialized]
        private List<ATrap> _deckTraps = new();
        
        [NonSerialized]
        private List<ATrap> _handTraps = new();
        
        [NonSerialized]
        private List<ATrap> _discardTraps = new();
        
        [NonSerialized]
        private List<ASkill> _deckSkills = new();
        
        [NonSerialized]
        private List<ATurret> _deckTurrets = new();

        public (
            ATrap[] deckTraps,
            ATrap[] handTraps,
            ATrap[] discardTraps,
            ASkill[] skills,
            ATurret[] turrets
            )? GetDeckData()
        {
            if (_deckTraps.Count + _handTraps.Count + _discardTraps.Count + _deckSkills.Count + _deckTurrets.Count == 0)
            {
                return null;
            }
            
            return (
                _deckTraps.ToArray(),
                _handTraps.ToArray(),
                _discardTraps.ToArray(),
                _deckSkills.ToArray(),
                _deckTurrets.ToArray()
            );
        }

        public void SetDeckData(
            ATrap[] deckTrap, ATrap[] handTrap, ATrap[] discardTraps,
            ASkill[] deckSkills,
            ATurret[] deckTurrets)
        {
            _deckTraps.Clear();
            _deckTraps.AddRange(deckTrap);
            
            _handTraps.Clear();
            _handTraps.AddRange(handTrap);
            
            _discardTraps.Clear();
            _discardTraps.AddRange(discardTraps);
            
            _deckSkills.Clear();
            _deckSkills.AddRange(deckSkills);
            
            _deckTurrets.Clear();
            _deckTurrets.AddRange(deckTurrets);
        }

        public override string ToString()
        {
            return
                $"MapNumber: {MapNumber}, CurrentMapRow: {CurrentMapRow}, CurrentMapColumn: {CurrentMapColumn}, PlayerHp: {PlayerHp}";
        }

        public override void OnAfterDeserialize()
        {
            PlayerHp = playerHp;
            MapNumber = mapNumber;
            CurrentMapRow = currentMapRow;
            CurrentMapColumn = currentMapColumn;
            Wallet = wallet;
            Maps = null;
            _deckTraps = new List<ATrap>();
            _deckSkills = new List<ASkill>();
            _deckTurrets = new List<ATurret>();
        }

        public override void OnBeforeSerialize()
        {
        }
    }
}