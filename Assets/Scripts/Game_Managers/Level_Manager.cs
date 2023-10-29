using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    [Header("Players")]
    public GameObject PlayerAction;
    public GameObject PlayerCamera;
    [Header("SpwanPoints")]
    public GameObject SpawnAction;
    public GameObject SpawnCamera;

    public void OnLevelStart()
    {

    }
    public void OnResetPlayer(int type)
    {
        if(type == 0)
        {
            PlayerAction.transform.position = SpawnAction.transform.position;
            PlayerAction.transform.rotation = SpawnAction.transform.rotation;
        }
        if (type == 1)
        {
            PlayerCamera.transform.position = SpawnCamera.transform.position;
            PlayerCamera.transform.rotation = SpawnCamera.transform.rotation;
        }
    }
    public void OnLevelEnd()
    {

    }
}
