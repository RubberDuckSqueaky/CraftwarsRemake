using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }
    public SlotTag myTag;
    public bool hovered;
    public int quantity;

    public string itemID = null;
    public Item item;
    public Recipe recipe;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(recipe != null)
            {
                FindFirstObjectByType<CraftingManager>().selectedRecipe = recipe;
                Debug.Log("Recipe Selected: " + FindFirstObjectByType<CraftingManager>().selectedRecipe);
            }
            else if(item != null)
            {
                FindFirstObjectByType<SaveAllItems>().selectedItem = item;
                Debug.Log("Item selected: " + FindFirstObjectByType<SaveAllItems>().selectedItem);
            }
        }
    }

    public void SetItem(InventoryItem item)
    {
        Inventory.carriedItem = null;

        item.activeSlot.myItem = null;

        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;

        if(myTag != SlotTag.None)
        {

        }
    }

    public void DisplayInventoryPreview()
    {
        var inventoryItem = FindFirstObjectByType<SaveAllItems>().playerInventory.Find(i => i.itemID == itemID);

        if (itemID != null && inventoryItem != null)
        {

            if(inventoryItem.isEquipped)
            {
                FindFirstObjectByType<Inventory>().equipButton.gameObject.SetActive(false);
                FindFirstObjectByType<Inventory>().unequipButton.gameObject.SetActive(true);
            }
            else
            {
                FindFirstObjectByType<Inventory>().equipButton.gameObject.SetActive(true);
                FindFirstObjectByType<Inventory>().unequipButton.gameObject.SetActive(false);
            }

            string name = FindFirstObjectByType<SaveAllItems>().GetName(itemID, 0);
            string description = FindFirstObjectByType<SaveAllItems>().GetDescription(itemID);

            FindFirstObjectByType<Inventory>().itemName.text = name;
            FindFirstObjectByType<Inventory>().itemDescription.text = description;
            // FindFirstObjectByType<Inventory>().stats.text = stats;
        }
    }

    public void DisplayCraftingPreview()
    {
        if(itemID != null)
        {
            string name = FindFirstObjectByType<SaveAllItems>().GetName(itemID, 1);
            string description = FindFirstObjectByType<SaveAllItems>().GetDescription(itemID);
            string requirements = "";
            foreach (var ingredient in recipe.ingredients)
            {
                int quantity = FindFirstObjectByType<SaveAllItems>().GetIngredientQuantity(ingredient.item.itemID, recipe);
                Debug.Log(quantity);
                if(!ingredient.consumeItem)
                {
                    requirements += FindFirstObjectByType<SaveAllItems>().GetName(ingredient.item.itemID, 3) + " x" + quantity + " (Not Consumed!), ";
                }
                else
                {
                    requirements += FindFirstObjectByType<SaveAllItems>().GetName(ingredient.item.itemID, 3) + " x" + quantity + ", ";
                }
            }
        
            FindFirstObjectByType<CraftingMenu>().itemName.text = name;
            FindFirstObjectByType<CraftingMenu>().itemDescription.text = description;
            FindFirstObjectByType<CraftingMenu>().itemRequirements.text = requirements;
        }
    }

    public IEnumerator DenyCrafting()
    {
        if (FindFirstObjectByType<CraftingMenu>().craftingText.text == "Can't craft!")
        {
            FindFirstObjectByType<CraftingMenu>().craftingText.text = "Craft";
            StopAllCoroutines();
        }
        FindFirstObjectByType<CraftingMenu>().craftingText.text = "Can't craft!";
        yield return new WaitForSeconds(3);
        FindFirstObjectByType<CraftingMenu>().craftingText.text = "Craft";
        StopAllCoroutines();
    }
    public IEnumerator CompleteCrafting()
    {
        if(FindFirstObjectByType<CraftingMenu>().craftingText.text == "Crafted")
        {
            FindFirstObjectByType<CraftingMenu>().craftingText.text = "Craft";
            StopAllCoroutines();
        }
        FindFirstObjectByType<CraftingMenu>().craftingText.text = "Crafted";
        yield return new WaitForSeconds(3);
        FindFirstObjectByType<CraftingMenu>().craftingText.text = "Craft";
        StopAllCoroutines();
    }
    public void CompleteCraft()
    {
        StartCoroutine(CompleteCrafting());
    }
    public void DenyCraft()
    {
        StartCoroutine(DenyCrafting());
    }


}
