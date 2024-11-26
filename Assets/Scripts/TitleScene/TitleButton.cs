using UnityEngine;

namespace TitleScene
{
    public class TitleButton : MonoBehaviour
    {
        void Start()
        {
            // クリック時にStartGameを呼び出す
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartTitle);
        }


        public static void StartTitle()
        {
            // セーブデータ削除
            //SaveController.DelSave();
            Debug.Log("gyaaa");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }
}