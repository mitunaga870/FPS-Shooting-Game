using System;
using AClass;
using UnityEngine;
using Object = UnityEngine.Object;

namespace lib
{
    public static class InstanceGenerator
    {
        /**
         * トラップ名からトラップを生成する
         */
        public static ATrap GenerateTrap(string trapName)
        {
            var traps = Resources.LoadAll<ATrap>("Prefabs/Traps");

            // Linqを使うと見づらいのでforeachで書いている
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var trap in traps)
                if (trap.GetTrapName() == trapName)
                    return trap;

            throw new Exception("トラップデータが見つかりません、セーブデータが壊れている可能性があります。");
        }

        /**
         * 砲台名から砲台を生成する
         */
        public static ATurret GenerateTurret(string turretName)
        {
            var turrets = Resources.LoadAll<ATurret>("Prefabs/Turrets");

            // Linqを使うと見づらいのでforeachで書いている
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var turret in turrets)
                if (turret.GetTurretName() == turretName)
                    return turret;

            throw new Exception("砲台データが見つかりません、セーブデータが壊れている可能性があります。");
        }
    }
}