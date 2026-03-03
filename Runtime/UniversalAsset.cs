using System;
using UnityEngine;
#if AB_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace AnkleBreaker.Utils.UniversalTypes
{
    /// <summary>
    /// Universal asset reference that supports direct references and Addressable keys.
    /// Addressable mode is only available when com.unity.addressables is installed.
    /// </summary>
    /// <typeparam name="T">The asset type (must inherit from UnityEngine.Object).</typeparam>
    [Serializable]
    public class UniversalAsset<T> : UniversalAssetBase where T : UnityEngine.Object
    {
        [SerializeField] private T directReference;

#if AB_ADDRESSABLES
        [SerializeField] private AssetReference addressableRef;

        /// <summary>
        /// Addressable asset reference. Use with Addressables.LoadAssetAsync&lt;T&gt;().
        /// Only available when com.unity.addressables is installed.
        /// </summary>
        public AssetReference AddressableReference => addressableRef;
#endif

        // ─── Properties ─────────────────────────────────────────────

        /// <summary>Direct asset reference. Only valid when Mode is Direct.</summary>
        public T DirectReference => directReference;

        /// <summary>
        /// Returns the direct reference if mode is Direct, null otherwise.
        /// For Addressable mode, use AddressableReference with Addressables.LoadAssetAsync&lt;T&gt;().
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
#if AB_ADDRESSABLES
                    case AssetMode.Addressable:
                        return addressableRef == null || !addressableRef.RuntimeKeyIsValid();
#endif
                    default:
                        return true;
                }
            }
        }

        public static implicit operator T(UniversalAsset<T> ua) => ua?.GetDirectValue();
    }
}