using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる


public class CrocTrap_ActionIgnition : MonoBehaviour
{
    [SerializeField] private float time = 4;
    //===== 定義領域 =====
    private Animator anim;  //Animatorをanimという変数で定義する

    // Start is called before the first frame update
    void Start()
    {
        //変数animに、Animatorコンポーネントを設定する
        anim = gameObject.GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.C)) {
            //https://qiita.com/sanpeita/items/3864578e852801c15f80
            anim.SetBool("RollBool", true);
        }
    }
}
