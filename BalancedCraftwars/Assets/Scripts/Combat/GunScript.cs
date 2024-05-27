using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject firepoint;

    [Header("Gun Settings")]
    [SerializeField] bool raycastEnabled = false;
    [SerializeField] private float bulletSpeed = 1.5f;
    [SerializeField] private float firerate = 0.1f;
    [SerializeField] private int bulletCount = 1;

    public List<Ability> abilities;
    public PlayerStatSystem playerStatSystem;
    public string weaponID;

    private void OnEnable()
    {
        playerStatSystem = GetComponent<PlayerStatSystem>();
        Debug.Log("I AM ALIVE");
        foreach (var ability in abilities)
        {
            if (ability.abilityName == "Shoot")
            {
                ability.cooldownDuration = firerate / playerStatSystem.playerFirerateMultiplier;
            }
        }
    }


    void Update()
    {
        foreach (var ability in abilities)
        {
            if (Input.GetKey(ability.activationKey))
            {
                if (!CooldownManager.Instance.IsOnCooldown(ability))
                {
                    ActivateAbility(ability);
                    CooldownManager.Instance.StartCooldown(ability);
                }
                else
                {
                    Debug.Log(ability.abilityName + " is on cooldown.");
                }
            }
        }
    }

    private void Shoot()
    {
        switch (raycastEnabled)
        {
            case false:
                // projectile
                Ray projectile_cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(projectile_cursorRay, out hit))
                {
                    GameObject projectile = GameObject.Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation);
                    projectile.transform.LookAt(hit.point);
                    projectile.GetComponent<Rigidbody>().velocity = bulletSpeed * projectile.transform.forward;
                }
                return;
            case true:
                // raycast
                Ray raycast_cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast_cursorRay, out raycastHit))
                {
                    GameObject projectile = GameObject.Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation);
                    projectile.transform.LookAt(raycastHit.point);
                    projectile.GetComponent<Rigidbody>().velocity = bulletSpeed * projectile.transform.forward;
                }
                return;
        }
    }
    private void LockOnShoot()
    {
        for (int x = 0; x < bulletCount; x++)
        {
            GameObject realBullet = Instantiate(bullet, firepoint.transform.position, Camera.main.transform.rotation);
            realBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * Camera.main.transform.forward;
        }
    }

    void ActivateAbility(Ability ability)
    {
        if (ability.abilityName == "Shoot")
        {
            Shoot();
        }
    }

}
