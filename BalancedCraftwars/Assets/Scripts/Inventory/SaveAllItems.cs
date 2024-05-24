using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class SaveAllItems : MonoBehaviour
{
    public Item[] items;

    public List<ItemInfo> playerInventory;

    public Item selectedItem;

    private Dictionary<SlotTag, int> equipLimits = new Dictionary<SlotTag, int>
    {
        { SlotTag.Item, 4 },
        { SlotTag.Weapon, 3 },
        { SlotTag.Equipment, 3 },
        { SlotTag.Helmet, 1 },
        { SlotTag.Armor, 1 },
        { SlotTag.Shield, 1 },
        { SlotTag.Neck, 1 },
        { SlotTag.Back, 1 },
        { SlotTag.Accessory, 5 },
    };
    public Dictionary<SlotTag, int> equippedCounts = new Dictionary<SlotTag, int>();

    [Serializable]
    public class ItemInfo
    {
        public string itemID;
        public int itemNumberID;
        public int quantity;
        public SlotTag itemTag;
        public InventoryTag inventoryTag;
        public bool isEquipped = false;
    }

    private void Awake()
    {
        foreach (SlotTag tag in System.Enum.GetValues(typeof(SlotTag)))
        {
            equippedCounts[tag] = 0;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SaveToJson();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            int random = UnityEngine.Random.Range(0, items.Length);

            AddItem(random, 5);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            int randomItem = UnityEngine.Random.Range(0, items.Length);

            RemoveItem(randomItem, 1);
        }
    }

    public void SaveToJson()
    {
        var jsonStr = JsonConvert.SerializeObject(playerInventory);

        string json = JsonUtility.ToJson(jsonStr);
        string filePath = Path.Combine(Application.persistentDataPath, "InventoryData.json");
        File.WriteAllText(filePath, jsonStr);

        Debug.Log(Application.persistentDataPath);
    }

    public void AddItem(int id, int itemstoAdd)
    {
        foreach (var item in items)
        {
            if (item.itemNumber == id)
            {
                AddToInventory(id, itemstoAdd);
                FindFirstObjectByType<Inventory>().UpdateInventoryPage();
                return;
            }
        }
    }
    public void RemoveItem(int id, int itemstoAdd)
    {
        foreach (var item in items)
        {
            item.quantity = 0;

            if (item.itemNumber == id)
            {
                RemoveFromInventory(id, itemstoAdd);
                FindFirstObjectByType<Inventory>().UpdateInventoryPage();
            }
        }
    }


    private void AddToInventory(int id, int itemstoAdd)
    {
        Item item = items[id];
        if(item == null)
        {
            return;
        }

        ItemInfo existingItem = playerInventory.Find(item => item.itemNumberID == id);

        ItemInfo newItem = new ItemInfo
        {
            itemID = item.itemID,
            itemNumberID = item.itemNumber,
            quantity = itemstoAdd,
            itemTag = item.itemTag,
            inventoryTag = item.inventoryTag
        };

        if (existingItem != null)
        {
            existingItem.quantity += itemstoAdd;
        }
        else
        {
            playerInventory.Add(newItem);
        }
    }
    private void RemoveFromInventory(int id, int itemstoRemove)
    {
        ItemInfo existingItem = playerInventory.Find(item => item.itemNumberID == id);

        if (existingItem != null)
        {
            existingItem.quantity -= itemstoRemove;
            if(existingItem.quantity <= 0)
            {
                playerInventory.Remove(existingItem);
            }
        }
    }

    public string GetName(string itemID, int nameType)
    {
        switch (nameType)
        {
            // name from inventory
            case 0:
                ItemInfo existingItem = playerInventory.Find(item => item.itemID == itemID);

                if (existingItem != null)
                {
                    // compare the itemID from ItemInfo to the original item array's itemID
                    foreach (var item in items)
                    {
                        if (item.itemID == existingItem.itemID)
                        {
                            return item.itemName;
                        }
                    }
                }
                else
                {
                    return null;
                }
                return null;
            // name from crafting recipe output
            case 1:
                Recipe craftingItem = FindFirstObjectByType<CraftingManager>().craftableItems.Find(item => item.outputItem.itemID == itemID);

                if (craftingItem != null)
                {
                    foreach (var item in FindFirstObjectByType<CraftingManager>().craftableItems)
                    {
                        if (item.outputItem.itemID == craftingItem.outputItem.itemID)
                        {
                            return item.outputItem.itemName;
                        }
                    }
                }
                else
                {
                    Debug.Log("No item found!");
                }
                return null;
            // name from ingredients in recipe
            case 2:
                string ingredientItem = FindFirstObjectByType<Recipe>().GetItemID(itemID);

                if (ingredientItem != null)
                {
                    string name = GetName(ingredientItem, 3);
                    return name;
                }
                else
                {
                    Debug.Log("No item found!");
                }
                return null;
            // name from item database
            case 3:
                foreach (var item in items)
                {
                    if (item.itemID == itemID)
                    {
                        return item.itemName;
                    }
                }
                return null;
            default:
                Debug.Log("Not a valid input or item!");
                return null;
        }
    }

    public string GetDescription(string itemID)
    {
        ItemInfo existingItem = playerInventory.Find(item => item.itemID == itemID);

        if (existingItem != null)
        {
            // compare the itemID from ItemInfo to the original item array's itemID
            foreach (var item in items)
            {
                if (item.itemID == existingItem.itemID)
                {
                    return item.itemDescription;
                }
            }
        }
        else
        {
            Recipe craftingItem = FindFirstObjectByType<CraftingManager>().craftableItems.Find(item => item.outputItem.itemID == itemID);
            if (craftingItem != null)
            {
                foreach (var item in items)
                {
                    if (item.itemID == craftingItem.outputItem.itemID)
                    {
                        return item.itemDescription;
                    }
                }
            }
            else
            {
                Debug.Log("No item found!");
            }

        }
        return null;
    }

    public int GetQuantity(string itemID, int type)
    {
        switch (type)
        {
            // quantity from inventory
            case 0:
                ItemInfo itemtoFind = playerInventory.Find(item => item.itemID == itemID);

                if (itemtoFind != null)
                {
                    // compare the itemID from ItemInfo to the original item array's itemID
                    foreach (var item in items)
                    {
                        if (item.itemID == itemtoFind.itemID)
                        {
                            return item.quantity;
                        }
                    }
                }
                else
                {
                    return 0;
                }
                return 0;
            // quantity from crafting recipe output
            case 1:
                Recipe craftingitemtoFind = FindFirstObjectByType<CraftingManager>().craftableItems.Find(item => item.outputItem.itemID == itemID);
                if (craftingitemtoFind != null)
                {
                    foreach (var item in items)
                    {
                        if (item.itemID == craftingitemtoFind.outputItem.itemID)
                        {
                            return item.quantity;
                        }
                    }
                }
                else
                {
                    Debug.Log("No item found!");
                }
                return 0;
            // quantity from item database
            case 2:
                foreach (var item in items)
                {
                    if (item.itemID == itemID)
                    {
                        return item.quantity;
                    }
                }
                return 0;
            default:
                Debug.Log("Not a valid input or item!");
                return 0;
        }
    }

    public int GetIngredientQuantity(string itemID, Recipe recipetoSeek)
    {
        string ingredientItem = recipetoSeek.GetItemID(itemID);

        if (ingredientItem != null)
        {
            foreach (var ingredient in recipetoSeek.ingredients)
            {
                if (ingredient.item.itemID == itemID)
                {
                    Debug.Log(ingredient.quantity);
                    return ingredient.quantity;
                }
            }
        }
        else
        {
            Debug.Log("No item found!");
            return 0;
        }
        return 0;
    }

    public Item GetItem(string itemID, int type)
    {
        switch (type)
        {
            // item from inventory
            case 0:
                ItemInfo itemtoFind = playerInventory.Find(item => item.itemID == itemID);

                if (itemtoFind != null)
                {
                    // compare the itemID from ItemInfo to the original item array's itemID
                    foreach (var item in items)
                    {
                        if (item.itemID == itemtoFind.itemID)
                        {
                            return item;
                        }
                    }
                }
                else
                {
                    return null;
                }
                return null;
            // quantity from crafting recipe output
            case 1:
                Recipe craftingitemtoFind = FindFirstObjectByType<CraftingManager>().craftableItems.Find(item => item.outputItem.itemID == itemID);
                if (craftingitemtoFind != null)
                {
                    foreach (var item in items)
                    {
                        if (item.itemID == craftingitemtoFind.outputItem.itemID)
                        {
                            return item;
                        }
                    }
                }
                else
                {
                    Debug.Log("No item found!");
                }
                return null;
            // quantity from item database
            case 2:
                foreach (var item in items)
                {
                    if (item.itemID == itemID)
                    {
                        return item;
                    }
                }
                return null;
            default:
                Debug.Log("Not a valid item!");
                return null;
        }

    }

    public bool EquipItem(ItemInfo item)
    {
        if (item.isEquipped)
        {
            return false;
        }

        if (equipLimits.ContainsKey(item.itemTag))
        {
            if (equippedCounts[item.itemTag] >= equipLimits[item.itemTag])
            {
                Debug.Log($"Cannot equip more than {equipLimits[item.itemTag]} items of type {item.itemTag}.");
                return false;
            }
        }

        item.isEquipped = true;
        equippedCounts[item.itemTag]++;
        FindFirstObjectByType<Inventory>().UpdateInventoryPage();
        return true;
    }
    public bool UnequipItem(ItemInfo item)
    {
        if (!item.isEquipped)
        {
            Debug.Log("Item is not equipped.");
            return false;
        }

        item.isEquipped = false;
        if (equippedCounts.ContainsKey(item.itemTag))
        {
            equippedCounts[item.itemTag]--;
        }
        FindFirstObjectByType<Inventory>().UpdateInventoryPage();
        return true;
    }

}
