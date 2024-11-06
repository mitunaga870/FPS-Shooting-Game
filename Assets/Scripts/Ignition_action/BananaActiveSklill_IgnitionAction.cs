using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PolygonArsenal; //DOTweenを使うときはこのusingを入れる

public class BananaActiveSklill_IgnitionAction : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;
    
    [SerializeField]
    private PolygonSoundSpawn _polygonSoundSpawn;
    
    public void IgnitionAction()
    {
        // パーティクル再生
        _particleSystem.Play();
        
        // 音再生
        _polygonSoundSpawn.IgnitionAction();
        
        // バナナを消す
        Destroy(gameObject, 1.0f);
    }
}
