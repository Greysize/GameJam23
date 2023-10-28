using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS_Controller
{
    public class ChangePlayerModules : MonoBehaviour
    {
        [Header("Swap the state during Action")]
        public bool SwapJump;
        public bool SwapIntercat;
        public bool SwapCancel;
        public bool SwapMovements;
        public bool SwapLook;
        public bool SwapNarrowLook;
        public bool SwapInteractDiapo;
        public bool SwapInteractDistance;
        [Header("Toggle ON and maintain after action")]
        public bool ToggleONJump;
        public bool ToggleONInteract;
        public bool ToggleONCancel;
        public bool ToggleONMovements;
        public bool ToggleONLook;
        public bool ToggleONNarrowLook;
        public bool ToggleONInteractDiapo;
        public bool ToggleONInteractDistance;
        [Header("Toggle OFF and maintain after action")]
        public bool ToggleOFFJump;
        public bool ToggleOFFInteract;
        public bool ToggleOFFCancel;
        public bool ToggleOFFMovements;
        public bool ToggleOFFLook;
        public bool ToggleOFFNarrowLook;
        public bool ToggleOFFInteractDiapo;
        public bool ToggleOFFInteractDistance;

        // Original Player Data
        private bool O_Interact;
        private bool O_Cancel;
        private bool O_Jump;
        private bool O_Movements;
        private bool O_Look;
        private bool O_Narrow_Look;
        private float O_Cam_FOV;
        private bool O_Interact_Diapo;
        private bool O_Interact_Distance;

        private FPS_InputManager FPS_Manager;

        public void Start()
        {
            FPS_Manager = FindObjectOfType<FPS_InputManager>();
        }

        public void Activate()
        {
            // record original state
            RecordPlayerModules();
            bool M = FPS_Manager.CanMove;
            bool L = FPS_Manager.CanLook;
            bool NL = FPS_Manager.CanNarrowLook;
            bool J = FPS_Manager.CanJump;
            bool I = FPS_Manager.CanInteract;
            bool C = FPS_Manager.CanCancel;
            bool D = FPS_Manager.CanInteractDiapo;
            bool ID = FPS_Manager.CanInteractDistance;
            // change Player values
            UpdatePlayerModule(M, L, NL, J, I, C, D, ID);
        }

        public void Deactivate()
        {
            FPS_Manager.isKinematic = true;
            // Reset to original values
            if (SwapIntercat)
                FPS_Manager.CanInteract = O_Interact;
            if (SwapNarrowLook)
                FPS_Manager.CanCancel = O_Cancel;
            if (SwapJump)
                FPS_Manager.CanJump = O_Jump;
            if (SwapMovements)
                FPS_Manager.CanMove = O_Movements;
            if (SwapLook)
                FPS_Manager.CanLook = O_Look;
            if (SwapNarrowLook)
                FPS_Manager.CanNarrowLook = O_Narrow_Look;
            if (SwapInteractDiapo)
                FPS_Manager.CanInteractDiapo = O_Interact_Diapo;
            if (SwapInteractDistance)
                FPS_Manager.CanInteractDistance = O_Interact_Distance;
            FPS_Manager.UpdateControls();
            FPS_Manager.isKinematic = false;
        }


        private void UpdatePlayerModule(bool M, bool L, bool NL, bool J, bool I, bool C, bool D, bool ID)
        {
            // INVERT
            if (SwapIntercat)
                I = !I;
            if (SwapCancel)
                C = !C;
            if (SwapJump)
                J = !J;
            if (SwapMovements)
                M = !M;
            if (SwapLook)
                L = !L;
            if (SwapNarrowLook)
                NL = !NL;
            if (SwapInteractDiapo)
                D = !D;
            if (SwapInteractDistance)
                ID = !ID;
            //ENABLE
            if (ToggleONInteract)
                I = true;
            if (ToggleONCancel)
                C = true;
            if (ToggleONJump)
                J = true;
            if (ToggleONMovements)
                M = true;
            if (ToggleONLook)
                L = true;
            if (ToggleONNarrowLook)
                NL = true;
            if (ToggleONInteractDiapo)
                D = true;
            if (ToggleONInteractDistance)
                ID = true;
            //DISABLE
            if (ToggleOFFInteract)
                I = false;
            if (ToggleOFFCancel)
                C = false;
            if (ToggleOFFJump)
                J = false;
            if (ToggleOFFMovements)
                M = false;
            if (ToggleOFFLook)
                L = false;
            if (ToggleOFFNarrowLook)
                NL = false;
            if (ToggleOFFInteractDiapo)
                D = false;
            if (ToggleOFFInteractDistance)
                ID = false;
            // update listenrer
            FPS_Manager.ChangeControls(M, L, J, NL, I, C, D, ID);
        }
        private void RecordPlayerModules()
        {
            O_Interact = FPS_Manager.CanInteract;
            O_Cancel = FPS_Manager.CanCancel;
            O_Jump = FPS_Manager.CanJump;
            O_Movements = FPS_Manager.CanMove;
            O_Look = FPS_Manager.CanLook;
            O_Narrow_Look = FPS_Manager.CanNarrowLook;
            O_Interact_Diapo = FPS_Manager.CanInteractDiapo;
            O_Interact_Distance = FPS_Manager.CanInteractDistance;
            //if (TakeOverCamera)
            //{
            //    O_Cam_FOV = Camera.GetComponent<Camera>().fieldOfView;
            //}
        }
    }
}