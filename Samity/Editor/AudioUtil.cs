#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Samity
{
    internal static class AudioUtil
    {
        public static void PlayClipPreview(AudioClip clip, float volumeScale = 1f)
        {
            // TODO: sometimes the audio just... won't play? starting playmode will fix it, but not sure the repro.
            PhantomAudioComponent.Instance.Source.Stop();
            PhantomAudioComponent.Instance.Source.PlayOneShot(clip, volumeScale);
        }
    }
}
#endif