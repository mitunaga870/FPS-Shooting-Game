using UnityEngine;

namespace TitleScene
{
    public class StartGameButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject loading;
        
        [SerializeField]
        private GameObject loadingEnemy;
        private Animator anim;
        
        void Start()
        {
            // クリック時にStartGameを呼び出す
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
            anim = loadingEnemy.GetComponent<Animator>();
        }


        public void StartGame()
        {
            loading.SetActive(true);
            loadingEnemy.SetActive(true);
            anim.Play("Run");
            
            // セーブデータ削除
            SaveController.DelSave();

            UnityEngine.SceneManagement.SceneManager.LoadScene("CreatePhase");
        }
    }
}