using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    public static JoystickManager singleton = null;
    public string cameraManJoystick;
    public string actionManJoystick;

    public void UpdateActionMan(string n)
    {
        actionManJoystick = n;
    }

    public void UpdateCameraMan(string n)
    {
        cameraManJoystick = n;
    }
}
