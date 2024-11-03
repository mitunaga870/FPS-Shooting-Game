using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class TrapMine_IgnitionAction : MonoBehaviour
{
    [SerializeField] private float time = 4;
    private int Check = 0; //ディレイ時間を過ぎているかどうか
    [SerializeField]
    private GameObject ExplosionEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        ExplosionEffect.SetActive(false);
        
        // 角度をつける
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    public void StartAction(){
        if (Check == 1) return;
        
        Check = 1;
        ExplosionEffect.SetActive(true);
        
        DOTween.Sequence().Append(DOVirtual.DelayedCall(time, () =>
        {
            Check = 0;
        }));
    }
}
