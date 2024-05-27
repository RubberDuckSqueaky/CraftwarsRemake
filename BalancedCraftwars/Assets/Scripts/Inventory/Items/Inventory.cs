using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Recipe;
using static SaveAllItems;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject craftingPreview;
    [SerializeField] public TextMeshProUGUI itemName;
    [SerializeField] public TextMeshProUGUI itemDescription;
    [SerializeField] public TextMeshProUGUI itemRequirements;
    [SerializeField] public Button equipButton;
    [SerializeField] public Button unequipButton;

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
        UpdateInventory();
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
                    string realName = items.GetName(items.playerInventory[slotNumber].itemID, 0);
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
                string realName = items.GetName(items.playerInventory[i].itemID, 0);
                int quantity = items.playerInventory[slotNumber].quantity;
                inventorySlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().text = realName != null ? realName + " x" + quantity : "";
                inventorySlots[slotNumber].GetComponent<InventorySlot>().itemID = items.playerInventory[i].itemID;
                inventorySlots[slotNumber].item = items.GetItem(items.playerInventory[i].itemID, 2);
                if (items.playerInventory[i].isEquipped)
                {
                    inventorySlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                }
                else
                {
                    switch(items.playerInventory[i].inventoryTag)
                    {
                        case InventoryTag.Rare:
                            inventorySlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                            break;
                        case InventoryTag.Debug:
                            inventorySlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().color = Color.magenta;
                            break;
                        default:
                            inventorySlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                            break;
                    }
                }
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
                if (items.GetName(item.itemID, 0).ToLower().Contains(searchText.ToLower()))
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
        slotCount = 0;
        for (int i = 0; i < filteredInventory.Count && i < inventorySlots.Length; i++)
        {
            string realName = items.GetName(filteredInventory[i].itemID, 0);
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

    public void EquipButton()
    {
        var inventoryItem = items.playerInventory.Find(i => i.itemID == FindFirstObjectByType<SaveAllItems>().selectedItem.itemID);

        if(inventoryItem != null)
        {
            EquipItem(inventoryItem);
        }
    }

    public void UnequipButton()
    {
        var inventoryItem = items.playerInventory.Find(i => i.itemID == FindFirstObjectByType<SaveAllItems>().selectedItem.itemID);

        if (inventoryItem != null)
        {
            UnequipItem(inventoryItem);
        }
    }


    private void EquipItem(ItemInfo item)
    {
        var inventoryItem = FindFirstObjectByType<SaveAllItems>().playerInventory.Find(i => i.itemID == item.itemID);

        bool success = FindFirstObjectByType<SaveAllItems>().EquipItem(item);
        FindFirstObjectByType<HotbarManager>().AssigntoHotbar(FindFirstObjectByType<SaveAllItems>().GetItem(inventoryItem.itemID, 0), inventoryItem.itemTag);
        itemRequirements.text = success ? $"{item.itemID} equipped successfully!" : $"Cannot equip {item.itemID}!";

        if(inventoryItem != null && inventoryItem.isEquipped)
        {
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(true);
        }
    }
    public void UnequipItem(ItemInfo item)
    {
        var inventoryItem = FindFirstObjectByType<SaveAllItems>().playerInventory.Find(i => i.itemID == item.itemID);
        bool success = FindFirstObjectByType<SaveAllItems>().UnequipItem(item);
        FindFirstObjectByType<HotbarManager>().UnassignfromHotbar(FindFirstObjectByType<SaveAllItems>().GetItem(inventoryItem.itemID, 0), inventoryItem.itemTag);
        itemRequirements.text = success ? $"{item.itemID} unequipped successfully!" : $"Cannot unequip {item.itemID}!";

        if (inventoryItem != null && !inventoryItem.isEquipped)
        {
            equipButton.gameObject.SetActive(true);
            unequipButton.gameObject.SetActive(false);
        }
    }

}