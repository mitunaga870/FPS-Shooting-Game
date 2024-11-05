using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;   // 追加
using LitMotion;
using LitMotion.Extensions;

public class TextDotween : MonoBehaviour
{
[SerializeField] float time;
[SerializeField] float BUNSHO; //ここにスコアに点数を入れる予定
[SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
       LMotion.Create(0f, BUNSHO, time).BindToText(text,"{0:N0}");
       //https://annulusgames.github.io/LitMotion/articles/ja/text-animation.html
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
