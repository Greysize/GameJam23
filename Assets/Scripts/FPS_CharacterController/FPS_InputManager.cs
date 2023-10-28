using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS_Controller
{
    public class FPS_InputManager : MonoBehaviour
    {
#pragma warning disable 649
        [Header("Controller functions")]
        public bool isInputActive = true;
        public bool CanMove = true;
        public bool CanLook = true;
        public bool CanNarrowLook = false;
        public bool CanPause = true;
        public bool CanJump = true;
        public bool CanInteract = true;
        public bool CanInteractUI = true;
        public bool CanInteractDistance = true; // FBI only
        public bool CanInteractDiapo = true; // FBI Only
        public bool CanCancel = false;
        public bool ForceCameraAngle = false;
        [Header("Modules")]

        public FPS_Movements movements;
        public FPS_MouseLook mouseLook;
        public FPS_Interact interact;
    

        [Header("Controls")]
        public bool isInputGamepad;
        public bool isKinematic;
        public bool isPosed;
        // Private
        InputActionsManager controls;
        InputActionsManager.Movement_DesktopActions MovementsActions;
        Vector2 MovementInput;
        Vector2 MovementKeyboardInput;
        Vector2 MouseInput;
        private string lastInput = "";
        InputDevice lastDevice = default;
        //private FPS_FocusInputManager FocusInputMan;
        private void Awake()
        {
            // Init the required components
            movements = gameObject.GetComponent<FPS_Movements>();
            mouseLook = gameObject.GetComponent<FPS_MouseLook>();
            interact = gameObject.GetComponent<FPS_Interact>();
            //FocusInputMan = gameObject.GetComponent<FPS_FocusInputManager>();
            // Debug
            if (movements == null)
                Debug.Log(" ERR# : No Movement Component found on " + gameObject.name + " !");
            if (mouseLook == null)
                Debug.Log(" ERR# : No Mouse Look Component found on " + gameObject.name + " !");
            if (interact == null)
                Debug.Log(" ERR# : No Interact at Reach Component found on " + gameObject.name + " !");
            // Int the Input System
            controls = new InputActionsManager();
            MovementsActions = controls.Movement_Desktop;

            // Init the input listeners
            CreateControlListener();
            UpdateControls();
        }



        private void Update()
        {
            if (isInputActive)
            {
                DetectInputType();
                if (!isKinematic)
                {
                    mouseLook.ReceiveInput(MouseInput, isInputGamepad);
                    if (!isPosed)
                    {
                        if (MovementInput.x != 0 || MovementInput.y != 0)
                            movements.ReceiveInput(MovementInput);
                        else
                            movements.ReceiveInput(MovementKeyboardInput);
                    }
                    else
                        movements.ReceiveInput(Vector2.zero);
                }
                else
                {
                    mouseLook.ReceiveInput(Vector2.zero, isInputGamepad);
                    movements.ReceiveInput(Vector2.zero);
                }
            }
        }

        public void CreateControlListener()
        {
            // Movements
            MovementsActions.Movements.performed += ctx => MovementKeyboardInput = ctx.ReadValue<Vector2>();
            // fix
            MovementsActions.MovementsX.performed += ctx => MovementInput.x = ctx.ReadValue<float>();
            MovementsActions.MovementsY.performed += ctx => MovementInput.y = ctx.ReadValue<float>();
            // Mouse Look
            MovementsActions.MouseX.performed += ctx => MouseInput.x = ctx.ReadValue<float>();
            MovementsActions.MouseY.performed += ctx => MouseInput.y = ctx.ReadValue<float>();
            // Jump]
            MovementsActions.Jump.performed += _ => movements.OnJumpPressed();
            // Interact
            MovementsActions.Interact.performed += _ => interact.OnInteractPressed();
            // Interact Diapo Controls
            MovementsActions.DiapoForward.performed += _ => interact.OnInteractPressed(true, true);
            MovementsActions.DiapoBackward.performed += _ => interact.OnInteractPressed(true, false);
            // Cancel
            MovementsActions.Cancel.performed += _ => interact.OnCancelPressed();
            // device control detection
            MovementsActions.Get().actionTriggered += ctx => lastDevice = ctx.control?.device;
            // rebinding
        }

        public void UpdateControls()
        {
            mouseLook.CanLook = CanLook;
            mouseLook.CanNarrowLook = CanNarrowLook;
            movements.CanMove = CanMove;
            movements.CanJump = CanJump;
            interact.CanInteract = CanInteract;
            interact.CanInteractUI = CanInteractUI;
            interact.CanInteractDiapo = CanInteractDiapo;
            interact.ChangeInteractDistance(CanInteractDistance);
            interact.CanCancel = CanCancel;
        }

        public void ChangeControls(bool M, bool L, bool J, bool NL, bool I, bool C, bool D, bool ID)
        {
            CanLook = L;
            CanMove = M;
            CanJump = J;
            CanInteract = I;
            CanCancel = C;
            CanNarrowLook = NL;
            CanInteractDiapo = D;
            CanInteractDistance = ID;
            UpdateControls();
        }

        private void DetectInputType()
        {
            if (lastDevice != null)
            {
                if (lastDevice.ToString() != lastInput)
                {
                    lastInput = lastDevice.ToString();
                    if (lastInput == "XInputControllerWindows:/XInputControllerWindows")
                        isInputGamepad = true;
                    else
                        isInputGamepad = false;
                }
            }
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        public void TeleportAtLocation(Vector3 position, Quaternion rotation)
        {

            // Disable the Unity Character controller before to move it
            gameObject.GetComponent<CharacterController>().enabled = false;
            // position
            gameObject.transform.position = position;
            //rotation body (horizontal)
            gameObject.transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
            //rotation camera(vertical)
            mouseLook.playerCamera.transform.eulerAngles = new Vector3(rotation.eulerAngles.x, 0f, 0f);
            // offset needed ?
            //PlayerLook.Offset = Camera.transform.eulerAngles;
            // Enable back the Unity Character controller
            gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }
}