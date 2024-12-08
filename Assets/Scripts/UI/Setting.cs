using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnOpenSetting() {
        gameObject.SetActive(true);
    }

    public void OnCloseButton() {
        gameObject.SetActive(false);
    }

    public void OnTitleButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
