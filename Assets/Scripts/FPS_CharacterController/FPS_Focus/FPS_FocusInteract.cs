using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace FPS_Controller
{
    public class FPS_FocusInteract : MonoBehaviour
    {
        public bool CanInteract;
        public bool CanCancel;

        [Header("Events")]
        public UnityEvent OnInteract;
        public UnityEvent OnCancel;

        private FPS_FocusInputManager FocusMan;

        public void Start()
        {
            FocusMan = FindObjectOfType<FPS_FocusInputManager>();
        }

        public void OnInteractPressed()
        {
            if (CanInteract && FocusMan.isFocusActive)
                OnInteract.Invoke();
        }

        public void OnCancelPressed()
        {
            if (CanCancel && FocusMan.isFocusActive)
            {
                FocusMan.EndFocus();
                OnCancel.Invoke();
            }
        }
    }
}