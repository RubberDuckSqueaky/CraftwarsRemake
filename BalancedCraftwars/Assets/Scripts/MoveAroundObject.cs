using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using static UnityEditor.PlayerSettings;

public class MoveAroundObject : MonoBehaviour
{
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                Vector2 mousePosition = CursorControl.GetPosition();

                CursorControl.SetPosition(mousePosition);
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(1))
            {
                Vector2 mousePosition = CursorControl.GetPosition();

                CursorControl.SetPosition(mousePosition);
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }
}