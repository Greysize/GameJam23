using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform InteractorOrigin;
    public bool canInteract = false;
    public float Raydistance = 2f;
    public string InteractButton = "Fire1";

    private Camera Cam;
    private bool isTestNeeded = false;

    private void Start()
    {
        Cam = FindObjectOfType<Camera>();
    }
    private void Update()
    {
        if (Input.GetButtonDown(InteractButton))
            InteractionTest();
    }

    private void InteractionTest()
    {
        Ray rayInteract = new Ray(InteractorOrigin.transform.position, Cam.transform.forward);
        RaycastHit hitData;
        Debug.DrawRay(InteractorOrigin.transform.position, Cam.transform.forward, Color.red);

        if (Physics.Raycast(rayInteract, out hitData, Raydistance))
        {
            if(hitData.collider.gameObject.GetComponent<Interactable>())
            {
                hitData.collider.gameObject.GetComponent<Interactable>().OnInteract();
            }
        }
    }
}
