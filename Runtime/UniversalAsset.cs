using System;
using UnityEngine;

namespace AnkleBreaker.Utils.Types
{
    /// <summary>
    /// Universal asset reference that supports both direct references and Addressable keys.
    /// For Addressable mode, use AddressableKey with Addressables.LoadAssetAsync&lt;T&gt;().
    /// </summary>
    /// <typeparam name="T">The asset type (must inherit from UnityEngine.Object).</typeparam>
    [Serializable]
    public class UniversalAsset<T> : UniversalAssetBase where T : UnityEngine.Object
    {
        [SerializeField] private T directReference;

        // ─── Properties ─────────────────────────────────────────────

        /// <summary>Direct asset reference. Only valid when Mode is Direct.</summary>
        public T DirectReference => directReference;

        /// <summary>
        /// Returns the direct reference if mode is Direct, null otherwise.
        /// For Addressable mode, use AddressableKey with Addressables.LoadAssetAsync&lt;T&gt;().
        /// </summary>
        public T GetDirectValue()
        {
            return Mode == AssetMode.Direct ? directReference : null;
        }

        public override bool IsEmpty
        {
            get
            {
                switch (Mode)
                {
                    case AssetMode.Direct:
                        return directReference == null;
                    case AssetMode.Addressable:
                        return string.IsNullOrEmpty(AddressableKey);
                    default:
                        return true;
                }
            }
        }

        public static implicit operator T(UniversalAsset<T> ua) => ua?.GetDirectValue();
    }
}