using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    public AudioSource speaker;
    public AudioClip autoSwitch,forceSwitch,buttonClick,newHighScore,friendlyTouch,hostileTouch;
    // Start is called before the first frame update
    private void Start()
    {
        speaker=GetComponent<AudioSource>();
    }
    public void Play(AudioClip clip)
    {
        speaker.clip=clip;
        speaker.PlayOneShot(clip);
    }
}
