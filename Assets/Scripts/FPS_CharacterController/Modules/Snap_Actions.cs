using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS_Controller
{
    public class Snap_Actions : MonoBehaviour
    {
        public bool isActive = true;
        public bool isPlatine = false;
        public Transform VinylePlatine;
        public Transform SleevePlatine;
        public GameObject VinylGO;
        public GameObject SleeveGO;
        public UnityEvent OnSnap;
        public UnityEvent OnUnSnap;
        public FPS_Interact FPS_Inter;
        private GameObject InstalledVinyle;
        private FPS_Interactable Interact_Man;

        public void Start()
        {
            Interact_Man = FindObjectOfType<FPS_Interactable>();//
        }
        public void Snapping(GameObject Item)
        {
            Interact_Man.ForceDropLayer(VinylGO);
            Interact_Man.ForceDropLayer(SleeveGO);
            if (isActive)
                OnSnap.Invoke();
        }

        public void UnSnapping()
        {
            print("Unsnapping from Platine " + VinylGO.name + " and " + SleeveGO.name);
            Interact_Man.ForceGrabLayer(VinylGO);
            Interact_Man.ForceGrabLayer(SleeveGO);
            VinylGO.GetComponent<Rigidbody>().isKinematic = false;
            VinylGO.GetComponent<BoxCollider>().isTrigger = false;
            //StartCoroutine(MoveToTarget(SleeveGO, SleeveGO.transform, VinylGO.transform, 1f));
            StartCoroutine(MoveToTarget(VinylGO, VinylGO.transform, SleeveGO.transform, 1f));
            VinylGO.transform.position = SleeveGO.transform.position;
            VinylGO.transform.rotation = SleeveGO.transform.rotation;
            SleeveGO.transform.parent = null;
            VinylGO.transform.parent = SleeveGO.transform.parent;
            print("reparenting");
            InstalledVinyle = null;
            if (isActive)
                OnUnSnap.Invoke();
        }

        public void Init_Snap()
        {
            // separate the Sleeve and the vinyle
            VinylGO.transform.parent = VinylePlatine.gameObject.transform;
            VinylGO.GetComponent<Rigidbody>().isKinematic = true;
            VinylGO.GetComponent<BoxCollider>().isTrigger = true;

            StartCoroutine(MoveToTarget(VinylGO, VinylGO.transform, VinylePlatine, 1f));
            StartCoroutine(MoveToTarget(SleeveGO, SleeveGO.transform, SleevePlatine, 1f));
        }

        IEnumerator MoveToTarget(GameObject Item, Transform Start, Transform End, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                Item.transform.position = Vector3.Lerp(Start.position, End.position, (elapsedTime / duration));
                Item.transform.rotation = Quaternion.Lerp(Start.rotation, End.rotation, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // lock in final position/rotation
            Item.transform.position = End.transform.position;
            Item.transform.rotation = End.transform.rotation;
        }
    }
}