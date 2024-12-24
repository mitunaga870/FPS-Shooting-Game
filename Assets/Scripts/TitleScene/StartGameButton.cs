using UnityEngine;

namespace TitleScene
{
    public class StartGameButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject loading;
        
        [SerializeField]
        private GameObject loadingEnemy;
        
        void Start()
        {
            // クリック時にStartGameを呼び出す
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        }


        public void StartGame()
        {
            loading.SetActive(true);
            loadingEnemy.SetActive(true);
            
            // セーブデータ削除
            SaveController.DelSave();

            UnityEngine.SceneManagement.SceneManager.LoadScene("CreatePhase");
        }
    }
}