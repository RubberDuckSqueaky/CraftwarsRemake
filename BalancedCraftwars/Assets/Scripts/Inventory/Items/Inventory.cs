using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static SaveAllItems;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    private SaveAllItems items;

    private List<ItemInfo> filteredInventory = new List<ItemInfo>();

    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;

    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [SerializeField] private int slotCount = 0;
    [SerializeField] private bool filter = false;
    [SerializeField] private int pageNumber = 1;

    private void Start()
    {
        EnableInput();
        items = FindFirstObjectByType<SaveAllItems>();
        pageNumber = 1;
        slotCount = 0;
        filter = false;
    }

    // keep an eye on this code below... :3
    
    public void UpdateInventory()
    {
        int slotNumber = 0;

        foreach (var slot in inventorySlots)
        {
            if (items.playerInventory != null)
            {
                if (slotNumber >= items.playerInventory.Count)
                {
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = ""; // Clear extra slots
                }
                else
                {
                    string realName = items.GetName(items.playerInventory[slotNumber].itemID);
                    int quantity = items.playerInventory[slotNumber].quantity;
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = realName != null ? realName + " x" + quantity : "";
                    slotNumber++;
                }
            }
        }
    }

    public void NextPage()
    {
        if(filter)
        {
            if (filteredInventory != null && filteredInventory.Count > inventorySlots.Length)
            {
                slotCount += inventorySlots.Length;

                if (slotCount >= filteredInventory.Count)
                {
                    slotCount -= inventorySlots.Length; // Prevent going out of bounds
                    Debug.Log("Can't do this!");
                    return;
                }

                pageNumber += 1;
                UpdateFilteredInventory();
            }
        }
        else if (items.playerInventory != null && items.playerInventory.Count > inventorySlots.Length)
        {
            slotCount += inventorySlots.Length;

            if (slotCount >= items.playerInventory.Count)
            {
                slotCount -= inventorySlots.Length; // Prevent going out of bounds
                Debug.Log("Can't do this!");
                return;
            }

            pageNumber += 1;
            UpdateInventoryPage();
        }
    }
    public void PreviousPage()
    {
        if (filter)
        {
            if (filteredInventory != null && filteredInventory.Count > inventorySlots.Length)
            {
                slotCount -= inventorySlots.Length;

                if (slotCount < 0)
                {
                    slotCount = 0; // Prevent going out of bounds
                    Debug.Log("Can't do this!");
                    return;
                }

                pageNumber -= 1;
                UpdateFilteredInventory();
            }
        }
        else if (items.playerInventory != null && slotCount > 0)
        {
            slotCount -= inventorySlots.Length;

            if (slotCount < 0)
            {
                slotCount = 0; // Prevent going out of bounds
                Debug.Log("Can't do this!");
                return;
            }

            pageNumber -= 1;
            UpdateInventoryPage();
        }
    }

    // keep an eye on this code above ... :3

    public void UpdateInventoryPage()
    {
        // Clear all slots first
        foreach (var slot in inventorySlots)
        {
            slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        int slotNumber = 0;
        for (int i = slotCount; i < slotCount + inventorySlots.Length; i++)
        {
            if (i < items.playerInventory.Count)
            {
                string realName = items.GetName(items.playerInventory[i].itemID);
                int quantity = items.playerInventory[slotNumber].quantity;
                inventorySlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().text = realName != null ? realName + " x" + quantity : "";
                slotNumber++;
            }
        }
    }

    public void FilterInventory(string searchText)
    {
        filteredInventory.Clear();

        if(string.IsNullOrEmpty(searchText))
        {
            filter = false;
            filteredInventory.AddRange(items.playerInventory);
        }
        else
        {
            filter = true;
            foreach (var item in items.playerInventory)
            {
                if (items.GetName(item.itemID).ToLower().Contains(searchText.ToLower()))
                {
                    filteredInventory.Add(item);
                }
            }
        }

        UpdateFilteredInventory();
    }

    private void UpdateFilteredInventory()
    {
        foreach (var slot in inventorySlots)
        {
            slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        int slotNumber = 0;
        for (int i = 0; i < filteredInventory.Count && i < inventorySlots.Length; i++)
        {
            string realName = items.GetName(filteredInventory[i].itemID);
            int quantity = filteredInventory[i].quantity;
            inventorySlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().text = realName != null ? realName + " x" + quantity : "";
            slotNumber++;
        }
    }

    public void DisableInput()
    {
        InputSystem.DisableDevice(Keyboard.current);
    }
    public void EnableInput()
    {
        InputSystem.EnableDevice(Keyboard.current);
    }

}