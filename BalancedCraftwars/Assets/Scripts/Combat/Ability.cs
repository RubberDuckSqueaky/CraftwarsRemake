using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public KeyCode activationKey;
    public float cooldownDuration;
}