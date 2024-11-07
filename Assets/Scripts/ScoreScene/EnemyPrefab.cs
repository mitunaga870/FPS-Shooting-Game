using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    [SerializeField]
    private GeneralS2SData generalS2SData;

    public GameObject prefabObj;
    
    private int enemyCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (enemyCount <= generalS2SData.Score)
        {
            CreateObject();
            enemyCount++;
        }
    }

    void CreateObject()
    {
        // ゲームオブジェクトを生成します。
        GameObject obj = Instantiate(prefabObj,new Vector3(-122, 582, -1546), Quaternion.identity);
    }
}
