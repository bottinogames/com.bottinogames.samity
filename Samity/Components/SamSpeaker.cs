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
            if (_source == null)
            {
                _source = GetComponent<AudioSource>();
                if (_source == null)
                    throw new System.NullReferenceException($"Sam Speaker ({gameObject.name}) could not find AudioSource via GetComponent.");
            }
            return _source;
        }
    }


    /// <summary>
    /// Plays a SAM audio clip based on text given. The clip is set as the audiosource clip and then played.
    /// </summary>
    /// <param name="text">The text to be spoken, in plain english.</param>
    /// <returns>The audio clip played, null if there is no voice to generate it.</returns>
    public AudioClip Speak(string text)
    {
        if (voice == null)
            return null;
        AudioClip clip = voice.GetClip(text);
        Source.Stop();
        Source.clip = clip;
        Source.Play();
        return clip;
    }

    /// <summary>
    /// Plays a SAM audio clip based on text given. The clip is played as a OneShot (see AudioSource.PlayOneShot).
    /// </summary>
    /// <param name="text">The text to be spoken, in plain english.</param>
    /// <returns>The audio clip played, null if there is no voice to generate it.</returns>
    public AudioClip SpeakOneShot(string text)
    {
        if (voice == null)
            return null;
        AudioClip clip = voice.GetClip(text);
        Source.PlayOneShot(clip);
        return clip;
    }

    /// <summary>
    /// Plays a SAM audio clip based on text given. The clip is set as the audiosource clip and then played.
    /// </summary>
    /// <param name="text">The text to be spoken, in SAM phonetics.</param>
    /// <returns>The audio clip played, null if there is no voice to generate it.</returns>
    public AudioClip SpeakPhonetic(string text)
    {
        if (voice == null)
            return null;
        AudioClip clip = voice.GetClip(text);
        Source.Stop();
        Source.clip = clip;
        Source.Play();
        return clip;
    }

    /// <summary>
    /// Plays a SAM audio clip based on text given. The clip is played as a OneShot (see AudioSource.PlayOneShot).
    /// </summary>
    /// <param name="text">The text to be spoken, in SAM phonetics.</param>
    /// <returns>The audio clip played, null if there is no voice to generate it.</returns>
    public AudioClip SpeakPhoneticOneShot(string text)
    {
        if (voice == null)
            return null;
        AudioClip clip = voice.GetClip(text);
        Source.PlayOneShot(clip);
        return clip;
    }
}
