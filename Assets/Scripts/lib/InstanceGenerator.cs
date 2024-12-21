using System;
using AClass;
using UnityEngine;

namespace lib
{
    public static class InstanceGenerator
    {
        /**
         * トラップ名からトラップを生成する
         */
        public static ATrap GenerateTrap(string trapName)
        {
            if (trapName == "") return null;
            
            var traps = Resources.LoadAll<ATrap>("Prefabs/Traps");

            // Linqを使うと見づらいのでforeachで書いている
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var trap in traps)
                if (trap.GetTrapName() == trapName)
                    return trap;
            
            throw new Exception("トラップデータが見つかりません、セーブデータが壊れている可能性があります。 " + trapName);
        }

        /**
         * 砲台名から砲台を生成する
         */
        public static ATurret GenerateTurret(string turretName)
        {
            if (turretName == "") return null;
            
            var turrets = Resources.LoadAll<ATurret>("Prefabs/Turrets");

            // Linqを使うと見づらいのでforeachで書いている
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var turret in turrets)
                if (turret.GetTurretName() == turretName)
                    return turret;

            throw new Exception("砲台データが見つかりません、セーブデータが壊れている可能性があります。");
        }

        public static ASkill GenerateSkill(string skill)
        {
            if (skill == "") return null;
            
            var skills = Resources.LoadAll<ASkill>("Prefabs/Skill");

            // Linqを使うと見づらいのでforeachで書いている
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var s in skills)
                if (s.GetSkillName() == skill)
                    return s;

            throw new Exception("スキルデータが見つかりません、セーブデータが壊れている可能性があります。");
        }
    }
}