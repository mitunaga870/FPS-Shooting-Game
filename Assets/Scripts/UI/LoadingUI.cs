using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public GameObject loading;

    // Start is called before the first frame update
    void Start()
    {
        loading.SetActive(false);
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(nowLoading);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void nowLoading() {
        loading.SetActive(true);
    }
}
