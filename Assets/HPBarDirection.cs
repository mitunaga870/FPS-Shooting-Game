using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI使うときは忘れずに。
using UnityEngine.UI;

public class HPBarDirection : MonoBehaviour
{
    public Canvas canvas;
    
    void Update()
    {
        //EnemyCanvasをMain Cameraに向かせる
        canvas.transform.rotation = 
            Camera.main.transform.rotation;
    }
}