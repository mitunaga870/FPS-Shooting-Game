using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる


public class TrapFan_IgnitionAction : MonoBehaviour
{   
    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 0;
    [SerializeField] private float Z = 1800;
    [SerializeField] private string ActionKey = "U";
    
    // 遷移に１秒かける
    private const float transitionTime = 0.5f;
    
    // ループ感覚
    private const float loopInterval = 0.25f;
    
    // 回転中フラグ
    private bool isRotating = false;

    /** アニメーションを再生する */
    public void IgnitionAction()
    {
        if (isRotating) return;
        isRotating = true;
        
        //time秒間にX,Y,Z度回転させる
        transform.DOLocalRotate(new Vector3(X,Y,Z),transitionTime,RotateMode.FastBeyond360).SetEase(Ease.InQuart)
            .OnComplete((() =>
            {
                // 無限回転
                transform.DOLocalRotate(new Vector3(X,Y,Z),loopInterval,RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear).
                    SetLoops(-1,LoopType.Restart);
            }));
    }
    
    /** アニメーションを停止する */
    public void StopAction()
    {
        Debug.Log("StopAction");
        isRotating = false;
        
        // 無限回転を停止
        transform.DOKill();
        
        //time秒間にX,Y,Z度回転させる
        transform.DOLocalRotate(new Vector3(0,0,0),transitionTime,RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart);
    }
}
