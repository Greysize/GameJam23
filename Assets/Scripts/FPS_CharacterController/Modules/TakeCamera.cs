using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FPS_Controller
{
    public class TakeCamera : MonoBehaviour
    {
        public GameObject Camera;
        public Transform EndCameraPose;
        public float PoseTime;
        public float CameraFOV;
        public bool isNarrowLook;
        public bool isLockedLook;
        public bool isCancellable;

        private FPS_MouseLook FPS_Look;
        private FPS_PromptActions LocalPrompt;
        private float O_CameraFOV;
        public void Start()
        {
            FPS_Look = FindObjectOfType<FPS_MouseLook>();
            LocalPrompt = gameObject.GetComponent<FPS_PromptActions>();
        }

        public void Activate()
        {
            if (Camera == null)
                Camera = FindObjectOfType<Camera>().gameObject;
            O_CameraFOV = Camera.GetComponent<Camera>().fieldOfView;
            if (!FPS_Look.isCameraPosed)
                StartCoroutine(FPS_Look.PoseCamera2(Camera, Camera.transform, EndCameraPose, Camera.GetComponent<Camera>().fieldOfView, CameraFOV, PoseTime, isCancellable));
        }

        public void Deactivate()
        {
            StartCoroutine(FPS_Look.PoseCamera2(Camera, Camera.transform, Camera.transform.parent.transform, Camera.GetComponent<Camera>().fieldOfView, O_CameraFOV, PoseTime, false));
        }
    }
}