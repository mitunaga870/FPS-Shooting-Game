using System;
using lib;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class ReRollButton : MonoBehaviour
    {
        [SerializeField] private Canvas reRollCanvas;

        private void Start()
        {
            // 迷路作成中非表示用で書いてるが、迷路作成処理のが早いと表示されないのでとりまコメントアウト
            // reRollCanvas.enabled = false;
        }

        /**
         * リロールボタンの表示, 時間指定で非表示にする
         */
        public void Show(int waitingTime = -1)
        {
            reRollCanvas.enabled = true;
            if (waitingTime > 0)
                StartCoroutine(General.DelayCoroutine(waitingTime, (() => reRollCanvas.enabled = false)));
        }

        /**
         * クリックイベントの追加
         */
        public void AddClickEvent(Action action)
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => action());
        }

        /**
         * リロールボタンの非表示
         */
        public void Hide()
        {
            reRollCanvas.enabled = false;
        }
    }
}