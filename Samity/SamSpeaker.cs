using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
[RequireComponent(typeof(AudioSource))]
public class SamSpeaker : MonoBehaviour
{
    public Samity.SamVoice voice;

    private AudioSource _source;
    public AudioSource Source 
    { 
        get 
        { 
            if(_source == null)
                _source = GetComponent<AudioSource>();
            return _source;
                
        } 
    }

    public void Speak(string text)
    {
        if (voice == null)
            return;
        AudioClip clip = voice.GetClip(text);
        Source.PlayOneShot(clip);
    }
}
