using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public List<Item> hotbarList;
    public List<HotbarSlot> hotbarSlots;

    public void Start()
    {
        foreach (var slot in hotbarSlots)
        {
            slot.hotbarText.text = "";
        }
    }

    public void AssigntoHotbar(Item selected, SlotTag tag)
    {
        foreach (var slot in hotbarSlots)
        {
            if(slot.myTag == tag && slot.item == null)
            {
                slot.hotbarText.text = selected.itemName;
                slot.item = selected;
                return;
            }
        }
    }

    public void UnassignfromHotbar(Item deselect, SlotTag tag)
    {
        foreach (var slot in hotbarSlots)
        {
            if (slot.myTag == tag && slot.item == deselect)
            {
                slot.hotbarText.text = "";
                slot.item = null;
                return;
            }
            else
            {
                Debug.Log("Nothing found!");
            }
        }
    }


}
