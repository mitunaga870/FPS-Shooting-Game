using DataClass;
using InvasionPhase;
using ScriptableObjects;
using UnityEngine;

namespace AClass
{
    public abstract class ASkill : MonoBehaviour
    {
        [SerializeField] protected InvasionEnemyController _enemyController;
        [SerializeField] protected SkillObject _skillObject;
        
        /**
         * 対象タイルを指定してスキルを利用、クールダウンはアイコン側で処理予定
         */
        public abstract void AwakeSkill(TilePosition tilePosition);

        /**
         *  クールダウンタイムを取得
         */
        public abstract int GetCd();
    }
}