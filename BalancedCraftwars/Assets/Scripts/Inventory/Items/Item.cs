using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// None - Takes up no special slot excluding a hotbar slot when equipped. Used for in-world crafting.
// Special - Also takes up no slots besides a hotbar slot when equipped. Mostly for items that fall under "bragging rights" or special codes.
// Weapon - Takes up one weapon slot when equipped, max of 3 weapons can be equipped at one time. For things/items created to do damage.
// Equipment - Takes up one equipment slot when equipped, max of 2 equipment can be equipped at one time. For utility or generally useful items.
// Helmet, Armor - Takes up one helmet/armor slot when equipped, a max of one helmet and armor can be equipped at one time. For things u can wear to protect.
// Neck - For amulets and unique neck accessories.

public enum SlotTag { None, Special, Weapon, Equipment, Helmet, Armor, Neck, Shield, Back, Accessory }

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public string itemID = "testID";
    public string itemName = "DefaultItem";
    public string itemDescription = "DefaultDescription";
    public bool stackable = true;
    public int itemNumber = 0;
    public int quantity = 0;
    public SlotTag itemTag;

    [Header("Player Health Modifiers")]
    public float healthIncrease = 0;
    public float healthMultiplier = 1;

    [Header("Player Defense Modifiers")]
    public float defenseIncrease = 0;
    public float defenseMultiplier = 1;
    public float damageReduction = 0;
    public float damageIntakeMultiplier = 1;

    [Header("Player Damage Modifiers")]
    public float damageIncrease = 0;
    public float damageMultiplier = 1;

    [Header("Player Speed Modifiers")]
    public float speedIncrease = 0;
    public float speedMultiplier = 1;

    [Header("Player Currency Modifiers")]
    public float currencyToAdd = 0;
    public float currencyMultiplier = 1;

    [Header("Equip Settings")]
    public GameObject equipmentPrefab;

}
