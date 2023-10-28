using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Sticks : MonoBehaviour
{

    public GameObject RightStick;
    public GameObject LeftStick;
    private IEnumerator RS;
    private IEnumerator LS;

    // Start is called before the first frame update


    public void OnFocusSticks()
    {
        print("shake");
        // shake sticks
        if (RS != null)
            StopCoroutine(RS);
        if(LS != null)
            StopCoroutine(LS);
        RS = ShakeSticks(RightStick);
        LS = ShakeSticks(LeftStick);
        StartCoroutine(RS);
        StartCoroutine(LS);
    }

    IEnumerator ShakeSticks(GameObject Stick)
    {
        print("Shake " + Stick.name);
        Stick.GetComponent<FPS_Controller.FPS_Interactable>().inProcess = true;
        Stick.GetComponent<CapsuleCollider>().enabled = true;
        var StickAnim = Stick.GetComponent<Animator>();
        StickAnim.enabled = true;
        StickAnim.Play("Baguette_Tap");
        yield return new WaitForSeconds(0.33f);
        StickAnim.enabled = false;
        Stick.GetComponent<CapsuleCollider>().enabled = false;
        Stick.GetComponent<FPS_Controller.FPS_Interactable>().inProcess = false;
        print("End corout");
        yield return null;
    }
}
