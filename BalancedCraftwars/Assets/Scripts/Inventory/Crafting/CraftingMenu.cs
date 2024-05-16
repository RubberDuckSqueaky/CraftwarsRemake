using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.InputSystem;
using static SaveAllItems;

public class CraftingMenu : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    private CraftingManager crafting;
    private SaveAllItems items;

    private List<ItemInfo> filteredCrafting = new List<ItemInfo>();

    [SerializeField] InventorySlot[] craftingSlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [SerializeField] private int slotCount = 0;
    [SerializeField] private bool filter = false;
    [SerializeField] private int pageNumber = 1;

    private void Start()
    {
        EnableInput();
        crafting = FindFirstObjectByType<CraftingManager>();
        items = FindFirstObjectByType<SaveAllItems>();
        pageNumber = 1;
        slotCount = 0;
        filter = false;
    }

    public void UpdateCraftingInventory()
    {
        int slotNumber = 0;

        foreach (var slot in craftingSlots)
        {
            if (crafting.craftableItems != null)
            {
                if (slotNumber >= crafting.craftableItems.Count)
                {
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = ""; // Clear extra slots
                }
                else
                {
                    string realName = items.GetName(crafting.craftableItems[slotNumber].outputItem.itemID);
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = realName != null ? realName : "";
                    slot.recipe = crafting.craftableItems[slotNumber];
                    slotNumber++;
                }
            }
        }
    }

    public void NextPage()
    {
        if (filter)
        {
            if (filteredCrafting != null && filteredCrafting.Count > craftingSlots.Length)
            {
                slotCount += craftingSlots.Length;

                if (slotCount >= filteredCrafting.Count)
                {
                    slotCount -= craftingSlots.Length; // Prevent going out of bounds
                    Debug.Log("Can't do this!");
                    return;
                }

                pageNumber += 1;
                UpdateFilteredInventory();
            }
        }
        else if (crafting.craftableItems != null && crafting.craftableItems.Count > craftingSlots.Length)
        {
            slotCount += craftingSlots.Length;

            if (slotCount >= crafting.craftableItems.Count)
            {
                slotCount -= craftingSlots.Length; // Prevent going out of bounds
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
            if (filteredCrafting != null && filteredCrafting.Count > craftingSlots.Length)
            {
                slotCount -= craftingSlots.Length;

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
        else if (crafting.craftableItems != null && slotCount > 0)
        {
            slotCount -= craftingSlots.Length;

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
        foreach (var slot in craftingSlots)
        {
            slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        int slotNumber = 0;
        for (int i = slotCount; i < slotCount + craftingSlots.Length; i++)
        {
            if (i < crafting.craftableItems.Count)
            {
                string realName = items.GetName(crafting.craftableItems[i].outputItem.itemID);
                craftingSlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().text = realName != null ? realName : "";
                slotNumber++;
            }
        }
    }

    public void FilterInventory(string searchText)
    {
        filteredCrafting.Clear();

        if (string.IsNullOrEmpty(searchText))
        {
            filter = false;
            filteredCrafting.AddRange(items.playerInventory);
        }
        else
        {
            filter = true;
            foreach (var item in items.playerInventory)
            {
                if (items.GetName(item.itemID).ToLower().Contains(searchText.ToLower()))
                {
                    filteredCrafting.Add(item);
                }
            }
        }

        UpdateFilteredInventory();
    }

    private void UpdateFilteredInventory()
    {
        foreach (var slot in craftingSlots)
        {
            slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        int slotNumber = 0;
        for (int i = 0; i < filteredCrafting.Count && i < craftingSlots.Length; i++)
        {
            string realName = items.GetName(filteredCrafting[i].itemID);
            craftingSlots[slotNumber].GetComponentInChildren<TextMeshProUGUI>().text = realName != null ? realName: "";
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
