using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static SaveAllItems;
using static UnityEditor.Progress;

public class CooldownManager : MonoBehaviour
{
    private static CooldownManager _instance;

    public static CooldownManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CooldownManager").AddComponent<CooldownManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();

    public bool IsOnCooldown(Ability ability)
    {
        return cooldownTimers.ContainsKey(ability) && cooldownTimers[ability] > Time.time;
    }

    public void StartCooldown(Ability ability)
    {
        if (cooldownTimers.ContainsKey(ability))
        {
            cooldownTimers[ability] = Time.time + ability.cooldownDuration;
        }
        else
        {
            cooldownTimers.Add(ability, Time.time + ability.cooldownDuration);
        }
    }

    public float GetCooldownRemaining(Ability ability)
    {
        if (cooldownTimers.ContainsKey(ability))
        {
            return Mathf.Max(0, cooldownTimers[ability] - Time.time);
        }
        return 0;
    }
}
