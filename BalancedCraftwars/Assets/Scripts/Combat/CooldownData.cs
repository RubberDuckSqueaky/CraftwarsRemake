using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CooldownData
{
    public string weaponID;
    public bool unmodifiable = false;
    public float defaultCooldown;
    public float zCooldown;
    public float xCooldown;
    public float cCooldown;
    public float vCooldown;

    public bool defaultActive;
    public bool zActive;
    public bool xActive;
    public bool cActive;
    public bool vActive;
}
