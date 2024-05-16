using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }
    public SlotTag myTag;
    public bool hovered;
    public Item heldItem;
    public int quantity;

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


}
