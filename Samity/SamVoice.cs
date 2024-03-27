using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SamSharp;

namespace Samity
{
    [CreateAssetMenu(fileName = "Sam Voice", menuName = "Sam Voice", order = 216)]
    public class SamVoice : ScriptableObject
    {
        public byte Pitch { get { return pitch; } set { pitch = value; UpdateSamOptions(); } }
        public byte Mouth { get { return mouth; } set { mouth = value; UpdateSamOptions(); } }
        public byte Throat { get { return throat; } set { throat = value; UpdateSamOptions(); } }
        public byte Speed { get { return speed; } set { speed = value; UpdateSamOptions(); } }
        public bool SingMode { get { return singMode; } set { singMode = value; UpdateSamOptions(); } }


        [SerializeField, Range(0, 255)] private byte pitch = 64;
        [SerializeField, Range(0, 255)] private byte mouth = 128;
        [SerializeField, Range(0, 255)] private byte throat = 128;
        [SerializeField, Range(0, 255)] private byte speed = 72;
        [SerializeField] private bool singMode = false;

        [Tooltip("Adds extra samples at the end of the audio clip to smooth out harsh popping.")]
        [SerializeField] private bool addPadding = true;

        [Tooltip("Text that will have its audio pre-generated and cached when this ScriptableObject loads.")]
        [SerializeField] internal PreCacheClip[] preCache;

        // Only enable-able through debug inspector, to keep people from turning it on accidentally. Mostly only for debugging SamSharp.
        [SerializeField, HideInInspector] private bool enableLogging = false; 
        
        private Sam sam = new Sam(new Options());

        private Dictionary<string, AudioClip> cachedTextClips = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> cachedPhoneticClips = new Dictionary<string, AudioClip>();

        #region Public Methods

        /// <summary>
        /// Get an AudioClip of SAM speaking <paramref name="text"/>.
        /// Will return a cached clip if available, otherwise will generate a new clip.
        /// Generated clips will NOT be added to the cache.
        /// </summary>
        /// <param name="text">The text to be spoken.</param>
        /// <param name="phonetic">Whether the text has already be split into SAM phonetics. Leave false if plain-text.</param>
        /// <returns>An audio clip containing the SAM speach.</returns>
        public AudioClip GetClip(string text, bool phonetic = false)
        {
            Dictionary<string, AudioClip> cache = phonetic ? cachedPhoneticClips : cachedTextClips;
            if (cache.TryGetValue(text, out AudioClip output))
                return output;

            output = GetUniqueClip(text, phonetic);
            return output;
        }

        /// <summary>
        /// Get an AudioClip of SAM speaking <paramref name="text"/>.
        /// Will always generate a new audio clip and will not interact with the clip cache.
        /// </summary>
        /// <param name="text">The text to be spoken.</param>
        /// <param name="phonetic">Whether the text has already be split into SAM phonetics. Leave false if plain-text.</param>
        /// <returns>An audio clip containing the SAM speach.</returns>
        public AudioClip GetUniqueClip(string text, bool phonetic = false)
        {
            // TODO: Do something about the console spam. Maybe hide it behind pre-compile flag?
            // TODO: seems like some characters (at least `) will cause a failer and a chaining nullref. needs investigation
            byte[] rawData = phonetic ? sam.SpeakPhonetic(text) : sam.Speak(text);

            // TODO: Modify SamSharp to calculate float samples to begin with, this is a bit absurd
            float[] samples;
            if (!addPadding)
            {
                samples = new float[rawData.Length];
                for (int i = 0; i < rawData.Length; i++)
                    samples[i] = rawData[i] / 255f;
            }
            else
            {
                const int padding = 180;
                samples = new float[rawData.Length + padding];
                float last = rawData[^1] / 255f;
                for (int i = 0; i < padding; i++)
                {
                    float t = i / (float)padding;
                    samples[^(i+1)] = t * last;
                }
                for (int i = 0; i < rawData.Length; i++)
                    samples[i] = rawData[i] / 255f;
            }
            // TODO: Investigate why samples are at 22050 Hz. Should this change?
            AudioClip clip = AudioClip.Create($"{this.name}(SAM) - {text}", samples.Length, 1, 22050, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// Get an AudioClip of SAM speaking <paramref name="text"/>.
        /// Will return a cached clip if available, otherwise will generate a new clip.
        /// Newly generated clips will be added to the cache for future retreival.
        /// Use this function if the given <paramref name="text"/> will be repeated.
        /// </summary>
        /// <param name="text">The text to be spoken.</param>
        /// <param name="phonetic">Whether the text has already be split into SAM phonetics. Leave false if plain-text.</param>
        /// <returns>An audio clip containing the SAM speach.</returns>
        public AudioClip GetCachedClip(string text, bool phonetic = false)
        {
            Dictionary<string, AudioClip> cache = phonetic ? cachedPhoneticClips : cachedTextClips;
            if (cache.TryGetValue(text, out AudioClip output))
                return output;

            output = GetUniqueClip(text, phonetic);
            cache.Add(text, output);

            return output;
        }

        /// <summary>
        /// Clears all references to cached clips.
        /// </summary>
        public void ClearCache()
        {
            cachedTextClips.Clear();
            cachedPhoneticClips.Clear();
        }

        #endregion

        #region Private Methods
        private void UpdateSamOptions()
        {
            sam.Options = new Options(pitch, mouth, throat, speed, singMode);
            sam.loggingEnabled = enableLogging;
            ClearCache(); // clear the cache of the old voice settings
        }

        private void GeneratePreCache()
        {
            if (preCache == null) return;

            for (int i = 0; i < preCache.Length; i++)
                _ = GetCachedClip(preCache[i].text, preCache[i].phonetic);
        }


        [ContextMenu("Enable Logging")] private void EnableLogging() { enableLogging = true; sam.loggingEnabled = enableLogging; }
        [ContextMenu("Disable Logging")] private void DisableLogging() { enableLogging = false; sam.loggingEnabled = enableLogging; }
        #endregion

        #region Unity Messages
        private void OnEnable()
        {
            UpdateSamOptions();
            if (Application.isPlaying)
                GeneratePreCache();
        }

        private void OnValidate()
        {
            UpdateSamOptions();
        }
        #endregion

        [System.Serializable]
        internal struct PreCacheClip
        {
            public string text;
            public bool phonetic;
        }
    }
}