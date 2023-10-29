using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputDetector : MonoBehaviour
{
    public UnityEvent launchGame;

    string[] joysticks;
    int joysticksCount = 0;
    public string cameraManJoystick;
    public string actionManJoystick;

    public bool cameramanRegistered = false;
    public bool actionmanRegistered = false;
    bool triggerOnce = false;

    public JoystickManager joystickManager;

    private void Start()
    {
        joystickManager = FindObjectOfType<JoystickManager>();
    }

    void Update()
    {
        triggerOnce = false;
        joysticks = Input.GetJoystickNames();
        if (joysticks.Length != joysticksCount)
        {
            joysticksCount = joysticks.Length;
            Debug.LogError($"Joysticks updated, Count {joysticksCount}");
        }



        if (!cameramanRegistered && !actionmanRegistered && !triggerOnce)
        {
            triggerOnce = true;
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    string targetString = key.ToString();
                    string compareString = key.ToString().Remove(8);
                    if (compareString.CompareTo("Joystick") == 0)
                    {
                        cameramanRegistered = true;
                        cameraManJoystick = targetString.Remove(9);
                        cameraManJoystick = cameraManJoystick.Substring(8);
                        joystickManager.UpdateCameraMan(cameraManJoystick);
                        Debug.LogError($"Gamepad number {cameraManJoystick} Pressed");
                    }
                }
            }
        }

        if (cameramanRegistered && !actionmanRegistered && !triggerOnce)
        {
            triggerOnce = true;
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    string targetString = key.ToString();
                    string compareString = key.ToString().Remove(8);
                    if (compareString.CompareTo("Joystick") == 0)
                    {
                        actionmanRegistered = true;
                        actionManJoystick = targetString.Remove(9);
                        actionManJoystick = actionManJoystick.Substring(8);
                        if (actionManJoystick != cameraManJoystick)
                        {
                            joystickManager.UpdateActionMan(actionManJoystick);
                            Debug.LogError($"Gamepad number {actionManJoystick} Pressed");
                        }

                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            print("launch game");
            launchGame.Invoke();
        }
    }
}