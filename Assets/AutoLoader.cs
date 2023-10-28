using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoLoader : MonoBehaviour
{
    public UnityEvent autoLoad;
    private void Awake()
    {
       autoLoad.Invoke();
    }
}
