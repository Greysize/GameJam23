using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] FootstepsSound;
    public AudioSource FootASound;
    private bool isPlaying;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground" && !isPlaying)
        {
            StartCoroutine(PlayFootStep());
        }
    }

    private IEnumerator PlayFootStep()
    {
        isPlaying = true;
        int index = Random.Range(0, FootstepsSound.Length);
        AudioClip CurAudio = FootstepsSound[index];
        float CurLenght = CurAudio.length;
        FootASound.clip = CurAudio;
        FootASound.Play();
        float t = 0f;
        while(t <= CurLenght)
        {
            t += Time.deltaTime;
            yield return 0;
        }
        isPlaying = false;
        yield return null;
    }
}
