using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public List<Item> hotbarList;
    public List<HotbarSlot> hotbarSlots;
    PlayerEquipment equipment;

    public void Awake()
    {
        equipment = FindFirstObjectByType<PlayerEquipment>();
    }
    public void Start()
    {
        foreach (var slot in hotbarSlots)
        {
            slot.hotbarText.text = "";
            slot.equipped = false;
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(!hotbarSlots[0].equipped)
            {
                GameObject item = hotbarSlots[0].item.itemToEquip;
                Equip(item);
                hotbarSlots[0].equipped = true;
            }
            else
            {
                hotbarSlots[0].equipped = false;
            }
        }
    }


    public void Equip(GameObject itemtoEquip)
    {
        GameObject item = GameObject.Instantiate(itemtoEquip, transform.position, equipment.parent.transform.rotation);
        item.transform.parent = equipment.handAnchor01.transform;
        item.transform.position = equipment.handAnchor01.transform.position;
    }
    public void AssigntoHotbar(Item selected, SlotTag tag)
    {
        foreach (var slot in hotbarSlots)
        {
            if(slot.myTag == tag && slot.item == null)
            {
                slot.hotbarText.text = selected.itemName;
                slot.item = selected;
                slot.itemtoEquip = selected.itemToEquip;
                slot.equipped = false;
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
                slot.itemtoEquip = null;
                slot.equipped = false;
                return;
            }
            else
            {
                Debug.Log("Nothing found!");
            }
        }
    }


}
