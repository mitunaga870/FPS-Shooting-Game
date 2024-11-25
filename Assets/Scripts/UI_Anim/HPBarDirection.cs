using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//https://unity-shoshinsha.biz/archives/1194\
//UIをカメラに向かせ続けるやつ
public class HPBarDirection : MonoBehaviour
{
     public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.rotation = 
            Camera.main.transform.rotation;
    }
}
