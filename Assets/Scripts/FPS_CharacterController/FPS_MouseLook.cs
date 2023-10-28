using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FPS_Controller
{
    public class FPS_MouseLook : MonoBehaviour
    {
#pragma warning disable 649
        [Header("Look")]
        public bool CanLook = true;
        [Tooltip("Horizontal Axis")]
        [SerializeField] float sensitivityX = 8;
        [Tooltip("Vertical Axis")]
        [SerializeField] float sensitivityY = 0.5f;
        [SerializeField] float multiplier = 1f;
        float MouseY, MouseX;
        [SerializeField] Vector2 GamePadMultiplier;
        [SerializeField] Vector2 MouseMultiplier;
        [Space]
        [Header("Narrow Look")]
        public bool CanNarrowLook = false;
        public float NarrowLookAngleLimitHorizontal = 15;
        public float NarrowLookAngleLimitVertical = 15;
        public float NarrowLookSensibilityMultiplier = 0.3f;
        [Header("Look Angle Force Feedback")]
        public bool isForceFeedback = false;
        public float ForceDelay = 1f;
        public float ForceStrengh = 2f;
        public Transform Aim = null;
        [Header("Camera Posing")]
        public bool isCameraPosed = false;
        public Transform RootCamera;
        [Header("General")]
        public Transform playerCamera;
        [SerializeField] float verticalClamp = 85;
        float xRotation = 0f;
        float yRotation = 0f;
        FPS_InputManager FPS_Manager;
        FPS_FocusInputManager FocusInputMan;

        private void Start()
        {
            FPS_Manager = FindObjectOfType<FPS_InputManager>();
            FocusInputMan = FindObjectOfType<FPS_FocusInputManager>();
        }

        public void LateUpdate()
        {
            if (FPS_Manager.isInputActive && CanLook)
            {
                Vector3 targetRotation = transform.eulerAngles;
                // Adjust input depending on CnLook & NarrowLook Sensitivity
                if (!CanLook)
                {
                    MouseY = 0;
                    MouseX = 0;
                }
                if (CanNarrowLook)
                {
                    MouseY = MouseY * NarrowLookSensibilityMultiplier / 10f;
                    MouseX = MouseX * NarrowLookSensibilityMultiplier * 2f;
                }

                //horizontal look
                yRotation = MouseY;
                if (CanNarrowLook)
                {
                    yRotation = Mathf.Clamp(yRotation, -NarrowLookAngleLimitHorizontal, NarrowLookAngleLimitHorizontal);
                    targetRotation.y = yRotation;
                }
                else
                {
                    transform.Rotate(Vector3.up, yRotation * Time.deltaTime);
                }
                //vertical look
                xRotation -= MouseX; //invert input
                if (CanNarrowLook)
                {
                    xRotation = Mathf.Clamp(xRotation, -NarrowLookAngleLimitVertical, NarrowLookAngleLimitVertical);
                }
                else
                {
                    xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp); // limit the max up and max down
                }
                // appply
                targetRotation.x = xRotation;
                playerCamera.eulerAngles = targetRotation;
            }
        }

        public void ReceiveInput(Vector2 mouseInput, bool isGamepad)
        {
            if (isGamepad)
            {
                MouseY = mouseInput.x * sensitivityX * multiplier * (float)MouseMultiplier.x;
                MouseX = mouseInput.y * sensitivityY * multiplier * (float)MouseMultiplier.y;
            }
            if (!isGamepad)
            {
                MouseY = mouseInput.x * sensitivityX * multiplier * (float)GamePadMultiplier.x;
                MouseX = mouseInput.y * sensitivityY * multiplier * (float)GamePadMultiplier.y;
            }
        }

        public IEnumerator PoseCamera(GameObject Camera, Transform Start, Transform End, float StartFOV, float EndFOV, float duration, bool isCancellable)
        {
            if (End != Camera.transform)
                FocusInputMan.StartFocus(End.gameObject, EndFOV);
            else
                FocusInputMan.EndFocus();
            FPS_Manager.isKinematic = true;
            print("Pose Camera from " + Start.gameObject.name + " to " + End.gameObject.name);
            //var EndRot = Quaternion.LookRotation(End.position - Start.position, Vector3.up);
            //float elapsedTime = 0f;
            //while (elapsedTime < duration)
            //{
            //    Camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(StartFOV, EndFOV, (elapsedTime / duration));
            //    Camera.transform.position = Vector3.Lerp(Start.position, End.position, (elapsedTime / duration));
            //    Camera.transform.rotation = Quaternion.Slerp(Start.rotation, EndRot, (elapsedTime / duration));
            //    elapsedTime += Time.deltaTime;
            //    yield return null;
            //}
            // lock in final position/rotation
            //Camera.GetComponent<Camera>().fieldOfView = EndFOV;
            //Camera.transform.position = End.transform.position;
            //Camera.transform.rotation = EndRot;
            FPS_Manager.isKinematic = false;
            FPS_Manager.CanCancel = isCancellable;

            FPS_Manager.isPosed = !FPS_Manager.isPosed;
            isCameraPosed = FPS_Manager.isPosed;
            print(" is paused = " + FPS_Manager.isPosed);
            FPS_Manager.UpdateControls();
            yield return null;
        }

        public IEnumerator PoseCamera2(GameObject Camera, Transform Start, Transform End, float StartFOV, float EndFOV, float duration, bool isCancellable)
        {
            FPS_Manager.isKinematic = true;
            print("Pose Camera from " + Start.gameObject.name + " to " + End.gameObject.name);
            var EndRot = Quaternion.LookRotation(End.position - Start.position, Vector3.up);
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                Camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(StartFOV, EndFOV, (elapsedTime / duration));
                Camera.transform.position = Vector3.Lerp(Start.position, End.position, (elapsedTime / duration));
                Camera.transform.rotation = Quaternion.Slerp(Start.rotation, EndRot, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // lock in final position/rotation
            Camera.GetComponent<Camera>().fieldOfView = EndFOV;
            Camera.transform.position = End.transform.position;
            Camera.transform.rotation = EndRot;
            FPS_Manager.isKinematic = false;
            FPS_Manager.CanCancel = isCancellable;

            FPS_Manager.isPosed = !FPS_Manager.isPosed;
            isCameraPosed = FPS_Manager.isPosed;
            print(" is paused = " + FPS_Manager.isPosed);
            FPS_Manager.UpdateControls();
            yield return null;
        }
    }
}