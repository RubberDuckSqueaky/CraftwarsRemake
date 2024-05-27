using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShieldSystem : MonoBehaviour
{
    [SerializeField] Slider shieldSlider;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] public bool shieldDisabled = false;

    [Header("Player Shield Settings")]
    [Min(0), SerializeField] public float playerShield = 100;
    [SerializeField] private float playerShieldRegenTick = 0.05f;
    [SerializeField] private int playerShieldRegenDelay = 5;

    private float maxShield;
    public bool tookDamage = false;
    public bool shieldRegen = false;
    private PlayerStatSystem playerStats;

    private void Awake()
    {
        maxShield = playerShield;
        shieldSlider.maxValue = maxShield;
        shieldSlider.value = playerShield;
        shieldText.text = playerShield.ToString() + "/" + maxShield;
    }

    private void Update()
    {
        if(shieldDisabled)
        {
            return;
        }
        shieldText.text = playerShield.ToString() + "/" + maxShield;
        shieldSlider.value = playerShield;

        PlayerShieldRegen();
        ShieldFix();
    }

    #region Shield Regen
    public void PlayerShieldRegen()
    {
        if (playerShield >= maxShield || shieldDisabled) { return; }

        StartCoroutine(ShieldRegen());
    }
    private IEnumerator ShieldRegen()
    {
        if(tookDamage)
        {
            shieldRegen = false;
            tookDamage = false;
            StopAllCoroutines();
        }
        if (!shieldRegen)
        {
            yield return new WaitForSeconds(playerShieldRegenDelay);
            shieldRegen = true;
        }

        yield return new WaitForSeconds(playerShieldRegenTick);
        playerShield++;
        StopAllCoroutines();
    }
    #endregion

    #region External Functionality
    public void SetMaxShield(float newMaxShield)
    {
        maxShield = newMaxShield;
        if (playerShield > maxShield)
        {
            playerShield = maxShield;
        }
    }
    public void ShieldHeal(float health)
    {
        if (playerShield == maxShield) { return; }
        playerShield += health;
        if (playerShield > maxShield)
        {
            playerShield = maxShield;
        }
    }
    #endregion

    #region Misc
    public void ShieldFix()
    {
        if(playerShield > maxShield)
        {
            playerShield = maxShield;
        }
        if (playerShield < 0)
        {
            playerShield = 0;
        }
    }
    #endregion

}
