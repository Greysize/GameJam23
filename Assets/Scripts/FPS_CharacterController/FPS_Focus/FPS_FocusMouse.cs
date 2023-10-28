using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FPS_Controller
{
    public class FPS_FocusMouse : MonoBehaviour
    {
        [Header("Look")]
        public bool CanLook;
        public Transform playerCamera;
        [Tooltip("Horizontal Axis")]
        [SerializeField] float sensitivityX = 8;
        [Tooltip("Vertical Axis")]
        [SerializeField] float sensitivityY = 0.5f;
        [SerializeField] float multiplier = 1f;
        public float Clamp = 10f;
        float MouseY, MouseX;
        [Space]
        [Header("Force Look")]
        public bool isForceLook = false;
        public Transform Aim;
        public float Pause = 1f;
        public float Duration = 2f;
        public float MinSensitivity = 0.1f;
        float xRotation = 0f;
        float yRotation = 0f;
        FPS_FocusInputManager FocusMan;
        [HideInInspector]
        public Vector3 CameraRotCenter;
        private Vector2 CamXClamp;
        private Vector2 CamYClamp;
        [HideInInspector]
        public Quaternion RecordedCamRot;

        private void Start()
        {
            FocusMan = GetComponent<FPS_FocusInputManager>();
        }

        public void SaveCameraInfo(Quaternion CamRot)
        {
            RecordedCamRot = CamRot;
            CameraRotCenter = playerCamera.eulerAngles;
            CamXClamp = new Vector2((CameraRotCenter.x - Clamp), CameraRotCenter.x + Clamp);
            CamYClamp = new Vector2((CameraRotCenter.y - Clamp), CameraRotCenter.y + Clamp);
            print("Camera rot " + CameraRotCenter + " | ClampX " + CamXClamp + " | CalmpY" + CamYClamp);
        }

        public void LateUpdate()
        {
            if (CanLook && FocusMan.isFocusActive)
            {
                //record camera rotation
                Vector3 targetRotation = playerCamera.eulerAngles;
                // add input values
                xRotation = (targetRotation.x + MouseX);
                yRotation = (targetRotation.y + MouseY);
                //clamp
                if (gameObject.transform.eulerAngles.y > 180)
                    yRotation = ClampAngle(yRotation, CamYClamp.x, CamYClamp.y, true);
                else
                    yRotation = ClampAngle(yRotation, CamYClamp.x, CamYClamp.y, false);
                xRotation = ClampAngle(xRotation, CamXClamp.x, CamXClamp.y, false);

                // apply to camera
                targetRotation.x = xRotation;
                targetRotation.y = yRotation;
                playerCamera.eulerAngles = targetRotation;
                if (isForceLook)
                {
                    if (MouseX <= MinSensitivity && MouseY <= MinSensitivity)
                    {
                        StartCoroutine(WaitForForce());
                    }
                }
            }


        }


        public void ReceiveInput(Vector2 mouseInput, bool isGamepad)
        {
            MouseY = mouseInput.x * sensitivityX * multiplier;
            MouseX = -mouseInput.y * sensitivityY * multiplier;
        }

        IEnumerator WaitForForce()
        {
            bool test = true;
            float elapsedTime = 0f;
            while (elapsedTime < Duration)
            {
                if (MouseX > MinSensitivity || MouseY > MinSensitivity)
                {
                    test = false;
                }
                yield return null;
                if (test)
                    StartCoroutine(ForceCameraToAim());
            }
            yield return null;
        }
        IEnumerator ForceCameraToAim()
        {
            var StartRot = playerCamera.eulerAngles;
            //disable temporarey the look
            CanLook = false;
            float elapsedTime = 0f;
            var EndRot = CameraRotCenter;
            if (Aim != null)
                EndRot = Aim.eulerAngles;
            while (elapsedTime < Duration)
            {
                var rotationToAdd = Vector3.Lerp(StartRot, EndRot, (elapsedTime / Duration));
                //playerCamera.Rotate(rotationToAdd);
                playerCamera.eulerAngles = Vector3.Slerp(StartRot, EndRot, (elapsedTime / Duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // lock in final position/rotation
            playerCamera.eulerAngles = EndRot;
            CanLook = true;
            yield return null;
        }

        //stable clamp for variable -/+ values around 360
        public static float ClampAngle(float angle, float min, float max, bool isvertical)
        {
            if (isvertical)
                return Mathf.Clamp((angle >= 180) ? angle : (360 + angle), min, max);
            else
                return Mathf.Clamp((angle <= 180) ? angle : -(360 - angle), min, max);
        }

    }
}