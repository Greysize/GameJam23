using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputUserManager : MonoBehaviour
{
    Dictionary<InputDevice, InputUser> deviceToUser;
    Dictionary<InputUser, InputActionAsset> userToControls;
    List<InputDevice> inputDevices;

    private void Awake()
    {
        inputDevices = new List<InputDevice>();
        deviceToUser = new Dictionary<InputDevice, InputUser>();
        userToControls = new Dictionary<InputUser, InputActionAsset>();
    }

    public InputActionAsset GetControlsFromUser(InputUser u) => userToControls[u];
    public bool DeviceUsed(InputDevice device) => inputDevices.Contains(device);
    // UserInputData is a class that contains important information regarding an InputUser
    public UserInputData GetInputData(InputUser user)
    {
        InputActionAsset controls = GetControlsFromUser(user);
        string controlScheme = user.controlScheme.Value.name;
        return new UserInputData(controlScheme, user.pairedDevices.ToArray(), controls, user);
    }

    public void DisconnectDevices(InputDevice[] devices)
    {
        InputUser user = deviceToUser[devices[0]];

        foreach (InputDevice device in devices)
        {
            userToControls.Remove(user);
            deviceToUser.Remove(device);
            inputDevices.Remove(device);
        }

        user.UnpairDevicesAndRemoveUser();
    }

    public InputUser CreateInputUser(InputDevice device)
    {
        List<InputDevice> devices = new List<InputDevice>();

        string controlScheme = "Gamepad";

        if (device is Mouse || device is Keyboard)
        {
            controlScheme = "Keyboard&Mouse";

            devices.Add(Keyboard.current);
            devices.Add(Mouse.current);
            devices.Add(Gamepad.current);
        }
        else
            devices.Add(device);

        InputUser user = InputUser.PerformPairingWithDevice(devices[0]);

        foreach (InputDevice dev in devices)
        {
            if (dev != devices[0])
                InputUser.PerformPairingWithDevice(dev, user: user);

            inputDevices.Add(dev);
            deviceToUser.Add(dev, user);
        }

        InputActionAsset userControls = new InputActionAsset();

        user.AssociateActionsWithUser(userControls);
        user.ActivateControlScheme(controlScheme);

        userControls.Enable();
        userToControls.Add(user, userControls);

        return user;
    }
}