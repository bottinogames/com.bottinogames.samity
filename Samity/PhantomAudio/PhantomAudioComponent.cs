// If you manage to get this script onto an object in a build, I fear for your soul.
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * ===========================================================
 * |              This is not a place of honor.              |
 * |      No highly esteemed deed is commemorated here.      |
 * |                 Nothing valued is here.                 |
 * |     What is here was dangerous and repulsive to us.     |
 * ===========================================================
 * 
 * This is some truly cursed this to get around the fact that 
 * I cannot seem to find another way to play audio in editor.
 */

namespace Samity
{
    [AddComponentMenu("")] // empty string will prevent the script from being added manually
    internal class PhantomAudioComponent : MonoBehaviour
    {
        private static PhantomAudioComponent _instance;
        public static PhantomAudioComponent Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("The Phantom Menace");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    _instance = go.AddComponent<PhantomAudioComponent>();
                    _instance.Source = go.AddComponent<AudioSource>();

                    _instance.Source.bypassEffects = true;
                    _instance.Source.bypassListenerEffects = true;
                    _instance.Source.bypassReverbZones = true;
                    _instance.Source.playOnAwake = false;

                    Debug.Log("New Phantom created.");
                }
                return _instance;
            }
        }

        public AudioSource Source { get; private set; }

        private void Awake()
        {
            Source = GetComponent<AudioSource>();
            if (_instance == null)
                _instance = this;
            else if (Application.isPlaying)
                Destroy(this.gameObject);
            else
                DestroyImmediate(this.gameObject);
        }

        [UnityEditor.MenuItem("Help/Clear Phantom Audio")]
        public static void ClearPhantomAudio()
        {
            if (_instance == null)
                return;
            
            if (Application.isPlaying)
                Destroy(_instance.gameObject);
            else
                DestroyImmediate(_instance.gameObject);
        }
    }

}
#endif