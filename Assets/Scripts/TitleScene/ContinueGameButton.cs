using System;
using Enums;
using UnityEngine;

namespace TitleScene
{
    public class ContinueGameButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject loading;
        
        [SerializeField]
        private GameObject loadingEnemy;
        
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

            // セーブデータのSceneに移動
            switch (SaveController.LoadPhase())
            {
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
        }
    }
}