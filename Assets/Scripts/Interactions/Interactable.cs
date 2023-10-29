using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isUsed = false;
    public UnityEvent ActionEvent;
    public UnityEvent CancelEvent;
    public bool isTimed = false;
    public float timerlenght = 3f;
    public void OnInteract()
    {
        if (!isUsed)
        {
            isUsed = true;
            ActionEvent.Invoke();
            if (isTimed)
                StartCoroutine(TimedActionnable());
        }
    }

    public IEnumerator TimedActionnable()
    {
        float t = 0f;
        while(t <= timerlenght)
        {
            t += Time.deltaTime;
            yield return 0;
        }
        CancelEvent.Invoke();
        isUsed = false;
        yield return null;
    }
}
