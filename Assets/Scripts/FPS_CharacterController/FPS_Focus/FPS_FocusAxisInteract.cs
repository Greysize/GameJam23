using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace FPS_Controller
{
    public class FPS_FocusAxisInteract : MonoBehaviour
    {
        public bool CanInteract;
        [Header("Focus Item Type")]
        //public bool isFocusItem;
        public GameObject Item;
        public float OrbitSensitivity;
        [Header("Focus Search Type")]
        //public bool isFocusSearch;
        public GameObject SearchKnob;
        [Header("Event")]
        public UnityEvent OnInteract;
        public Vector2 AxisInput;

        private float OrbitX;
        private float OrbitY;

        public void ReceiveInput(Vector2 input)
        {
            AxisInput = input;
            OnInteract.Invoke();
            if (Item != null)
            {
                OrbitItem();
            }
            if (SearchKnob != null)
            {
                SearchSwipe();
            }
        }

        private void OrbitItem()
        {
            Vector3 targetRotation = Item.transform.eulerAngles;

            OrbitX = (AxisInput.y * OrbitSensitivity);
            OrbitY = (AxisInput.x * OrbitSensitivity);

            targetRotation.x = targetRotation.x + OrbitX;
            targetRotation.y = targetRotation.y + OrbitY;

            Item.transform.Rotate(Vector3.right, OrbitX * Time.deltaTime);
            Item.transform.Rotate(Vector3.up, OrbitY * Time.deltaTime);
            //Item.transform.rotation = Quaternion.Euler(targetRotation);
        }

        private void SearchSwipe()
        {
            Vector3 T = SearchKnob.transform.position;
            T.x = T.x + AxisInput.x;

            SearchKnob.transform.position = new Vector3(T.x, T.y, T.z);
        }
    }
}