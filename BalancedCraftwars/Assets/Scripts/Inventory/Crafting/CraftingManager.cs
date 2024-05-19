using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public CraftingMenu craftingInventory;
    public SaveAllItems items;
    public List<Recipe> craftableItems;

    public Recipe selectedRecipe;

    public void Start()
    {
        selectedRecipe = null;
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
        if (selectedRecipe != null)
        {
            bool canCraft = true;

            foreach (var ingredient in selectedRecipe.ingredients)
            {
                var inventoryItem = items.playerInventory.Find(i => i.itemID == ingredient.item.itemID);
                if (inventoryItem == null || inventoryItem.quantity < ingredient.quantity)
                {
                    canCraft = false;
                    return;
                }
            }

            if (canCraft)
            {
                foreach (var ingredient in selectedRecipe.ingredients)
                {
                    if(ingredient.consumeItem)
                    {
                        var inventoryItem = items.playerInventory.Find(i => i.itemID == ingredient.item.itemID);
                        items.RemoveItem(inventoryItem.itemNumberID, ingredient.quantity);
                    }
                }
            }
            items.AddItem(selectedRecipe.outputItem.itemNumber, 1);
        }
        else
        {
            Debug.Log("No recipe found!");
            FindFirstObjectByType<InventorySlot>().DenyCraft();
            return;
        }
    }


}
