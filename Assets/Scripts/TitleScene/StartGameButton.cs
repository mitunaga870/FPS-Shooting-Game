using UnityEngine;

namespace TitleScene
{
    public class StartGameButton : MonoBehaviour
    {
        void Start()
        {
            // クリック時にStartGameを呼び出す
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        }

        private static void StartGame()
        {
            // セーブデータ削除
            SaveController.DelSave();

            UnityEngine.SceneManagement.SceneManager.LoadScene("CreatePhase");
        }
    }
}