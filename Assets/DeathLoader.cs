using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DeathLoader : MonoBehaviour
{
    public float delay;
    private void Awake()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {

        yield return new WaitForSeconds(delay);
        print("load level");
        SceneManager.LoadScene("Level");
        yield return null;
    }
}
