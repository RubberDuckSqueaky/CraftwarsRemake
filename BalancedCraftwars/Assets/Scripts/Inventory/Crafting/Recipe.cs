using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}
