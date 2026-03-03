using System;
using UnityEngine;
#if AB_FMOD
using FMODUnity;
#endif

namespace AnkleBreaker.Utils.UniversalTypes
{
    /// <summary>
    /// Universal sound reference supporting AudioClip, Wwise events, and FMOD events.
    /// Available modes depend on which audio middleware is installed.
    /// 
    /// Usage example:
    /// <code>
    /// switch (mySound.Mode)
    /// {
    ///     case UniversalSound.SoundMode.AudioClip:
    ///         audioSource.PlayOneShot(mySound.AudioClip);
    ///         break;
    /// #if AB_WWISE
    ///     case UniversalSound.SoundMode.Wwise:
    ///         mySound.WwiseEvent.Post(gameObject);
    ///         break;
    /// #endif
    /// #if AB_FMOD
    ///     case UniversalSound.SoundMode.FMOD:
    ///         RuntimeManager.PlayOneShot(mySound.FMODEvent);
    ///         break;
    /// #endif
    /// }
    /// </code>
    /// </summary>
    [Serializable]
    public class UniversalSound
    {
        public enum SoundMode
        {
            AudioClip = 0,
#if AB_WWISE
            Wwise = 1,
#endif
#if AB_FMOD
            FMOD = 2,
#endif
        }

        [SerializeField] private SoundMode mode = SoundMode.AudioClip;
        [SerializeField] private AudioClip audioClip;

#if AB_WWISE
        [SerializeField] private AK.Wwise.Event wwiseEvent;
#endif

#if AB_FMOD
        [SerializeField] private EventReference fmodEvent;
#endif

        // ─── Properties ─────────────────────────────────────────────

        public SoundMode Mode => mode;
        public AudioClip AudioClip => audioClip;

#if AB_WWISE
        /// <summary>Wwise Event reference. Call .Post(gameObject) to play.</summary>
        public AK.Wwise.Event WwiseEvent => wwiseEvent;
#endif

#if AB_FMOD
        /// <summary>FMOD Event reference. Use with RuntimeManager.PlayOneShot().</summary>
        public EventReference FMODEvent => fmodEvent;
#endif

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
#if AB_WWISE
                    case SoundMode.Wwise:
                        return wwiseEvent == null || !wwiseEvent.IsValid();
#endif
#if AB_FMOD
                    case SoundMode.FMOD:
                        return fmodEvent.IsNull;
#endif
                    default:
                        return true;
                }
            }
        }
    }
}