using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TitleScene
{
    public class CreditButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject loading;

        [SerializeField]
        private GameObject loadingEnemy;

        //ロード進捗状況を管理するための変数
        private AsyncOperation async;


        void Start()
        {
            // クリック時にStartGameを呼び出す
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartCredit);
        }


        public void StartCredit()
        {
            loading.SetActive(true);
            loadingEnemy.SetActive(true);

            // ロードを開始するメソッド
            StartCoroutine(Load());
        }

        private IEnumerator Load() {
            // シーンを非同期でロードする
            async = SceneManager.LoadSceneAsync("Credit");

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