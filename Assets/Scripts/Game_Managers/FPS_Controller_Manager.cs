using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller_Manager : MonoBehaviour
{
    [Header("SYSTEM")]
    public GameObject PlayerPrefab;
    public int LeaderCameraIndex;
    public GameObject PlayerA;
    public GameObject PlayerB;
    [Header("SPAWNING")]
    public GameObject PlayerAPoint;
    public GameObject PlayerBPoint;

    private FPS_Manager PlayerAController;
    private FPS_Manager PlayerBController;
    private PlayerInputManager InputMan;




    // Start is called before the first frame update
    void Start()
    {
        InputMan = gameObject.GetComponent<PlayerInputManager>();
        DetectPlayersConnected();
    }

    public void DetectPlayersConnected()
    {
        if(InputSystem.devices.Count >=2)
        {
            OnCreatePlayers();
        }
        else
        {
            Debug.LogError("Only" + InputSystem.devices.Count + "player foumd : ");
        }
    }


    public void OnCreatePlayers()
    {
        if (PlayerPrefab != null)
        {
            
            //PlayerA = Instantiate(PlayerPrefab, PlayerAPoint.transform.position, PlayerAPoint.transform.rotation);

            PlayerA = PlayerInput.Instantiate(PlayerPrefab, controlScheme: "GamePad", pairWithDevice: Gamepad.all[0]).gameObject;
            PlayerA.transform.position = PlayerAPoint.transform.position;
            PlayerA.transform.rotation = PlayerAPoint.transform.rotation;
            PlayerAController = PlayerA.GetComponent<FPS_Manager>();
            //PlayerB = Instantiate(PlayerPrefab, PlayerBPoint.transform.position, PlayerBPoint.transform.rotation);

            PlayerB = PlayerInput.Instantiate(PlayerPrefab, controlScheme: "Keyboard", pairWithDevice: Gamepad.all[1]).gameObject;
            PlayerB.transform.position = PlayerBPoint.transform.position;
            PlayerB.transform.rotation = PlayerBPoint.transform.rotation;
            PlayerBController = PlayerB.GetComponent<FPS_Manager>();
            UpdatePlayerRole(LeaderCameraIndex);
        }
        else
        {
            Debug.LogError("No player prefab given");
        }
    }

    public void UpdatePlayerRole(int index)
    {
        // Player setup
        if (LeaderCameraIndex == 0)
        {
            PlayerAController.isPlayerCamera = true;
            PlayerBController.isPlayerCamera = false;
            PlayerAController.PlayerCamera.SetActive(true);
            PlayerBController.PlayerCamera.SetActive(false);
        }
        if (LeaderCameraIndex == 1)
        {
            PlayerAController.isPlayerCamera = false;
            PlayerBController.isPlayerCamera = true;
            PlayerAController.PlayerCamera.SetActive(false);
            PlayerBController.PlayerCamera.SetActive(true);
        }
    }
}
