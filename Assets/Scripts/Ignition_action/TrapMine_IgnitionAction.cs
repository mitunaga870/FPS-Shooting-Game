using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class TrapMine_IgnitionAction : MonoBehaviour
{
    [SerializeField] private float time = 4;
    private int Check = 0; //ディレイ時間を過ぎているかどうか
    [SerializeField] GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        obj.SetActive(false);   // 無効にする
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKey (KeyCode.M	)) {
            Debug.Log(Check);
            if(Check== 0){
                obj.SetActive(true);
                Check = 1;
                DOTween.Sequence().Append(DOVirtual.DelayedCall(time, () => Check=0));
                //DOTween.Sequence().Append(DOVirtual.DelayedCall(time, () => this.gameObject.SetActive(false))); //time goni mukou
            }
        }
            
    }
}
