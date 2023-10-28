using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace FPS_Controller
{
    public class FPS_PromptZone : MonoBehaviour
    {
        public GameObject Prompt;
        public GameObject SelectedItem;

        public EventSystem ES;

        private void Start()
        {
            if (ES == null)
                ES = FindObjectOfType<EventSystem>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.ToString() == "Player")
            {
                print(" Prompt " + Prompt.name + " ON");
                if (ES != null)
                {
                    //ES.SetSelectedGameObject(SelectedItem);
                }
                else
                    Debug.Log("Can't find the eventsystem");
                Prompt.SetActive(true);
            }

        }
        private void OnTriggerExit(Collider other)
        {
            print(" Prompt " + Prompt.name + " OFF");
            if (ES != null)
            {
                //ES.SetSelectedGameObject(null);
            }
            else
                Debug.Log("Can't find the eventsystem");
            Prompt.SetActive(false);
        }
    }
}