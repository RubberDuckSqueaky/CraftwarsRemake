using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatSystem : MonoBehaviour
{
    [Header("Player UI Settings")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider shieldSlider;
    [SerializeField] public bool healthActive = true;
    [SerializeField] public bool shieldActive = false;

    [Header("Player Health Settings")]
    [Min(0), SerializeField] public float playerHealth = 100;
    [SerializeField] private float playerRegenTick = 0.05f;
    [SerializeField] private int playerRegenDelay = 5;

    [Header("Player Shield Settings")]
    [Min(0), SerializeField] public float playerShield = 100;
    [SerializeField] private float playerShieldRegenTick = 0.05f;
    [SerializeField] private int playerShieldRegenDelay = 5;

    // [Header("Death Settings")]
    // [SerializeField] private Canvas deathCanvas;

    private float maxHealth;
    private float maxShield;

    public float playerMoveSpeed;
    public float playerSprintSpeed;
    public float playerJumpSpeed;

    public bool isAlive = true;
    private bool shieldRegen = false;
    private bool regenActive = false;
    private float defaultMoveSpeed;
    public float defaultSprintSpeed;
    public float defaultJumpSpeed;
    private void Awake()
    {
        // UI
        maxHealth = playerHealth;
        maxShield = playerShield;

        if (maxShield <= 0 || !shieldActive)
        {
            shieldSlider.gameObject.SetActive(false);
        }
        else if (maxShield > 0 || shieldActive)
        {
            shieldSlider.gameObject.SetActive(true);
            shieldSlider.maxValue = maxShield;
            shieldSlider.value = playerShield;
        }

        if (maxHealth <= 0 || !healthActive)
        {
            healthSlider.gameObject.SetActive(false);
        }
        else if (maxHealth > 0 || healthActive)
        {
            healthSlider.gameObject.SetActive(true);
            healthSlider.maxValue = maxShield;
            healthSlider.value = playerShield;
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = playerHealth;
        // UI end

        // stat values
        defaultMoveSpeed = transform.parent.parent.GetComponent<ThirdPersonController>().MoveSpeed;
        defaultSprintSpeed = transform.parent.parent.GetComponent<ThirdPersonController>().SprintSpeed;
        defaultJumpSpeed = 1.2f;

        transform.parent.parent.GetComponent<ThirdPersonController>().MoveSpeed = defaultMoveSpeed;
        transform.parent.parent.GetComponent<ThirdPersonController>().SprintSpeed = defaultSprintSpeed;
        transform.parent.parent.GetComponent<ThirdPersonController>().JumpHeight = defaultJumpSpeed;
        // stats end

        isAlive = true;
        Cursor.lockState = CursorLockMode.Locked;
        // deathCanvas.enabled = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            PlayerTakeDamage(5);
        }
        healthSlider.value = playerHealth;

        if (playerShield <= 0)
        {
            shieldSlider.gameObject.SetActive(false);
        }
        else if (maxShield > 0 || shieldActive)
        {
            shieldSlider.gameObject.SetActive(true);
            shieldSlider.maxValue = maxShield;
            shieldSlider.value = playerShield;
        }

        PlayerHealthRegen();
        PlayerShieldRegen();
        HealthFix();
    }
    #region External Functionality
    #region Health & Shield Functions
    public void PlayerHeal(float health)
    {
        if (!isAlive || playerHealth == maxHealth) { return; }
        playerHealth += health;
        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }
    }
    public void ShieldHeal(float health)
    {
        if (!isAlive || playerShield == maxShield) { return; }
        playerShield += health;
        if (playerShield > maxShield)
        {
            playerShield = maxShield;
        }
    }
    public void SetMaxHealth(float newMaxHealth)
    {
        if (!isAlive || !healthActive) { return; }
        maxHealth = newMaxHealth;
        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }
    }
    public void SetMaxShield(float newMaxShield)
    {
        if (!isAlive || !shieldActive) { return; }
        maxShield = newMaxShield;
        if (playerShield > maxShield)
        {
            playerShield = maxShield;
        }
    }
    #endregion
    #region Speed Functions
    public void SetSpeed(float speed)
    {
        if (!isAlive) { return; }
        playerMoveSpeed = speed;
        playerSprintSpeed = speed * 1.5f;
    }
    #endregion
    public void PlayerTakeDamage(float damage)
    {
        if (!isAlive) { return; }
        if (playerShield > 0)
        {
            float damagetoSubtract = playerShield;
            playerShield -= damage;
            damage -= damagetoSubtract;
            shieldRegen = false;
            if (damage > 0)
            {
                playerHealth -= damage;
                regenActive = false;
            }
        }
        else
        {
            playerHealth -= damage;
            regenActive = false;
        }
        if (playerHealth <= 0)
        {
            Debug.Log("died");
        }
    }
    #endregion

    #region Health & Shield Regen (Player)
    private void PlayerHealthRegen()
    {
        if (!isAlive || playerHealth >= maxHealth || !healthActive) { return; }

        StartCoroutine(Regen());
    }
    private void PlayerShieldRegen()
    {
        if (!isAlive || playerShield >= maxShield || !shieldActive) { return; }

        StartCoroutine(ShieldRegen());
    }
    private void HealthFix()
    {
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
        if (playerShield < 0)
        {
            playerShield = 0;
        }
        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }
    }
    private IEnumerator Regen()
    {
        if (!regenActive)
        {
            yield return new WaitForSeconds(playerRegenDelay);
            regenActive = true;
        }

        yield return new WaitForSeconds(playerRegenTick);
        playerHealth++;
        StopAllCoroutines();
    }
    private IEnumerator ShieldRegen()
    {
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
}
