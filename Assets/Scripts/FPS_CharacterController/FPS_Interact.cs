using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace FPS_Controller
{
    public class FPS_Interact : MonoBehaviour
    {
        [Header("Interactable")]
        private float CurrentInteractDistance = 1f;
        [SerializeField] float InteractDistance = 1f;
        [SerializeField] float InteractFarDistance = 100f;
        [SerializeField] float InteractRadius = 0.5f;
        public Transform Attachement;
        [Header("Multi Grab")]
        public List<Transform> MultiGrabAttach;
        [Space]
        public List<LayerMask> interactionLayer;
        [SerializeField] float MoveToHandSpeed;
        public bool CanInteract = true;
        public bool CanInteractUI = true;
        public bool CanInteractDiapo = true;
        public bool CanCancel = false;
        [Header("UI")]
        [SerializeField] float UIInteractionDistance = 1f;
        [SerializeField] LayerMask UIInteractionLayer;
        [Header("Diaporama")]
        public float delay = 1f;
        public UnityEvent ActivateDiapo;


        // private
        private FPS_InputManager InputMan;
        private GameObject InteractableInHand;
        [HideInInspector]
        public FPS_PromptActions ActivePrompt;
        private GameObject InteractableAtReach;
        //private GameObject ItemInHand;
        private GameObject InteractableUIAtReach;
        //[HideInInspector]
        public bool inProcess = false;
        private bool isInteractableInHand = false;
        private bool FocusActive;
        private Transform PlayerCamera;
        private FPS_FocusInputManager FocusInputMan;
        //private FBI.SceneTriggerManager SceneTrigger;
        private Coroutine Loading;
        private List<int> interactionLayerInt;
        private void Awake()
        {
            FocusActive = false;
            PlayerCamera = gameObject.GetComponent<FPS_MouseLook>().playerCamera;
            FocusInputMan = FindObjectOfType<FPS_FocusInputManager>();
            if (PlayerCamera == null)
                Debug.Log(" ERR# : No Camera to raycast for itneraction");
            InputMan = FindObjectOfType<FPS_InputManager>();
            if (InputMan == null)
                Debug.Log("Can't find the input manager");
            //init and fill the interactionlayerint
            interactionLayerInt = new List<int>();
            foreach (LayerMask mask in interactionLayer)
            {
                print("add layer " + (int)(Mathf.Log(mask.value, 2)));
                interactionLayerInt.Add((int)(Mathf.Log(mask.value, 2)));
            }
        }

        // Main interact function ( starter for all type : Grab / Drop / Swap / Snap / Focus / UI)
        public void OnInteractPressed(bool isDiapo = false, bool Diapoforward = true)
        {
            if (InputMan.isInputActive)
            {
                if (CanInteract)
                {
                    InteractableUIAtReach = UICheck();
                    if (InteractableUIAtReach == null)
                    {
                        print(" >> No UI at reach");
                        // If nothing in Hand
                        if (!isInteractableInHand && !inProcess && !isDiapo)
                        {
                            print(" >> Nothing in hand is there something at reach ?");
                            // test if interactable is at reach
                            InteractableAtReach = InteractionCheck();
                            // Grab object
                            if (InteractableAtReach != null && !isInteractableInHand)
                                Grab();
                        }
                        //Diapo 
                        if (InteractableAtReach == null && isDiapo)
                        {
                            print("can diapo move");
                            if (CanInteractDiapo)
                            {
                                //DiapoAction(Diapoforward);
                            }
                        }
                        // If something in hand
                        if (isInteractableInHand && InteractableInHand != null && !inProcess && !isDiapo)
                        {
                            // test if there is a snap surface
                            var possibleSnap = SnapCheck();
                            if (possibleSnap != null)
                                OnMoveSnapItem(InteractableInHand, possibleSnap);
                            //if not snapping then let's check Swapping
                            if (!inProcess)
                            {
                                // test if user want to swap object in hand
                                InteractableAtReach = InteractionCheck();
                                if (InteractableAtReach != null)
                                    Swap();
                            }
                            // else start focus
                            if (!inProcess)
                            {
                                if (InteractableInHand.GetComponent<Grab_Actions>() != null)
                                {
                                    if(InteractableInHand.GetComponent<Grab_Actions>().isSpecialFocus)
                                    {
                                        InteractableInHand.GetComponent<Grab_Actions>().SpecialFocus();
                                    }
                                    else
                                        FocusInputMan.StartFocus(InteractableInHand);
                                }
                                else
                                    FocusInputMan.StartFocus(InteractableInHand);
                            }
                        }
                    }
                    else
                    {
                        if (CanInteractUI && !isDiapo)
                        { 
                        //interact with UI
                        if (!inProcess)
                            InteractUI();
                        }
                    }
                }
            }
        }

        public void ChangeInteractDistance(bool isDistant)
        {
            if (isDistant)
            {
                print(" Far distance ON @" + InteractFarDistance);
                CurrentInteractDistance = InteractFarDistance;
                UIInteractionDistance = InteractFarDistance;
            }
            else
            {
                print(" Normal distance ON @" + InteractDistance);
                CurrentInteractDistance = InteractDistance;
                UIInteractionDistance = InteractDistance;
                ;
            }
        }

        // to attach the interact with the current scene trigger
        public void FindSceneTrigger()
        {
            //SceneTrigger = FindObjectOfType<FBI.SceneTriggerManager>();
        }

        // diapo controls
       /* public void DiapoAction(bool isForward)
        {
            //if (!TutorialMode)
            //{
                // udpdate / find scene trigger
                FindSceneTrigger();
                if (SceneTrigger != null)
                {
                    bool check = true;
                    // if reverse swap the scene to load
                    if (!isForward)
                    {
                        check = SceneTrigger.SwapToPreviousScene();
                    }
                    // send to switch Scene Event
                    if (check)
                    {
                        ActivateDiapo.Invoke();
                        if (delay == 0f)
                        {
                            SceneTrigger.TriggerChangeScenes();
                        }
                        else
                        {
                            if (Loading != null)
                                StopCoroutine(Loading);
                            Loading = StartCoroutine(LoadSceneWithDelay());
                        }
                    }
                }
                else
                {
                    Debug.LogError(" ERR : can't find Scene management...");
                }
            //}
            //else
            //{
            //    Tutorial.Invoke();
            //}
        }

        // diapo loading with delay
        IEnumerator LoadSceneWithDelay()
        {
            yield return new WaitForSeconds(delay);
            SceneTrigger.TriggerChangeScenes();
        }

        */


        // Allow to cancel a given operation
        public void OnCancelPressed()
        {
            if (InputMan.isInputActive)
            {
                // UI
                if (CanCancel)
                    ActivePrompt.OnExitPrompt();
                // Drop
                if (InteractableInHand != null && isInteractableInHand && !CanCancel)
                    Drop();
            }
        }

        // When you snap object Coroutine starter
        public void OnMoveSnapItem(GameObject Item, GameObject target)
        {
            inProcess = true;
            var T = Item.GetComponent<FPS_Interactable>();
            if (T.canSnap)
                StartCoroutine(MoveInteractable(Item, Item.transform, target.gameObject.transform, T.TargetPlaceDuration));
        }

        // When you focus object Coroutine starter
        public void OnMoveFocusItem(GameObject Item, Transform Target, float duration)
        {
            var T = Item.GetComponent<FPS_Interactable>();
            if (T.canFocusOn)
                StartCoroutine(MoveInteractable(Item, Item.transform, Target, duration));
        }

        // Test if there is an Interactable at reach
        private GameObject InteractionCheck()
        {
            GameObject result = null;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f, 0f));

            //if (Physics.SphereCast(ray, InteractRadius, out hit, InteractDistance, interactionLayer))
            foreach (LayerMask mask in interactionLayer)
            {
                print(" >> Testing object on layer " + (int)(Mathf.Log(mask.value, 2)));
                if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, CurrentInteractDistance, mask) && result == null)
                {
                    if (hit.collider.gameObject.GetComponent<FPS_Interactable>() != null)
                    { 
                        result = hit.collider.gameObject;
                        if (!result.GetComponent<FPS_Interactable>().isActive)
                            result = null;
                    }
                }
            }
            return result;
        }

        // Test if there is a Snap at reach
        private GameObject SnapCheck()
        {
            GameObject result = null;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f, 0f));
            // test if we havbe an object in hand if ther eis a snap at reach
            if (InteractableInHand != null)
            {
                if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, CurrentInteractDistance))
                //if (Physics.SphereCast(ray, InteractRadius, out hit, InteractDistance))
                {
                    if (hit.collider.tag == InteractableInHand.GetComponent<FPS_Interactable>().TargetName)
                    {
                        //Debug.Log("Snap at reach " + hit.collider.gameObject.name);
                        result = hit.collider.gameObject;
                        if (!InteractableInHand.GetComponent<FPS_Interactable>().canSnap)
                            result = null;
                    }
                }
            }
            return result;
        }


        // Grab 
        private void Grab()
        {
            inProcess = true;
            if (InteractableAtReach != null)
            {
                if (InteractableAtReach.gameObject.GetComponent<Rigidbody>() != null)
                {
                    print("Grabbing " + InteractableAtReach.name);
                    if (!InteractableAtReach.GetComponent<FPS_Interactable>().isButton)
                    {
                        //test if snapped
                        if (InteractableAtReach.GetComponent<FPS_Interactable>().CurrentSnap != null)
                        {
                            InteractableAtReach.GetComponent<FPS_Interactable>().CurrentSnap.GetComponent<Snap_Actions>().UnSnapping();
                        }
                        // test if custom grab actions
                        if (InteractableAtReach.GetComponent<Grab_Actions>() != null)
                        {
                            print("Grab advqanced funstionc");
                            InteractableAtReach.GetComponent<Grab_Actions>().MultiGrab();
                            StartCoroutine(MoveInteractable(InteractableAtReach.gameObject, InteractableAtReach.gameObject.transform, Attachement, MoveToHandSpeed, true));
                        }
                        else
                        {
                        StartCoroutine(MoveInteractable(InteractableAtReach.gameObject, InteractableAtReach.gameObject.transform, Attachement, MoveToHandSpeed));
                        }
                    }
                    else
                    {
                        //is it a button only
                        InteractableAtReach.GetComponent<FPS_Interactable>().OnInteract();
                        ResetStatus();
                    }
                }
                else
                    Debug.Log("No rigidbody attached to " + InteractableAtReach.gameObject.name);
            }
        }

        // Drop
        private void Drop()
        {
            inProcess = true;
            if (InteractableInHand != null)
            {
                if (InteractableInHand.GetComponent<Rigidbody>() != null)
                {
                    if (InteractableInHand.GetComponent<Grab_Actions>() != null)
                    {
                        InteractableInHand.GetComponent<Grab_Actions>().PinnedDrop();
                        ResetStatusDrop();
                    }
                    else
                        StartCoroutine(TimedDrop(MoveToHandSpeed));
                }
            }
        }


        // Swap
        private void Swap()
        {
            print("Swapping " + InteractableInHand.name + " with " + InteractableAtReach.name);
            inProcess = true;
            //Drop
            if (InteractableInHand != null)
            {
                Drop();
                // Grab new item
                if (InteractableAtReach == null)
                    InteractableAtReach = InteractionCheck();
                if (InteractableAtReach != null)
                    Grab();
            }
        }

        private void InteractUI()
        {
            if (InteractableUIAtReach != null && !inProcess)
            {
                if (InteractableUIAtReach.GetComponent<Button>() != null)
                {
                    print("pressing Button " + InteractableUIAtReach.name);
                    InteractableUIAtReach.GetComponent<Button>().onClick.Invoke();
                    StartCoroutine(TimerGrab(1f));
                }
            }
        }

        private GameObject UICheck()
        {
            GameObject result = null;
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, UIInteractionDistance))
            {
                print("testing UI for " + hit.collider.gameObject.name + " | " + hit.collider.gameObject.layer + " >> " + (int)(Mathf.Log(UIInteractionLayer.value, 2)));
                if (hit.collider.gameObject.layer == (int)(Mathf.Log(UIInteractionLayer.value, 2)))
                {
                    // we have an interactable
                    result = hit.collider.gameObject;
                    Debug.Log("UI " + hit.collider.gameObject.name);
                }
            }
            return result;
        }

        // Main fucntion to move the interactable to position/rotation for Grab / Snap and Focus
        public IEnumerator MoveInteractable(GameObject Item, Transform Start, Transform End, float duration, bool skipMove = false)
        {
            StartCoroutine(TimerGrab(MoveToHandSpeed));
            // if we are snapping
            if (End.tag == Item.GetComponent<FPS_Interactable>().TargetName)
            {
                //if there are some advanced snapping fucntions
                if (End.GetComponent<Snap_Actions>() != null)
                {
                    print("Snapping has advanced functions");
                    // set variables for FBI
                    End.GetComponent<Snap_Actions>().FPS_Inter = this;
                    End.GetComponent<Snap_Actions>().VinylGO = null;// Item.GetComponentInChildren<FBI.Vinyle_Actions>().gameObject;
                    End.GetComponent<Snap_Actions>().SleeveGO = Item;
                    End.GetComponent<Snap_Actions>().Init_Snap();
                    skipMove = true;
                }
            }
            if (!skipMove)
            {
                print("Moving interactable from " + Start.gameObject.name + " to " + End.gameObject.name);
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
            // If We are grabbing
            if (End == Attachement && InteractableAtReach != null && InteractableInHand == null)
            {
                print(" > end of moving object to Grab");
                // test if we are grabbing from a snap
                if (Start.tag == Item.GetComponent<FPS_Interactable>().TargetName)
                {
                    // test if Snap has a function
                    if (Start.GetComponent<Snap_Actions>() != null)
                    {
                        print("Unsapping Process");
                        Start.GetComponent<Snap_Actions>().UnSnapping();
                    }
                }
                InteractableAtReach.GetComponent<FPS_Interactable>().OnInteract(Attachement);
                InteractableInHand = InteractableAtReach;
                isInteractableInHand = true;
                InteractableAtReach = null;

            }
            // If we are Snapping
            if (End.tag == Item.GetComponent<FPS_Interactable>().TargetName)
            {
                print(" > end of moving object to Snapping");
                InteractableInHand.GetComponent<FPS_Interactable>().CurrentSnap = End.gameObject;
                InteractableInHand.GetComponent<FPS_Interactable>().OnInteract();
                InteractableInHand = null;
                InteractableAtReach = null;
                isInteractableInHand = false;
                // test if Snap has a function
                if(End.GetComponent<Snap_Actions>() !=null)
                {
                    End.GetComponent<Snap_Actions>().Snapping(Item);
                }
            }
            // If we are Focusing
            if (End == FocusInputMan.FocusTargetTransform)
            {
                print(" > end of moving object to Focus");
                if (InteractableInHand.GetComponent<FPS_Interactable>().canFocusOn)
                    FocusActive = true;
                else
                    FocusActive = false;
            }
            //end
            yield return null;
        }

        IEnumerator TimedDrop(float duration)
        {
            inProcess = true;
            yield return new WaitForSeconds(duration/4f);
            print("Dropping " + InteractableInHand.name);
            InteractableInHand.GetComponent<FPS_Interactable>().OnInteract();
            // update data and clean-up
            isInteractableInHand = false;
            InteractableInHand = null;
            inProcess = false;
        }

        IEnumerator TimerGrab(float duration)
        {
            inProcess = true;
            yield return new WaitForSeconds(duration * 2);
            inProcess = false;
        }

        private void ResetStatus()
        {
            inProcess = false;
            InteractableAtReach = null;
            InteractableUIAtReach = null;
        }

        private void ResetStatusDrop()
        {
            InteractableInHand = null;
            inProcess = false;
            InteractableAtReach = null;
            InteractableUIAtReach = null;
            isInteractableInHand = false;
        }
    }
}