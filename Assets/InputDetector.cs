using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputDetector : MonoBehaviour
{
    public UnityEvent launchGame;
    public GameObject textLegacy;
    Text text;

    string[] joysticks;
    int joysticksCount = 0;
    public string cameraManJoystick;
    public string actionManJoystick;

    public bool cameramanRegistered = false;
    public bool actionmanRegistered = false;
    public bool triggerOneByOne = false;
    public bool triggerB = false;

    Coroutine COTriggerBuffer;

    public JoystickManager joystickManager;

    private void Start()
    {
        joystickManager = FindObjectOfType<JoystickManager>();
        text = textLegacy.GetComponent<Text>();
    }

    void Update()
    {
        triggerOneByOne = false;
        joysticks = Input.GetJoystickNames();
        if (joysticks.Length != joysticksCount)
        {
            joysticksCount = joysticks.Length;
            Debug.Log($"Joysticks updated, Count {joysticksCount}");
        }


        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                if (!cameramanRegistered && !actionmanRegistered)
                {
                    print(key.ToString());

                    string targetString = key.ToString();
                    string compareString = key.ToString().Remove(8);
                    if (compareString.CompareTo("Joystick") == 0)
                    {
                        triggerOneByOne = true;
                        if (COTriggerBuffer == null) COTriggerBuffer = StartCoroutine(TriggerBuffer());
                        cameraManJoystick = targetString.Remove(9);
                        cameraManJoystick = cameraManJoystick.Substring(8);
                        joystickManager.UpdateCameraMan(cameraManJoystick);
                        print("Gamepad registered = " + cameraManJoystick);

                        /*                    text.text = "ACTIONMAN PRESS A BUTTON";*/
                    }
                }

                else if (cameramanRegistered && !actionmanRegistered)
                {
                    print(key.ToString());

                    string targetString = key.ToString();
                    string compareString = key.ToString().Remove(8);
                    if (compareString.CompareTo("Joystick") == 0)
                    {
                        triggerOneByOne = true;
                        if (COTriggerBuffer == null) COTriggerBuffer = StartCoroutine(TriggerBuffer());
                        actionManJoystick = targetString.Remove(9);
                        actionManJoystick = actionManJoystick.Substring(8);
                        joystickManager.UpdateActionMan(actionManJoystick);
                        print("Gamepad registered = " + actionManJoystick);

                        /*                    text.text = "ACTIONMAN PRESS A BUTTON";*/
                    }
                }
            }
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("launch game");
            launchGame.Invoke();
        }
    }

    IEnumerator TriggerBuffer()
    {
        float time = 0;
        while (time < 0.5)
        {
            time += Time.deltaTime;
            yield return null;
        }

        if (!cameramanRegistered && !actionmanRegistered)
        {
            cameramanRegistered = true;
            print("cameramanRegistered");
        }
        else if (cameramanRegistered && !actionmanRegistered)
        {
            actionmanRegistered = true;
            print("actionmanRegistered");
        }
        COTriggerBuffer = null;

/*            triggerOneByOne = false;*/
        yield return null;
    }
}