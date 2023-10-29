using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerZone : MonoBehaviour
{
    public bool isEnabled = true;
    public bool isPlayerActionnable = true;
    public bool isBlockActionnable = false;
    public bool isBothPlayerRequired = false;
    public bool isOneUseOnly = false;
    public UnityEvent TriggerEvent;
    private int NumPlayerPassed = 0;
    private Level_Manager LvlMan;

    private void Start()
    {
        LvlMan = FindObjectOfType<Level_Manager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && isEnabled && isPlayerActionnable)
        {
            if (other.GetComponent<vThirdPersonInput>() != null)
            {
                if (!isBothPlayerRequired)
                {
                    LvlMan.LastTriggerRole = other.GetComponent<vThirdPersonInput>().role;
                    TriggerEvent.Invoke();
                    if (isOneUseOnly)
                        isEnabled = false;
                }
                else
                {
                    NumPlayerPassed += 1;
                    if (NumPlayerPassed >= 2)
                    {
                        LvlMan.LastTriggerRole = other.GetComponent<vThirdPersonInput>().role;
                        TriggerEvent.Invoke();
                        if (!isOneUseOnly)
                            NumPlayerPassed = 0;
                        else
                            isEnabled = false;
                    }
                }
            }
        }
        if(other.tag == "Block" && isEnabled && isBlockActionnable)
        {
            TriggerEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isEnabled && isPlayerActionnable)
        {
            if (isBothPlayerRequired)
            {
                NumPlayerPassed -= 1;
                if (NumPlayerPassed <= 0)
                {
                    NumPlayerPassed = 0;
                }
            }
        }
    }

}
