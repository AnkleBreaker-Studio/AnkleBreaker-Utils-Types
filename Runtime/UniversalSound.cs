using System;
using UnityEngine;

namespace AnkleBreaker.Utils.UniversalTypes
{
    /// <summary>
    /// Universal sound reference that supports AudioClip, Wwise events, and FMOD events.
    /// Stores data for each audio system without requiring compile-time dependencies.
    /// 
    /// Usage example:
    /// <code>
    /// switch (mySound.Mode)
    /// {
    ///     case UniversalSound.SoundMode.AudioClip:
    ///         audioSource.PlayOneShot(mySound.AudioClip);
    ///         break;
    ///     case UniversalSound.SoundMode.Wwise:
    ///         // AkSoundEngine.PostEvent(mySound.WwiseEventName, gameObject);
    ///         break;
    ///     case UniversalSound.SoundMode.FMOD:
    ///         // RuntimeManager.PlayOneShot(mySound.FMODEventPath);
    ///         break;
    /// }
    /// </code>
    /// </summary>
    [Serializable]
    public class UniversalSound
    {
        public enum SoundMode
        {
            AudioClip,
            Wwise,
            FMOD
        }

        [SerializeField] private SoundMode mode = SoundMode.AudioClip;
        [SerializeField] private AudioClip audioClip;

        /// <summary>Wwise event name (e.g. "Play_UI_Click").</summary>
        [SerializeField] private string wwiseEventName = "";

        /// <summary>FMOD event path (e.g. "event:/UI/Click").</summary>
        [SerializeField] private string fmodEventPath = "";

        // ─── Properties ─────────────────────────────────────────────

        public SoundMode Mode => mode;
        public AudioClip AudioClip => audioClip;
        public string WwiseEventName => wwiseEventName;
        public string FMODEventPath => fmodEventPath;

        /// <summary>
        /// Returns true if no value is assigned for the current mode.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                switch (mode)
                {
                    case SoundMode.AudioClip:
                        return audioClip == null;
                    case SoundMode.Wwise:
                        return string.IsNullOrEmpty(wwiseEventName);
                    case SoundMode.FMOD:
                        return string.IsNullOrEmpty(fmodEventPath);
                    default:
                        return true;
                }
            }
        }
    }
}