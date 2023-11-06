using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoLoader : MonoBehaviour
{
    public float delay;
    public UnityEvent autoLoad;
    private void Awake()
    {
        StartCoroutine(Delay() );
    }

    private IEnumerator Delay()
    {

        yield return new WaitForSeconds(delay);
        print("load level");
        autoLoad.Invoke();
        yield return null;
    }
}
