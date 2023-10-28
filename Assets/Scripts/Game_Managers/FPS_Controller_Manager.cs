using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        OnCreatePlayers();
    }

    public void OnCreatePlayers()
    {
        if (PlayerPrefab != null)
        {
            PlayerA = Instantiate(PlayerPrefab);
            PlayerAController = PlayerA.GetComponent<FPS_Manager>();

            PlayerB = Instantiate(PlayerPrefab);
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
