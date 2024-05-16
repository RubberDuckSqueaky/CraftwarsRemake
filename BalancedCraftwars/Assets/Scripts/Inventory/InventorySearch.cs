using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySearch : MonoBehaviour
{
    public Inventory inventory;
    public TMP_InputField searchInput;

    void Start()
    {
        searchInput.onValueChanged.AddListener(OnSearchValueChanged);
    }

    void OnSearchValueChanged(string searchText)
    {
        inventory.FilterInventory(searchText);
    }
}
