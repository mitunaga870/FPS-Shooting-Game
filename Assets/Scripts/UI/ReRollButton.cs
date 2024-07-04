using System;
using lib;
using UnityEngine;

namespace UI
{
    public class ReRollButton : MonoBehaviour
    {
        private void Start()
        {
            // 初期化時は隠す
            enabled = false;
        }

        /**
         * リロールボタンの表示, 時間指定で非表示にする
         */
        public void Show(int waitingTime = -1)
        {
            enabled = true;
            if (waitingTime > 0)
                StartCoroutine(General.DelayCoroutine(waitingTime, (() => enabled = false)));
        }

        public void AddClickEvent(Action action)
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => action());
        }
    }
}