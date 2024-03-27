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
        public static void PlayClipPreview(AudioClip clip, float volume = 1f)
        {
            PhantomAudioComponent.PlayClip(clip, volume);
        }
    }
}
#endif