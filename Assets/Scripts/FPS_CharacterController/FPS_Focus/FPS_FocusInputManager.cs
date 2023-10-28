using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace FPS_Controller
{
    public class FPS_FocusInputManager : MonoBehaviour
    {
        [Header("Controller Focus functions")]
        public bool CanLook = true;
        public bool CanInteract = true;
        public bool CanAxisInteract = true;
        public bool CanCancel = false;
        [Header("Camera Settings")]
        public float FocusActivationTime = 1f;
        public float CameraFOV;
        public GameObject CameraItems;
        [Header("Focus Item")]
        public Transform FocusTargetTransform;
        public Vector3 CameraRotationFocusTarget;
        public GameObject CurrentFocusTarget;
        [Header("Modules")]
        public FPS_FocusInteract FocusInteract;
        public FPS_FocusMouse FocusMouseLook;
        public FPS_FocusAxisInteract FocusAxisInteract;
        public FPS_Manager FPS_Man;

        [Header("Controls")]
        public bool isFocusActive = false;
        public bool isInputGamepad;
        public bool isKinematic = false;
        // Private
        InputMap controls;
        InputMap.FocusActions FocusActions;
        Vector2 InteractAxisInput;
        Vector2 MouseInput;
        private string lastInput = "";
        InputDevice lastDevice = default;
        private GameObject playerCamera;
        private float O_FOV;
        private ChangePlayerSystems SysSwap;


        private void Awake()
        {
            // Init the required components
            FocusMouseLook = gameObject.GetComponent<FPS_FocusMouse>();
            FocusInteract = gameObject.GetComponent<FPS_FocusInteract>();
            FocusAxisInteract = gameObject.GetComponent<FPS_FocusAxisInteract>();
            FPS_Man = gameObject.GetComponent<FPS_Manager>();
            // Debug
            if (FocusMouseLook == null)
                Debug.Log(" ERR# : No Focus Mouse Look Component found on " + gameObject.name + " !");
            if (FocusAxisInteract == null)
                Debug.Log(" ERR# : No Focus Axis Interact Component found on " + gameObject.name + " !");
            if (FocusInteract == null)
                Debug.Log(" ERR# : No Focus Interact at Reach Component found on " + gameObject.name + " !");
            if (FPS_Man == null)
                Debug.Log(" ERR# : No FPS MANAGER at Reach Component found on " + gameObject.name + " !");
            // Int the Input System
            controls = new InputMap();
            FocusActions = controls.Focus;
            playerCamera = FocusMouseLook.playerCamera.gameObject;
            // Init the input listeners
            CreateControlListener();
            UpdateControls();
        }

        public void StartFocus(GameObject Target = null, float Fov = 0)
        {
            CurrentFocusTarget = Target;
            print("Start Focus");
            if (SysSwap == null)
                SysSwap = gameObject.AddComponent<ChangePlayerSystems>();
            O_FOV = playerCamera.GetComponent<Camera>().fieldOfView;
            if (Fov == 0)
                Fov = CameraFOV;
            SysSwap.AutoSwap(playerCamera, Target, Fov, FocusActivationTime);
            if (Target != null)
            {
                FocusAxisInteract.Item = Target;
            }
        }

        public void EndFocus()
        {
            print("End Focus");
            if (SysSwap == null)
                SysSwap = gameObject.AddComponent<ChangePlayerSystems>();
            SysSwap.AutoSwap(playerCamera, CurrentFocusTarget, O_FOV, FocusActivationTime);
            CurrentFocusTarget = null;
            FocusAxisInteract.Item = null;
        }

        private void LateUpdate()
        {
            if (isFocusActive)
            {
                DetectInputType();
                if (!isKinematic)
                {
                    FocusMouseLook.ReceiveInput(MouseInput, isInputGamepad);
                    if (CanAxisInteract)
                        FocusAxisInteract.ReceiveInput(InteractAxisInput);
                    else
                        FocusAxisInteract.ReceiveInput(Vector2.zero);
                }
                else
                {
                    FocusMouseLook.ReceiveInput(Vector2.zero, isInputGamepad);
                    FocusAxisInteract.ReceiveInput(Vector2.zero);
                }
            }
        }

        public void CreateControlListener()
        {
            // Axis Interact
            FocusActions.AxisInteract.performed += ctx => InteractAxisInput = ctx.ReadValue<Vector2>();
            // Mouse Look
            FocusActions.MouseX.performed += ctx => MouseInput.x = ctx.ReadValue<float>();
            FocusActions.MouseY.performed += ctx => MouseInput.y = ctx.ReadValue<float>();
            // Interact
            FocusActions.Interact.performed += _ => FocusInteract.OnInteractPressed();
            // Cancel
            FocusActions.Cancel.performed += _ => FocusInteract.OnCancelPressed();
            // device control detection
            FocusActions.Get().actionTriggered += ctx => lastDevice = ctx.control?.device;
            // rebinding
        }

        public void UpdateControls()
        {
            FocusMouseLook.CanLook = CanLook;
            FocusInteract.CanInteract = CanInteract;
            FocusAxisInteract.CanInteract = CanAxisInteract;
            FocusInteract.CanCancel = CanCancel;
        }


        public void ChangeControls(bool L, bool I, bool C, bool CI)
        {
            CanLook = L;
            CanInteract = I;
            CanAxisInteract = CI;
            CanCancel = C;
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
    }
}