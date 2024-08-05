using System;
using AClass;
using UnityEngine;
using Object = UnityEngine.Object;

namespace lib
{
    public static class TrapGenerator
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
            {
                if (trap.GetTrapName() == trapName)
                {
                    return Object.Instantiate(trap);
                }
            }

            throw new Exception("トラップデータが見つかりません、セーブデータが壊れている可能性があります。");
        }
    }
}