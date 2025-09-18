using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Game/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public ItemBase[] allItems;

    public ItemBase GetItemById(string id)
    {
        foreach(var item in allItems)
        {
            if (item.id == id) return item;
        }
        return null;
    }
}
