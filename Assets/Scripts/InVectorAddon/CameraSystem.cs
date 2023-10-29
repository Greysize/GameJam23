using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector;

public class CameraSystem : MonoBehaviour
{
    public GameObject CamMaster;
    public Vector2 MouseSensitivity;
    [Header("Camera Input")]
    public string rotateCameraXInput = "Mouse X";
    public string rotateCameraYInput = "Mouse Y";
    [HideInInspector]
    public Vector2 movementSpeed;
    [Header("Camera Limits")]
    public Vector2 xLimit = new Vector2(-360f, 360f);
    public Vector2 yLimit = new Vector2( - 40f, 80f);

    private float mouseY = 0f;
    private float mouseX = 0f;


    private void Update()
    {
        CameraInput();
    }
    private void CameraInput()
    {
        var Y = Input.GetAxis(rotateCameraYInput);
        var X = Input.GetAxis(rotateCameraXInput);
        print("Input is " + X + " and " + Y);
        RotateCamera(new Vector2(X,Y));
    }

    private void RotateCamera( Vector2 rotation)
    {
        Vector3 targetRotation = CamMaster.transform.eulerAngles;
        // free rotation 
        mouseX = rotation.y * MouseSensitivity.y;
        mouseY = -rotation.x * MouseSensitivity.x;


        //movementSpeed.x = rotation.x;
        //movementSpeed.y = -rotation.y;

        //mouseY = vExtensions.ClampAngle(mouseY, yLimit.x, yLimit.x);
        //mouseX = vExtensions.ClampAngle(mouseX, xLimit.y, xLimit.y);
        targetRotation.y += mouseY;
        targetRotation.x += mouseX;
        print("Rotating" + targetRotation);
        //A pply
        CamMaster.transform.Rotate(Vector3.up, targetRotation.y * Time.deltaTime);
        CamMaster.transform.eulerAngles = targetRotation;
    }
}
