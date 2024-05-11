using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "DefaultItem";
    public string itemDescription = "DefaultDescription";

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

}
