using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS_Controller
{
    public class Grab_Actions : MonoBehaviour
    {
        [Header("Multi Grab")]
        public List<GameObject> Grabbables;
        public List<Transform> Targets;
        public float duration = 0.5f;
        [Header("Drop Pin")]
        public List<Transform> DropPoints;
        private FPS_Interact Interact_Man;
        [Header("Special Focus")]
        public bool isSpecialFocus;
        public UnityEvent SpecialFocusEvent;
        private List<Animator> BaguetteAnim;
        // Start is called before the first frame update


        public void Start()
        {
            Interact_Man = FindObjectOfType<FPS_Interact>();
            if (Interact_Man != null && Targets.Count == 0)
                Targets = Interact_Man.MultiGrabAttach;
            else
                print("Can't find the interaction manager");
            //animators
            BaguetteAnim = new List<Animator>();
            foreach (GameObject Bag in Grabbables)
            {
                Bag.GetComponent<Animator>().enabled = false;
                BaguetteAnim.Add(Bag.GetComponent<Animator>());
            }
        }

        // Grab
        public void MultiGrab()
        {
            print("Multi Grab");
            int i = 0;
            foreach (GameObject Grabbable in Grabbables)
            {
                BaguetteAnim[i].enabled = false;
                Grabbable.GetComponent<Rigidbody>().isKinematic = true;
                Grabbable.GetComponent<Rigidbody>().useGravity = false;
                StartCoroutine(MoveToTarget(Grabbable, Grabbable.transform, Targets[i], duration));
                i++;
            }
        }
        //Drop

        public void PinnedDrop()
        {
            print("Pinned Drop");
            int i = 0;
            foreach (GameObject Grabbable in Grabbables)
            {
                BaguetteAnim[i].enabled = false;
                //BaguetteAnim[i].Play("Baguette_Idle");
                StartCoroutine(MoveToTarget(Grabbable, Grabbable.transform, DropPoints[i], duration, true));
                i++;
            }
        }

        //Focus
        public void SpecialFocus()
        {
            print("Special Focus");
            SpecialFocusEvent.Invoke();
        }

        IEnumerator MoveToTarget(GameObject Item, Transform Start, Transform End, float duration, bool isDrop=false)
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
            //if drop end with drop system
            if (isDrop)
                EndDrop(Item);
            else
                EndGrab(Item, End);
        }

        private void EndDrop(GameObject Item)
        {
            Item.GetComponent<FPS_Interactable>().OnInteract();
            Item.GetComponent<Rigidbody>().isKinematic = false;
            Item.GetComponent<Rigidbody>().useGravity = true;
        }
        private void EndGrab(GameObject Item, Transform Target)
        {
            Item.GetComponent<FPS_Interactable>().OnInteract(Target);
            Item.GetComponent<Rigidbody>().isKinematic = true;
            Item.GetComponent<Rigidbody>().useGravity = false;
        }

    }

}