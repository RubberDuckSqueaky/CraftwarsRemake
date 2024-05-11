using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float cameraRotationSpeed = 2f;

    private Vector3 oldMousePosition;

    void Start()
    {
        virtualCamera = GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<CinemachineVirtualCamera>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            oldMousePosition = Input.mousePosition;
            return;
        }


        if (Input.GetMouseButton(1))
        {

            Vector3 currentMousePosition = Input.mousePosition;

            if (currentMousePosition.x < oldMousePosition.x)
            {
                float x = virtualCamera.transform.eulerAngles.x;
                float y = virtualCamera.transform.eulerAngles.y;
                virtualCamera.transform.eulerAngles = new Vector3(x, y + cameraRotationSpeed);
            }

            if (currentMousePosition.x > oldMousePosition.x)
            {
                float x = virtualCamera.transform.eulerAngles.x;
                float y = virtualCamera.transform.eulerAngles.y;
                virtualCamera.transform.eulerAngles = new Vector3(x, y - cameraRotationSpeed);
            }

        }

    }

}
