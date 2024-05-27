using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatSystem : MonoBehaviour
{
    [Header("Player UI Settings")]
    [SerializeField] public Slider defaultHealthSlider;
    [SerializeField] public Slider sharedHealthSlider;
    [SerializeField] public TextMeshProUGUI defaultHealthText;
    [SerializeField] public TextMeshProUGUI sharedHealthText;

    [Header("Player Health Settings")]
    [SerializeField] public bool healthActive = true;
    [Min(0), SerializeField] public float playerHealth = 100;
    [SerializeField] private float playerRegenTick = 0.05f;
    [SerializeField] private int playerRegenDelay = 5;

    [Header("Other Stats")]
    [SerializeField] public float playerHealingModifier = 1.0f;
    [SerializeField] public float playerHurtingModifier = 1.0f;
    [SerializeField] public float playerDamageModifier = 1.0f;
    [SerializeField] public float playerSpeedModifier = 1.0f;

    [SerializeField] public float playerFirerateMultiplier = 1.0f;
    [SerializeField] public float playerSwingSpeedMultiplier = 1.0f;
    [SerializeField] public float playerCastingSpeedMultiplier = 1.0f;
    [SerializeField] public float playerSummonSpeedMultiplier = 1.0f;

    [SerializeField] public float playerDamageReduction = 0.0f;
    [SerializeField] public int playerSpeed = 16;
    [SerializeField] public int playerJump = 5;
    [SerializeField] public int playerArmor = 0;
    [SerializeField] public int playerLuck = 0;

    // [Header("Death Settings")]
    // [SerializeField] private Canvas deathCanvas;

    private float maxHealth;
    private PlayerShieldSystem shieldSystem;

    public float playerMoveSpeed;
    public float playerSprintSpeed;
    public float playerJumpSpeed;

    public bool isAlive = true;
    private bool regentookDamage = false;
    private bool regenActive = false;
    private float defaultMoveSpeed;
    public float defaultSprintSpeed;
    public float defaultJumpSpeed;
    private void Awake()
    {
        shieldSystem = GetComponent<PlayerShieldSystem>();

        // UI
        maxHealth = playerHealth;
        sharedHealthSlider.gameObject.SetActive(true);
        sharedHealthSlider.maxValue = maxHealth;
        sharedHealthSlider.value = playerHealth;
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
            PlayerTakeDamage(5, 0);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerTakeDamage(5, 1);
        }
        defaultHealthSlider.value = playerHealth;
        sharedHealthSlider.value = playerHealth;
        defaultHealthText.text = playerHealth.ToString() + "/" + maxHealth;
        sharedHealthText.text = playerHealth.ToString() + "/" + maxHealth;

        PlayerHealthRegen();
        HealthFix();
    }
    #region External Functionality
    #region Health Functions
    public void PlayerHeal(float health)
    {
        if (!isAlive || playerHealth == maxHealth) { return; }
        playerHealth += health;
        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
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
    #endregion
    #region Speed Functions
    public void SetSpeed(float speed)
    {
        if (!isAlive) { return; }
        playerMoveSpeed = speed;
        playerSprintSpeed = speed * 1.5f;
    }
    #endregion
    public void PlayerTakeDamage(float damage, int damageType)
    {
        if (!isAlive) { return; }

        switch(damageType)
        {
            // normal damage
            case 0:
                if (shieldSystem.playerShield > 0)
                {
                    float damagetoSubtract = shieldSystem.playerShield;
                    shieldSystem.playerShield -= damage;
                    damage -= damagetoSubtract;
                    shieldSystem.shieldRegen = false;
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
                    shieldSystem.tookDamage = true;
                    regentookDamage = true;
                }
                if (playerHealth <= 0)
                {
                    Debug.Log("died");
                }
                return;
            // shield bypassing damage
            case 1:
                playerHealth -= damage;
                regenActive = false;
                regentookDamage = true;
                if (playerHealth <= 0)
                {
                    Debug.Log("died");
                }
                return;
            default:
                Debug.Log("Not a valid type of damage!");
                return;
        }
    }
    #endregion

    #region Health & Shield Regen (Player)
    private void PlayerHealthRegen()
    {
        if (!isAlive || playerHealth >= maxHealth || !healthActive) { return; }

        StartCoroutine(Regen());
    }
    private void HealthFix()
    {
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }
    }
    private IEnumerator Regen()
    {
        if(regentookDamage)
        {
            regenActive = false;
            regentookDamage = false;
            StopAllCoroutines();
        }
        if (!regenActive)
        {
            yield return new WaitForSeconds(playerRegenDelay);
            regenActive = true;
        }

        yield return new WaitForSeconds(playerRegenTick);
        playerHealth++;
        StopAllCoroutines();
    }
    #endregion
}
