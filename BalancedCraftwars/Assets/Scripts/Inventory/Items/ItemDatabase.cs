using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public Item[] items;

    public Item GetItem(int itemNumber)
    {
        foreach (var item in items)
        {
            if (item != null && item.itemNumber == itemNumber) return item;
        }
        return null;
    }
}
