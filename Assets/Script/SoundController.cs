using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    public AudioSource audioSourceOnce;
    public AudioSource audioSourceLoop;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //Play single sound once
    public void PlaySound(AudioClip clip)
    {
        audioSourceOnce.loop = false;
        audioSourceOnce.PlayOneShot(clip);
    }

    //Play sound on loop
    public void PlaySoundLoop(AudioClip clip)
    {
        audioSourceLoop.loop = true;
        audioSourceLoop.clip = clip;
        audioSourceLoop.Play();
    }

    //Stop looping sound
    public void StopLoopSound()
    {
        audioSourceLoop.Stop();
    }
}
