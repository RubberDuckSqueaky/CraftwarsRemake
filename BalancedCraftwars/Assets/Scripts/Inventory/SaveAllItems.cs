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

    [Serializable]
    public class ItemInfo
    {
        public string itemID;
        public int itemNumberID;
        public int quantity;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SaveToJson();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            int randomItem = UnityEngine.Random.Range(0, items.Length);

            AddItem(randomItem, 1);
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
            item.quantity = 0;

            if (item.itemNumber == id)
            {
                AddToInventory(id, itemstoAdd);
                FindFirstObjectByType<Inventory>().UpdateInventoryPage();
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
        ItemInfo existingItem = playerInventory.Find(item => item.itemNumberID == id);

        if (existingItem != null)
        {
            existingItem.quantity += itemstoAdd;
        }
        else
        {
            ItemInfo newItem = new ItemInfo
            {
                itemID = items[id].itemID,
                itemNumberID = id,
                quantity = itemstoAdd
            };
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

    public string GetName(string itemID)
    {
        ItemInfo existingItem = playerInventory.Find(item => item.itemID == itemID);

        if (existingItem != null)
        {
            // compare the itemID from ItemInfo to the original item array's itemID
            foreach (var item in items)
            {
                if(item.itemID == existingItem.itemID)
                {
                    return item.itemName;
                }
            }
        }
        else
        {
            Recipe craftingItem = FindFirstObjectByType<CraftingManager>().craftableItems.Find(item => item.outputItem.itemID == itemID);
            if(craftingItem != null)
            {
                foreach (var item in items)
                {
                    if (item.itemID == craftingItem.outputItem.itemID)
                    {
                        return item.itemName;
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

    public int GetQuantity(string itemID)
    {
        ItemInfo existingItem = playerInventory.Find(item => item.itemID == itemID);

        if (existingItem != null)
        {
            // compare the itemID from ItemInfo to the original item array's itemID
            foreach (var item in items)
            {
                if (item.itemID == existingItem.itemID)
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
    }

}
