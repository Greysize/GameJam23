using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputDetector : MonoBehaviour
{
    public UnityEvent launchGame;
    public GameObject textLegacy;
    public GameObject checkMark1;
    public GameObject checkMark2;
    Text text;

    Fader fader;

    string[] joysticks;
    int joysticksCount = 0;
    public string cameraManJoystick;
    public string actionManJoystick;

    public bool cameramanRegistered = false;
    public bool actionmanRegistered = false;
    public bool preventDouble = false;
    public bool triggerB = false;
    public float loopOnlyTwo = 0;

    Coroutine COTriggerBuffer;
    Coroutine COStartGame;

    public JoystickManager joystickManager;

    private void Start()
    {
        joystickManager = FindObjectOfType<JoystickManager>();
        text = textLegacy.GetComponent<Text>();
        fader = FindObjectOfType<Fader>();
    }

    void Update()
    {
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
                if (!cameramanRegistered && !actionmanRegistered && loopOnlyTwo < 2)
                {
                    print(key.ToString());

                    string targetString = key.ToString();
                    string compareString = key.ToString().Remove(8);
                    if (compareString.CompareTo("Joystick") == 0)
                    {
                        preventDouble = true;
                        if (COTriggerBuffer == null) COTriggerBuffer = StartCoroutine(TriggerBuffer());
                        cameraManJoystick = targetString.Remove(9);
                        cameraManJoystick = cameraManJoystick.Substring(8);
                        joystickManager.UpdateCameraMan(cameraManJoystick);
                        print("Gamepad registered = " + cameraManJoystick);
                        checkMark1.SetActive(true);
                        loopOnlyTwo++;
                        text.text = "ACTION MAN PRESS A";
                    }
                }

                else if (cameramanRegistered && !actionmanRegistered && !preventDouble)
                {
                    print(key.ToString());

                    string targetString = key.ToString();
                    string compareString = key.ToString().Remove(8);
                    if (compareString.CompareTo("Joystick") == 0)
                    {
                        actionManJoystick = targetString.Remove(9);
                        actionManJoystick = actionManJoystick.Substring(8);
                        print("cameraman = " + cameraManJoystick + "actionman = " + actionManJoystick);
                        bool o = actionManJoystick.CompareTo(cameraManJoystick) == 0;
                        print(o);
                        if (o == false && actionManJoystick.CompareTo("B") != 0)
                        {
                            preventDouble = true;
                            if (COTriggerBuffer == null) COTriggerBuffer = StartCoroutine(TriggerBuffer());
                            joystickManager.UpdateActionMan(actionManJoystick);
                            print("Gamepad registered = " + actionManJoystick);
                            checkMark2.SetActive(true);
                            text.text = "GAME WILL START";
                            FindObjectOfType<SceneManagement>().lastLoadedSceneIsActive = false;
                            StartCoroutine(GameObject.FindObjectOfType<Fader>().FadeAndLoadScene(Fader.FadeDirection.In));
                            
                        }
                    }
                }
            }
        }


/*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("launch game");
            launchGame.Invoke();
        }*/
    }

    IEnumerator TriggerBuffer()
    {
        float time = 0;
        while (time < 0.1)
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
        preventDouble = false;
        yield return null;
    }

/*    IEnumerator StartGame()
    {
        float time = 0;
        while (time < 2)
        {
            time += Time.deltaTime;
            yield return null;
        }
        launchGame.Invoke();

    }*/
}