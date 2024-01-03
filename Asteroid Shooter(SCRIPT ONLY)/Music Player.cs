using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System.Reflection;
using DG.Tweening;
public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> track;
    public AudioSource speaker;
    public AudioClip activeTrack { get; private set; }
    public bool autoPlay;
    public enum Mode 
    {
        SUFFLE,
        REPEAT,
        SEQUENTIAL
    }
    public Mode mode=Mode.SEQUENTIAL;

    public void MPDebug(AudioClip clip)
    {
        Debug.Log(clip.name);
    }
    public void PlayAlbum(string name)
    {
        MusicPlayer mp = transform.Find(name).GetComponent<MusicPlayer>();
        mp.speaker = speaker;
        mp.AutoPlay();
        //mp.track.ForEach(MPDebug);
    }
    public void Play(int index)
    {
        CancelInvoke(nameof(AutoPlay));
        activeTrack = track[index];
        speaker.clip = activeTrack;
        speaker.Play();
        if(autoPlay)
            Invoke(nameof(AutoPlay),activeTrack.length);
    }

    public void Play(string name)
    {
        CancelInvoke(nameof(AutoPlay));
        activeTrack = track.Find(n => n.name.Contains(name));
        speaker.clip = activeTrack;
        speaker.Play();
        if (autoPlay)
            Invoke(nameof(AutoPlay), activeTrack.length);
    }

    public void Play(AudioClip clip)
    {
        CancelInvoke(nameof(AutoPlay));
        activeTrack=track[track.IndexOf(clip)];          
        speaker.clip = activeTrack;
        speaker.Play();
        if (autoPlay)
            Invoke(nameof(AutoPlay), activeTrack.length);
    }

    public void Next()
    {
        CancelInvoke(nameof(AutoPlay));
        var v = track.IndexOf(activeTrack) + 1;
        if (v>track.Count-1)
            v = 0;
        activeTrack = track[v];
        speaker.Play();
        if (autoPlay)
            Invoke(nameof(AutoPlay), activeTrack.length);
    }

    public void Previous()
    {
        CancelInvoke(nameof(AutoPlay));
        var v = track.IndexOf(activeTrack) - 1;
        if(v<0)
            v = track.Count - 1;
        activeTrack = track[v];
        speaker.Play();
        if (autoPlay)
            Invoke(nameof(AutoPlay), activeTrack.length);
    }

    public void AutoPlay()
    {
        switch (mode)
        {
            case Mode.SEQUENTIAL:
                {
                    Next();
                    break;
                }
            case Mode.REPEAT:
                {
                    Play(activeTrack);
                    break;
                }
            case Mode.SUFFLE:
                {
                    Play(Random.Range(0, track.Count));
                    break;
                }
        }
    }

}
