using System;
using System.Collections.Generic;
using AClass;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace DataClass
{
    /**
     * 報酬データ
     * 報酬の種類とその値を保持する
     */
    [Serializable]
    public class RewardData
    {
        /** 報酬額 */
        public int money;

        /** ランダムトラップが何個か */
        public int randomTrap;

        /** 選択トラップ報酬 */
        public List<ATrap> selectedTrap;

        /** ランダムタレットが何個か */
        public int randomTurret;

        /** 選択タレット報酬 */
        public List<ATurret> selectedTurret;

        /** ランダムスキルが何個か */
        public int randomSkill;

        /** 選択スキル報酬 */
        public List<ASkill> selectedSkill;
    }
}