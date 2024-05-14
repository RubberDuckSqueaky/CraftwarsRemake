using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    private SaveAllItems items;

    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;

    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    private void Start()
    {
        items = GetComponent<SaveAllItems>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UpdateInventory();
        }
    }

    private void UpdateInventory()
    {

    }




}