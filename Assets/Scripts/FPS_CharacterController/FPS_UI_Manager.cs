using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace FPS_Controller
{
    public class FPS_UI_Manager : MonoBehaviour
    {
        public FPS_InputManager FPS_Manager;
        public GameObject GUI_Point;


        public void InGameUIVisible(bool state)
        {
            GUI_Point.SetActive(state);
        }

    }
}