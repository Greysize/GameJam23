using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    JoystickManager joystickManager;
    public Transform InteractorOrigin;
    public bool canInteract = true;
    public float Raydistance = 2f;
    [SerializeField] string InteractButton = "Fire";
    [SerializeField] vThirdPersonInput myController;

   // private Level_Manager LvlMan;
   // private Camera Cam;
    private bool isTestNeeded = false;

    private void Start()
    {
        joystickManager = FindObjectOfType<JoystickManager>();
        var joynum = joystickManager.actionManJoystick;
        print("INITIALIZE INTERACTOR");
        InteractButton = "Fire" + joynum;
        print("joynum = " + joynum);

       // LvlMan = FindObjectOfType<Level_Manager>();
       // Cam = LvlMan.Camera.GetComponent<Camera>();
    }
    private void Update()
    {
        if (Input.GetButtonDown(InteractButton))
            InteractionTest();

        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Debug joystick");
            InteractButton = "Fire" + myController.joystickNumber;
        }

    }

    private void InteractionTest()
    {
        Ray rayInteract = new Ray(InteractorOrigin.transform.position, InteractorOrigin.transform.forward);
        RaycastHit hitData;
        Debug.DrawRay(InteractorOrigin.transform.position, InteractorOrigin.transform.forward, Color.red);

        if (Physics.Raycast(rayInteract, out hitData, Raydistance))
        {
            if(hitData.collider.gameObject.GetComponent<Interactable>())
            {
                hitData.collider.gameObject.GetComponent<Interactable>().OnInteract();
            }
        }
    }
}
