using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class UserInputData
{
    public readonly string ControlScheme; //InputControlScheme
    public readonly InputDevice[] Devices;
    public readonly InputActionAsset Controls;
    public readonly InputUser InputUser;
    public readonly bool Keyboard;

    public UserInputData(string controlScheme, InputDevice[] devices, InputActionAsset controls, InputUser inputUser)
    {
        Keyboard = controlScheme == "Keyboard&Mouse";
        ControlScheme = controlScheme;
        Devices = devices;
        Controls = controls;
        InputUser = inputUser;
    }
}

