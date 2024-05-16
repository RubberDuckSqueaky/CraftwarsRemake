using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private GameObject craftingPreview;
    public CraftingMenu craftingInventory;
    public SaveAllItems items;
    public List<Recipe> craftableItems;

    public Recipe selectedRecipe;

    public void Start()
    {
        selectedRecipe = null;
        craftingPreview.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            craftingInventory.UpdateCraftingInventory();
        }
    }

    public void CraftItem()
    {
        if(selectedRecipe != null)
        {
            int index = 0;
            foreach(var ingredient in selectedRecipe.ingredients)
            {
                if (selectedRecipe.ingredients[index].consumeItem)
                {
                    int itemID = selectedRecipe.ingredients[index].item.itemNumber;
                    int numbertoRemove = selectedRecipe.ingredients[index].quantity;

                    items.RemoveItem(itemID, numbertoRemove);
                }
            }
        }
        else
        {
            Debug.Log("No recipe found!");
        }
    }

    public void DisplayCraftingPreview(int itemNumberID)
    {

    }



}
