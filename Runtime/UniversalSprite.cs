using System;
using UnityEngine;

namespace AnkleBreaker.Utils.Types
{
    /// <summary>
    /// Universal sprite reference. Supports direct Sprite reference or Addressable key.
    /// Convenience alias for <see cref="UniversalAsset{T}"/> with T = Sprite.
    /// </summary>
    [Serializable]
    public class UniversalSprite : UniversalAsset<Sprite> { }
}