using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using InvasionPhase;
using Enums;



public class STARTDOTween : MonoBehaviour
{   
    //public TextMeshProUGUI dotweenTextMeshPro;
    //public float dotweenInterval;
    // Start is called before the first frame update
    [SerializeField] 
    private InvasionController invasionController;
    bool flag;

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (invasionController.GameState == GameState.Playing && flag ==false) {
            this.transform.localScale = new Vector3(10.3f, 10.3f, 10.3f);
            //516
            this.transform.localPosition = new Vector3(-500f, 0f, 0f);
             // コンポ取得(TMPのあるふぁ初期設定)
            TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();

            // 現在のカラーを取得
            Color currentColor = textMeshPro.color;

            // 新しいアルファ値を設定
            currentColor.a = 0f; // 例として0fに設定

            // 設定した新しいカラーを適用
            textMeshPro.color = currentColor;
            transform.DOLocalMove(new Vector3(500, 0, 0), 0.35f).SetRelative(true).SetEase(Ease.InOutQuad);
            transform.DOScale(new Vector3(-5f, -5f, -5f), 0.4f).SetRelative(true).SetEase(Ease.InBack);//秒で大きさをにする
           //this.rendererComponent.material.DOFade(endValue: 0f, duration: 0.3f);//秒でMaterialのアルファをにする
            this.GetComponent<TextMeshProUGUI>().DOFade(1f, 0.5f).Play();

            //フェードアウトも実装したいので、アルファの逆
            this.GetComponent<TextMeshProUGUI>().DOFade(0f, 0.3f).Play().SetDelay(0.5f);
            flag = true;
        }
    }
}
