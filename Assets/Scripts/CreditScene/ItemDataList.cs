using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList", menuName = "ScriptableObjects/ItemDataList", order = 2)]
public class ItemDataList : ScriptableObject
{
    public List<ItemData> items;  // ItemDataのリスト
}
