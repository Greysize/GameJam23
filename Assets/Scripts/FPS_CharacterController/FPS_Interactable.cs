using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace FPS_Controller
{
    public class FPS_Interactable : MonoBehaviour
    {
        [Header("Status")]
        public bool isActive;
        public bool isInPlayerHands;
        public bool isSnapped = false;
        public GameObject CurrentSnap = null;
        [Header("Snap-on")]
        public bool canSnap;
        public string TargetName;
        public float TargetDistanceCheck = 1f;
        public float TargetPlaceDuration = 0.5f;

        [Header("Button Mode / Distant Grab")]
        public bool isButton = false;
        public UnityEvent ButtonPressed;

        [Header("Can Focus")]
        public bool canFocusOn = true;

        [Header("Events")]
        public UnityEvent OnGrab;
        public UnityEvent OnDrop;
        public UnityEvent OnSnap;
        public UnityEvent OnFocus;

        [HideInInspector]
        public bool inProcess = false;
        private Collider Col;
        private Rigidbody RB;
        private FPS_Interact FPS_InteractionManager;
        public void Start()
        {
            FPS_InteractionManager = FindObjectOfType<FPS_Interact>();
            RB = gameObject.GetComponent<Rigidbody>();
            Col = gameObject.GetComponent<Collider>();
            if (RB == null)
                Debug.Log("No RigidBody on Interactable !");
        }

        public void OnInteract(Transform Parent = null)
        {
            if (!isButton)
            {
                if (Parent == null)
                    Drop();
                else
                    Grab(Parent);
            }
            else
                ButtonPressed.Invoke();
        }

        private void Drop(bool isSnap = false)
        {
            isInPlayerHands = false;
            if (!isSnap)
            {
                RB.useGravity = true;
                RB.isKinematic = false;
            }
            if (Col != null)
                Col.enabled = true;
            gameObject.transform.parent = null;
            ForceDropLayer(gameObject);
            if (!isSnap)
                OnDrop.Invoke();
            print("End drop");
        }

        public void ForceDropLayer(GameObject obj )
        {
            var Num = ((int)(Mathf.Log(FPS_InteractionManager.interactionLayer[0].value, 2)));
            ChangeRenderLayer(obj, Num);
        }

        public void ForceGrabLayer(GameObject obj)
        {
            ChangeRenderLayer(obj, 8);
        }

        private void Grab(Transform Parent)
        {
            print("Grab to " + Parent.name);
            InitGrab();
            gameObject.transform.parent = Parent;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            ForceGrabLayer(gameObject);
        }

        private void InitGrab()
        {
            isSnapped = false;
            CurrentSnap = null;
            isInPlayerHands = true;
            RB.useGravity = false;
            RB.isKinematic = true;
            if (Col != null)
                Col.enabled = false;
            OnGrab.Invoke();
        }



        private void ChangeRenderLayer(GameObject obj, int Num)
        {
            print("Change Layer from " + obj.layer +" to " + Num);
            obj.layer = Num;

            foreach (MeshFilter child in obj.GetComponentsInChildren<MeshFilter>())
            {
                child.gameObject.layer = Num;
                //ChangeRenderLayer(child.gameObject, Num);
            }
        }


    }
}