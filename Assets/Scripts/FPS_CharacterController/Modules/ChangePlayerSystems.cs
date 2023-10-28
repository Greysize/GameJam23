using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FPS_Controller
{
    public class ChangePlayerSystems : MonoBehaviour
    {
        private bool FocusMode;
        private bool InputMode;

        private FPS_FocusInputManager FocusMan;
        private FPS_UI_Manager UIMan;
        private FPS_InputManager InputMan;
        private Transform ItemTargetTransfom;
        private Transform CameraTarget;
        private Quaternion CamTargetRot;
        private GameObject CameraItems;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            UIMan = FindObjectOfType<FPS_UI_Manager>();
            FocusMan = FindObjectOfType<FPS_FocusInputManager>();
            InputMan = FindObjectOfType<FPS_InputManager>();
            if (UIMan == null)
                Debug.Log("Can't find UI Manager");
            if (FocusMan == null)
                Debug.Log("Can't find Focus Manager");
            if (InputMan == null)
                Debug.Log("Can't find Input Manager");
            CameraItems = FocusMan.CameraItems;
        }

        public void SwapSystems()
        {
            // retrive
            FocusMode = FocusMan.isFocusActive;
            InputMode = InputMan.isInputActive;
            if (FocusMode)
            {
                FocusMode = false;
                InputMode = true;
                UIMan.InGameUIVisible(true);
            }
            else
            {
                FocusMode = true;
                InputMode = false;
                UIMan.InGameUIVisible(false);
            }
            // apply
            FocusMan.isFocusActive = FocusMode;
            InputMan.isInputActive = InputMode;
        }

        public void AutoSwap(GameObject Cam, GameObject Target, float FOV, float duration)
        {
            Init();
            SwapSystems();
            if (FocusMode)
            {
                ItemTargetTransfom = FocusMan.FocusTargetTransform;
                CameraTarget = ItemTargetTransfom;
                CamTargetRot = CameraTarget.rotation;
            }
            else
            {
                ItemTargetTransfom = InputMan.interact.Attachement;
                CameraTarget = InputMan.mouseLook.playerCamera;
                CamTargetRot = FocusMan.FocusMouseLook.RecordedCamRot;//Quaternion.Euler(FocusMan.FocusMouseLook.CameraRotCenter);
            }
            StartCoroutine(FocusChange(Cam, Target, FOV, duration, true));
        }

        IEnumerator FocusChange(GameObject playerCamera, GameObject Target, float EndFOV, float duration, bool isdestroy)
        {
            // init
            FocusMan.isFocusActive = false;
            InputMan.isInputActive = false;
            var EndRot = playerCamera.transform.rotation;
            var Cam = playerCamera.GetComponent<Camera>();
            var O_Rot = playerCamera.transform.rotation;
            Quaternion startRot = playerCamera.transform.rotation;
            var fov = Cam.fieldOfView;
            // calculate final rotation of the camera
            if (Target != null)
            {
                // move the Item to position
                gameObject.GetComponent<FPS_Interact>().OnMoveFocusItem(Target, ItemTargetTransfom, duration);
                //Target.GetComponent<FPS_Interactable>().OnMoveFocusItem(ItemTargetTransfom, duration);
                //get final camera rotation
                if (CameraTarget != playerCamera.transform)
                {
                    EndRot = Quaternion.LookRotation(CameraTarget.position - playerCamera.transform.position, Vector3.up);
                    print(" ITEM Go to " + EndRot.eulerAngles + " from " + playerCamera.transform.eulerAngles);
                }
                else
                {
                    EndRot = CamTargetRot;
                }
            }
            else
            {
                EndRot = CamTargetRot;
                print("No Item given");
            }
            // CAMERA ROTATION & FOVE LERP
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                Cam.fieldOfView = Mathf.Lerp(fov, EndFOV, (t / duration));
                if(CameraItems != null)
                    CameraItems.GetComponent<Camera>().fieldOfView = Mathf.Lerp(fov, EndFOV, (t / duration));
                playerCamera.transform.rotation = Quaternion.Slerp(startRot, EndRot, t / duration);
                yield return null;
            }
            // lock i9n end
            print("End : " + EndRot);
            playerCamera.transform.rotation = EndRot;
            Cam.fieldOfView = EndFOV;
            if (CameraItems != null)
                CameraItems.GetComponent<Camera>().fieldOfView = EndFOV;
            //let's parent the ITEM now
            if (Target != null)
            {
                Target.transform.parent = ItemTargetTransfom;
                Target.transform.position = ItemTargetTransfom.position;
                Target.transform.rotation = ItemTargetTransfom.rotation;
                // disable the colliders if we are in Focus mode only
                //if (Target.GetComponents<Collider>().Length != 0)
                //{
                //    foreach (Collider col in Target.GetComponents<Collider>())
                //    {
                //        col.enabled = InputMode;
                //    }
                //}
            }
            //end
            if (CameraTarget != playerCamera.transform)
            {
                FocusMan.FocusMouseLook.SaveCameraInfo(O_Rot);
            }
            FocusMan.isFocusActive = FocusMode;
            InputMan.isInputActive = InputMode;
            FocusMan.UpdateControls();
            InputMan.UpdateControls();
            if (isdestroy)
                Destroy(this);

            yield return null;
        }


        IEnumerator SmoothLookAt(Transform Main, Vector3 TargetPos, Vector3 upAxis, float duration)
        {
            Quaternion startRot = Main.rotation;
            Quaternion endRot = Quaternion.LookRotation(TargetPos - Main.position, upAxis);
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                Main.rotation = Quaternion.Slerp(startRot, endRot, t / duration);
                yield return null;
            }
            Main.rotation = endRot;
        }
    }


}