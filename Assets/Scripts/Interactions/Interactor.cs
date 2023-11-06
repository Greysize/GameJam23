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
    [SerializeField] AudioSource buttonSound;

   // private Level_Manager LvlMan;
   // private Camera Cam;
    private bool isTestNeeded = false;

    private void Start()
    {
        string joynum = null;
        joystickManager = FindObjectOfType<JoystickManager>();
        if (joystickManager != null) joynum = joystickManager.actionManJoystick;
        else joynum = "1";

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
                buttonSound.Play();
                hitData.collider.gameObject.GetComponent<Interactable>().OnInteract();
            }
        }
    }
}
