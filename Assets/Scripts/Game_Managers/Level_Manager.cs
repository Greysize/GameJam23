using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level_Manager : MonoBehaviour
{
    [Header("Player Roles Name")]
    public string ActionmanName = "Actionman";
    public string CameranmanName = "Cameraman";
    [Header("Players")]
    public GameObject PlayerAction;
    public GameObject PlayerCamera;
    [Header("Camera")]
    public GameObject Camera;
    [Header("SpwanPoints")]
    public GameObject SpawnAction;
    public GameObject SpawnCamera;
    [Header("Trigger Recorder")]
    public string LastTriggerRole;
    [Header("Reset Leverl")]
    public UnityEvent ResetLevel;

    private SceneManagement SceneManager;

    public void Start()
    {
        OnLevelStart();
        SceneManager = FindObjectOfType<SceneManagement>();
    }
    public void OnLevelStart()
    {
        OnResetPlayer(ActionmanName);
        OnResetPlayer(CameranmanName);
    }

    public void OnResetPlayer()
    {
        string type = LastTriggerRole;
        if (type.CompareTo(ActionmanName) == 0)
        {
            PlayerAction.transform.position = SpawnAction.transform.position;
            PlayerAction.transform.rotation = SpawnAction.transform.rotation;
        }
        if (type.CompareTo(CameranmanName) == 0)
        {
            PlayerCamera.transform.position = SpawnCamera.transform.position;
            PlayerCamera.transform.rotation = SpawnCamera.transform.rotation;
        }
    }
    public void OnResetPlayer(string type)
    {
        if(type.CompareTo(ActionmanName) == 0)
        {
            PlayerAction.GetComponent<vThirdPersonController>().enabled = false;
            PlayerAction.GetComponent<vThirdPersonInput>().enabled = false;
            PlayerAction.GetComponent<Rigidbody>().isKinematic = true;
            PlayerAction.GetComponent<CapsuleCollider>().enabled = false;
            PlayerAction.transform.position = SpawnAction.transform.position;
            PlayerAction.transform.rotation = SpawnAction.transform.rotation;
            PlayerAction.GetComponent<CapsuleCollider>().enabled = true;
            PlayerAction.GetComponent<Rigidbody>().isKinematic = false;
            PlayerAction.GetComponent<vThirdPersonController>().enabled = true;
            PlayerAction.GetComponent<vThirdPersonInput>().enabled = true;
        }
        if (type.CompareTo(CameranmanName) == 0)
        {
            PlayerCamera.GetComponent<vThirdPersonController>().enabled = false;
            PlayerCamera.GetComponent<vThirdPersonInput>().enabled = false;
            PlayerCamera.GetComponent<Rigidbody>().isKinematic = true;
            PlayerCamera.GetComponent<CapsuleCollider>().enabled = false;
            PlayerCamera.transform.position = SpawnCamera.transform.position;
            PlayerCamera.transform.rotation = SpawnCamera.transform.rotation;
            PlayerCamera.GetComponent<CapsuleCollider>().enabled = true;
            PlayerCamera.GetComponent<Rigidbody>().isKinematic = false;
            PlayerCamera.GetComponent<vThirdPersonController>().enabled = true;
            PlayerCamera.GetComponent<vThirdPersonInput>().enabled = true;

        }
    }
    public void OnResetLevel()
    {
        Destroy(Camera);
        Destroy(PlayerAction);
        Destroy(PlayerCamera);
        ResetLevel.Invoke();
    }
}
