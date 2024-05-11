using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class lookatplayer : MonoBehaviour
{
    public GameObject player;

    private void Awake()
    {
        player = FindFirstObjectByType<ThirdPersonController>().gameObject;
    }
    void Update()
    {
        transform.LookAt(player.transform);
    }
}
