using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject firepoint;

    [Header("Gun Settings")]
    [SerializeField] bool raycastEnabled = false;
    [SerializeField] private float bulletSpeed = 1.5f;
    [SerializeField] private int bulletCount = 1;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        switch (raycastEnabled)
        {
            case false:
                // projectile
                for (int x = 0; x < bulletCount; x++)
                {
                    GameObject realBullet = Instantiate(bullet, firepoint.transform.position, Camera.main.transform.rotation);
                    realBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * Camera.main.transform.forward;
                }
                return;
            case true:
                // raycast
                Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(cursorRay, out hit))
                {
                    GameObject projectile = GameObject.Instantiate(bullet, firepoint.transform.position, firepoint.transform.rotation);
                    projectile.transform.LookAt(hit.point);
                    projectile.GetComponent<Rigidbody>().velocity = bulletSpeed * projectile.transform.forward;
                }
                return;
        }


    }

}
