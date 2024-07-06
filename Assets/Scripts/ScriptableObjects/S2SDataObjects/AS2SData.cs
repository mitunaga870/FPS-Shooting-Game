using System;
using UnityEngine;
using TrapData = DataClass.TrapData;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * フェーズ間でデータを受け渡すためのクラスの基底クラス
     * このクラスを継承してデータを受け渡すクラスを作成する
     *
     * 基本的にはScriptableObjectを継承し、標準のToStringメソッドをオーバーライドする
     */
    // ReSharper disable once InconsistentNaming
    public abstract class AS2SData : ScriptableObject
    {
        public abstract override string ToString();
    }
}