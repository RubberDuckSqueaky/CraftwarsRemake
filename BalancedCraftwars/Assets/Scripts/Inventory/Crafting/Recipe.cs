using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Scriptable Objects/Recipe")]
[Serializable]
public class Recipe : ScriptableObject
{
    public Item outputItem;
    public List<Ingredient> ingredients;

    [Serializable]
    public class Ingredient
    {
        public Item item;
        public int quantity;
        public bool consumeItem = true;
    }

    public string GetItemID(string itemID)
    {
        Ingredient craftingItem = ingredients.Find(item => item.item.itemID == itemID);

        if (craftingItem != null)
        {
            foreach (var item in ingredients)
            {
                if (item.item.itemID == craftingItem.item.itemID)
                {
                    return item.item.itemID;
                }
            }
        }
        else
        {
            Debug.Log("No item found!");
        }
        return null;
    }


}
