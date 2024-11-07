using System;
// ReSharper disable InconsistentNaming

namespace DataClass
{
    [Serializable]
    public class StageCustomData
    {
        /** 移動速度のスケール */
        public float MoveSpeedScale;
        /** 敵の攻撃力のスケール */
        public float EnemyAttackScale;
        /** 敵のHPのスケール */
        public float EnemyHpScale;
        /** プレイヤーの攻撃力のスケール */
        public float PlayerAttackScale;
        /** スキルを使えるかどうか */
        public bool IsAllowedToUseSkill;
        /** タレットを設置できるかどうか */
        public bool IsAllowedToSetTurret;
        /** 敵の残機数のスケール, これがある限り生き返る  */
        public int EnemyRemainingLivesScale;
        /** 道の容量 -1は無限 */
        public int CapacityRoad;
        /** スキップフラグ */
        public bool IsSkip;
        
        public StageCustomData()
        {
            MoveSpeedScale = 1.0f;
            EnemyAttackScale = 1.0f;
            EnemyHpScale = 1.0f;
            IsAllowedToUseSkill = true;
            IsAllowedToSetTurret = true;
            PlayerAttackScale = 1.0f;
            IsAllowedToUseSkill = true;
            IsAllowedToSetTurret = true;
            EnemyRemainingLivesScale = 0;
            CapacityRoad = -1;
            IsSkip = false;
        }

        public StageCustomData(string data)
        {
            var split = data.Split(',');
            MoveSpeedScale = float.Parse(split[0]);
            EnemyAttackScale = float.Parse(split[1]);
            EnemyHpScale = float.Parse(split[2]);
            IsAllowedToUseSkill = true;
            IsAllowedToSetTurret = true;
            PlayerAttackScale = float.Parse(split[3]);
            IsAllowedToUseSkill = bool.Parse(split[4]);
            IsAllowedToSetTurret = bool.Parse(split[5]);
            EnemyRemainingLivesScale = int.Parse(split[6]);
            CapacityRoad = int.Parse(split[7]);
            IsSkip = bool.Parse(split[8]);
        }

        // 保存用 CSV
        public override string ToString()
        {
            return $"{MoveSpeedScale},{EnemyAttackScale},{EnemyHpScale},{PlayerAttackScale},{IsAllowedToUseSkill},{IsAllowedToSetTurret},{EnemyRemainingLivesScale},{CapacityRoad},{IsSkip}";
        }
    }
}