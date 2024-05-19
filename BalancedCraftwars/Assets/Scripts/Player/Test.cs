using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    private const float LOW_LIMIT =  -85.0f;
    private const float HIGH_LIMIT = 85.0f;

    public GameObject theCamera;
    public float followDistance = 5.0f;
    public float mouseSensitivityX = 4.0f;
    public float mouseSensitivityY = 2.0f;
    public float heightOffset = 0.5f;
    public LayerMask obstacleLayers;

    private Vector3 desiredPosition;
    private float currentXRotation;
    private float currentYRotation;

    void Start()
    {
        desiredPosition = theCamera.transform.position;
        theCamera.transform.forward = gameObject.transform.forward;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentXRotation = theCamera.transform.eulerAngles.x;
        currentYRotation = theCamera.transform.eulerAngles.y;
    }

    void Update()
    {
        Vector2 cameraMovement = Vector2.zero;

        if (Input.GetMouseButton(1))
        {
            cameraMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        // Calculate desired camera rotation
        currentXRotation = Mathf.Clamp(currentXRotation - cameraMovement.y * mouseSensitivityY, LOW_LIMIT, HIGH_LIMIT);
        currentYRotation += cameraMovement.x * mouseSensitivityX;


        // Apply rotation to camera
        theCamera.transform.eulerAngles = new Vector3(currentXRotation, currentYRotation, 0);

        // Calculate desired position based on player's position and follow distance
        Vector3 playerPosition = transform.position + new Vector3(0, heightOffset, 0);
        desiredPosition = playerPosition - theCamera.transform.forward * followDistance;

        // Check for obstacles and adjust camera position if necessary
        RaycastHit hit;
        if (Physics.Linecast(playerPosition, desiredPosition, out hit, obstacleLayers))
        {
            // Position the camera at the point of collision, slightly offset to avoid clipping
            theCamera.transform.position = hit.point + hit.normal * 0.1f;
        }
        else
        {
            // No obstacles, position the camera at the desired position
            theCamera.transform.position = desiredPosition;
        }

        // Maintain the camera's up direction to avoid flipping
        theCamera.transform.LookAt(playerPosition);
    }

}
