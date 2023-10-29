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

    private BoxCollider Trigger;
    private int NumPlayerPassed;
    private void Start()
    {
        Trigger = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && isEnabled && isPlayerActionnable)
        {
            if (!isBothPlayerRequired)
            {
                TriggerEvent.Invoke();
                if (isOneUseOnly)
                    isEnabled = false;
            }
            else
            {
                NumPlayerPassed += 1;
                if (NumPlayerPassed >= 2)
                {
                    TriggerEvent.Invoke();
                    if (!isOneUseOnly)
                        NumPlayerPassed = 0;
                    else
                        isEnabled = false;
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
