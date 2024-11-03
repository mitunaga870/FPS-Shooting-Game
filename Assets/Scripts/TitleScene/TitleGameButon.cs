using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGameButon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void TitleGame()
        {
            // セーブデータ削除
            //SaveController.DelSave();

            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
}
