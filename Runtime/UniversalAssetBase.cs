using System;
using UnityEngine;

namespace AnkleBreaker.Utils.UniversalTypes
{
    /// <summary>
    /// Base class for UniversalAsset&lt;T&gt;. Provides shared mode and addressable key fields.
    /// Use <see cref="UniversalAsset{T}"/> or <see cref="UniversalSprite"/> in your scripts.
    /// </summary>
    [Serializable]
    public abstract class UniversalAssetBase
    {
        public enum AssetMode
        {
            Direct,
            Addressable
        }

        [SerializeField] private AssetMode mode = AssetMode.Direct;

        /// <summary>
        /// Addressable asset key or address (e.g. "Assets/Sprites/Icon.png" or a custom address).
        /// Used when mode is Addressable. Load via Addressables.LoadAssetAsync&lt;T&gt;(addressableKey).
        /// </summary>
        [SerializeField] private string addressableKey = "";

        // ─── Properties ─────────────────────────────────────────────

        public AssetMode Mode => mode;
        public string AddressableKey => addressableKey;

        /// <summary>
        /// Returns true if no value is assigned for the current mode.
        /// </summary>
        public abstract bool IsEmpty { get; }
    }
}