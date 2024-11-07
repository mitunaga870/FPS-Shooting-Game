using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public ItemDataList itemDataList;  // ItemDataListへの参照
    public Transform parentTransform;  // 生成したPrefabの親オブジェクト

    void Start()
    {
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        foreach (ItemData itemData in itemDataList.items)
        {
            if (itemData.prefab != null)
            {
                // Prefabを生成し、親オブジェクトに設定
                GameObject prefabInstance = Instantiate(itemData.prefab, parentTransform);
                prefabInstance.name = itemData.itemName;  // オブジェクト名を設定
            }
            else
            {
                Debug.LogWarning($"{itemData.itemName} のPrefabが設定されていません");
            }
        }
    }
}