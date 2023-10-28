using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace FPS_Controller
{
    public class FPS_PromptActions : MonoBehaviour
    {
        [Header("Player")]
        public FPS_InputManager FPS_Manager;
        public FPS_Interact FPS_Interactions;

        [Header("Events")]
        public UnityEvent OnPromptOpen;
        public UnityEvent OnPromptClose;


        private void Start()
        {
            FPS_Manager = FindObjectOfType<FPS_InputManager>();
            FPS_Interactions = FindObjectOfType<FPS_Interact>();
            if (FPS_Manager == null)
                Debug.Log(" No Player controller found !");
            if (FPS_Interactions == null)
                Debug.Log("No Interact Manager Found !");
        }

        public void OnEnterPrompt()
        {
            FPS_Interactions.ActivePrompt = this;
            OnPromptOpen.Invoke();
        }


        public void OnExitPrompt()
        {
            FPS_Manager.isKinematic = true;
            FPS_Interactions.ActivePrompt = null;
            OnPromptClose.Invoke();
        }

    }
}