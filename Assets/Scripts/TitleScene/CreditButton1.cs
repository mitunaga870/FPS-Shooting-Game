using UnityEngine;

namespace TitleScene
{
    public class CreditButton : MonoBehaviour
    {
        void Start()
        {
            // クリック時にStartGameを呼び出す
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartCredit);
        }


        public static void StartCredit()
        {
            // セーブデータ削除
            //SaveController.DelSave();

            UnityEngine.SceneManagement.SceneManager.LoadScene("Credit");
        }
    }
}