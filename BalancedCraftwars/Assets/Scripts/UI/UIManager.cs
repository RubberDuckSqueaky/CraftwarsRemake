using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Canvas craftingCanvas;
    [SerializeField] public Canvas inventoryCanvas;
    [SerializeField] public Canvas settingsCanvas;
    private CraftingMenu craftMenu;
    private Inventory inventoryMenu;

    public void Start()
    {
        craftMenu = FindFirstObjectByType<CraftingMenu>();
        inventoryMenu = FindFirstObjectByType<Inventory>();
    }

    public void ToggleCrafting()
    {
        if(craftingCanvas.enabled)
        {
            craftingCanvas.enabled = false;
        }
        else
        {
            craftingCanvas.enabled = true;
            inventoryCanvas.enabled = false;
            settingsCanvas.enabled = false;
            craftMenu.UpdateCraftingInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryCanvas.enabled)
        {
            inventoryCanvas.enabled = false;
        }
        else
        {
            inventoryCanvas.enabled = true;
            craftingCanvas.enabled = false;
            settingsCanvas.enabled = false;
            inventoryMenu.UpdateInventory();
        }
    }


}
