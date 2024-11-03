using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{   

    public GameObject prefabObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKey (KeyCode.A)) {
            CreateObject();
        }
    }

    void CreateObject()
    {
        // ゲームオブジェクトを生成します。
        GameObject obj = Instantiate(prefabObj,new Vector3(-122, 582, -1546), Quaternion.identity);
    }
}
