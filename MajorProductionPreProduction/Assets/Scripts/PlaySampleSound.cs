using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySampleSound : MonoBehaviour
{
    public AudioSource sampleSound;

    public void PlaySound()
    {
        if(sampleSound != null && sampleSound.clip != null)
        {
            sampleSound.Play();

        }

    }
}
