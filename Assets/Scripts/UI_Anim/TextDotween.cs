using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;   // 追加
using LitMotion;
using LitMotion.Extensions;
using ScriptableObjects.S2SDataObjects;

public class TextDotween : MonoBehaviour
{
[SerializeField] float time;
[SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
       LMotion.Create(0f, 0, time).BindToText(text,"{0:N0}");
       //https://annulusgames.github.io/LitMotion/articles/ja/text-animation.html
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
