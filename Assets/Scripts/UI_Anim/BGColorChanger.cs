using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BGColorChanger : MonoBehaviour
{
    float H;
    float S;
    float V;
//http://www.yuchi-appdev.com/?p=33
    // Start is called before the first frame update
    void Start()
    {
        H = 0f;
        S = 93.4f;
        V = 78f;
    }

    // Update is called once per frame
    void Update()
    {

        //5秒かけてaを100まで変化させる
        DOTween.To(() => H, (x) => H = x, 100, 30);

        

        //GetComponent<UnityEngine.Camera>().backgroundColor = Color.HSVToRGB(0,93.4f,73);
        GetComponent<Renderer>().material.color = Color.HSVToRGB(0,93.4f,73);
    }
}
