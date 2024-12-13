using System.Collections;
using lib;
using TMPro;
using UnityEngine;

namespace Chat
{
    /**
     * メッセージボックスのコントローラ
     * テキストを制御して表示・更新・非表示を行う
     */
    public class MessageBoxController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI messageText;

        private IEnumerator _closeCoroutine;
    
        /** メッセージボックスに表示した後消えるまでの時間 */
        private readonly int _closeDelay = 3;
    
        public void SetMessage(string message)
        {
            if (_closeCoroutine != null)
                StopCoroutine(_closeCoroutine);
        
            messageText.text = message;
            gameObject.SetActive(true);

            _closeCoroutine = General.DelayCoroutine(
                _closeDelay,
                () => gameObject.SetActive(false)
            );
            StopCoroutine(_closeCoroutine);
        }
    
        public void SetMessages(string[] messages)
        {
            if (_closeCoroutine != null)
                StopCoroutine(_closeCoroutine);
        
            // 一つ目を表示
            messageText.text = messages[0];
            gameObject.SetActive(true);
        
            // 最後なら終了
            if (messages.Length == 1)
            {
                _closeCoroutine = General.DelayCoroutine(
                    _closeDelay,
                    () => gameObject.SetActive(false)
                );
                StartCoroutine(_closeCoroutine);
            
                return;
            }
        
            // 残りを取得
            var restMessages = new string[messages.Length - 1];
            for (var i = 1; i < messages.Length; i++)
                restMessages[i - 1] = messages[i];
        
            // 再起的に表示
            _closeCoroutine = General.DelayCoroutine(
                _closeDelay,
                () => SetMessages(restMessages)
            );
            StartCoroutine(_closeCoroutine);
        }
    }
}