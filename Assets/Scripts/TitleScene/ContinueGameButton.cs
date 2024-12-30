using System;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TitleScene
{
    public class ContinueGameButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject loading;
        
        [SerializeField]
        private GameObject loadingEnemy;

        //ロード進捗状況を管理するための変数
        private AsyncOperation async;

        private void Start()
        {
            // クリック時にStartGameを呼び出す
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        }

        public void StartGame()
        {
            // ローディング画面を表示
            loading.SetActive(true);
            loadingEnemy.SetActive(true);
            
            Debug.Log("StartGame");

            // ゲームオーバーかどうか
            if (SaveController.LoadGameOvered()) {
                return;
            }

            // ロードを開始するメソッド
            StartCoroutine(Load());
        }

        private IEnumerator Load() {

            // セーブデータのSceneに移動
            switch (SaveController.LoadPhase()) {
                case Phase.Create:
                    // ゲーム開始
                    UnityEngine.SceneManagement.SceneManager.LoadScene("CreatePhase");
                    break;
                case Phase.Invade:
                    // ゲーム開始
                    UnityEngine.SceneManagement.SceneManager.LoadScene("InvasionPhase");
                    break;
                default:
                    throw new Exception("セーブデータがおかしい（Sceneが未実装）");
            }

            // ロードが完了するまで待機する
            while (!async.isDone) {
                yield return null;
            }

            // ロード画面を非表示にする
            loading.SetActive(false);
            loadingEnemy.SetActive(false);
        }
    }
}