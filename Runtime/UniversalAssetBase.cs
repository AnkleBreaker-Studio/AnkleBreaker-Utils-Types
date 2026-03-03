using System;
using UnityEngine;

namespace AnkleBreaker.Utils.UniversalTypes
{
    /// <summary>
    /// Base class for UniversalAsset&lt;T&gt;. Provides shared mode field.
    /// Use <see cref="UniversalAsset{T}"/> or <see cref="UniversalSprite"/> in your scripts.
    /// </summary>
    [Serializable]
    public abstract class UniversalAssetBase
    {
        public enum AssetMode
        {
            Direct = 0,
#if AB_ADDRESSABLES
            Addressable = 1,
#endif
        }

        [SerializeField] private AssetMode mode = AssetMode.Direct;

        // ─── Properties ─────────────────────────────────────────────

        public AssetMode Mode => mode;

        /// <summary>
        /// Returns true if no value is assigned for the current mode.
        /// </summary>
        public abstract bool IsEmpty { get; }
    }
}