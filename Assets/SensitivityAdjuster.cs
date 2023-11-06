using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityAdjuster : MonoBehaviour
{
    public GameObject cameraObject;
    vThirdPersonCamera cameraManager;
    [SerializeField] GameObject mainUI;

    Slider thisSlider;

    public void Start()
    {
        cameraManager = cameraObject.GetComponent<vThirdPersonCamera>();
        thisSlider = GetComponent<Slider>();
        thisSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    void ValueChangeCheck()
    {
        print("Value changing sensitivity");
        float sensitivity = (thisSlider.value * 12) + 1;
        print ("Sensitivity = " + sensitivity);
        cameraManager.xMouseSensitivity = sensitivity;
        cameraManager.yMouseSensitivity = sensitivity;
    }

    public void OnDone()
    {
        mainUI.SetActive(false);
    }
}