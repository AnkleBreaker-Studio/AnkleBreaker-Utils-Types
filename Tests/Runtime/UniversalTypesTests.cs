using NUnit.Framework;
using AnkleBreaker.Utils.UniversalTypes;

namespace AnkleBreaker.Utils.UniversalTypes.Tests
{
    public class UniversalTypesTests
    {
        // ─── UniversalString ────────────────────────────────────────

        [Test]
        public void UniversalString_DefaultMode_IsPlainText()
        {
            var us = new UniversalString();
            Assert.AreEqual(UniversalString.StringMode.PlainText, us.Mode);
        }

        [Test]
        public void UniversalString_Default_IsEmpty()
        {
            var us = new UniversalString();
            Assert.IsTrue(us.IsEmpty);
        }

        [Test]
        public void UniversalString_DefaultGetRawValue_ReturnsEmptyString()
        {
            var us = new UniversalString();            Assert.AreEqual("", us.GetRawValue());
        }

        [Test]
        public void UniversalString_ImplicitConversion_ReturnsRawValue()
        {
            var us = new UniversalString();
            string result = us;
            Assert.AreEqual("", result);
        }

        [Test]
        public void UniversalString_ToString_ReturnsRawValue()
        {
            var us = new UniversalString();
            Assert.AreEqual("", us.ToString());
        }

#if AB_I2_LOCALIZE
        [Test]
        public void UniversalString_I2Mode_Exists()
        {
            // Verify I2Localize enum value is available
            var mode = UniversalString.StringMode.I2Localize;
            Assert.AreEqual(1, (int)mode);
        }
#endif
#if AB_UNITY_LOCALIZATION
        [Test]
        public void UniversalString_UnityLocMode_Exists()
        {
            var mode = UniversalString.StringMode.UnityLocalization;
            Assert.AreEqual(2, (int)mode);
        }
#endif

        // ─── UniversalSound ────────────────────────────────────────

        [Test]
        public void UniversalSound_DefaultMode_IsAudioClip()
        {
            var sound = new UniversalSound();
            Assert.AreEqual(UniversalSound.SoundMode.AudioClip, sound.Mode);
        }

        [Test]
        public void UniversalSound_Default_IsEmpty()
        {
            var sound = new UniversalSound();
            Assert.IsTrue(sound.IsEmpty);
        }

        [Test]
        public void UniversalSound_DefaultAudioClip_IsNull()        {
            var sound = new UniversalSound();
            Assert.IsNull(sound.AudioClip);
        }

#if AB_WWISE
        [Test]
        public void UniversalSound_WwiseMode_Exists()
        {
            var mode = UniversalSound.SoundMode.Wwise;
            Assert.AreEqual(1, (int)mode);
        }
#endif

#if AB_FMOD
        [Test]
        public void UniversalSound_FMODMode_Exists()
        {
            var mode = UniversalSound.SoundMode.FMOD;
            Assert.AreEqual(2, (int)mode);
        }
#endif

        // ─── UniversalAsset / UniversalSprite ──────────────────────

        [Test]
        public void UniversalSprite_DefaultMode_IsDirect()        {
            var sprite = new UniversalSprite();
            Assert.AreEqual(UniversalAssetBase.AssetMode.Direct, sprite.Mode);
        }

        [Test]
        public void UniversalSprite_Default_IsEmpty()
        {
            var sprite = new UniversalSprite();
            Assert.IsTrue(sprite.IsEmpty);
        }

        [Test]
        public void UniversalSprite_DefaultDirectReference_IsNull()
        {
            var sprite = new UniversalSprite();
            Assert.IsNull(sprite.DirectReference);
        }

        [Test]
        public void UniversalSprite_GetDirectValue_ReturnsNull()
        {
            var sprite = new UniversalSprite();
            Assert.IsNull(sprite.GetDirectValue());
        }

#if AB_ADDRESSABLES
        [Test]
        public void UniversalSprite_AddressableMode_Exists()
        {            var mode = UniversalAssetBase.AssetMode.Addressable;
            Assert.AreEqual(1, (int)mode);
        }
#endif

        // ─── Enum Stability ────────────────────────────────────────

        [Test]
        public void EnumValues_AreStable_AcrossIfBlocks()
        {
            // PlainText, AudioClip, Direct should always be 0
            Assert.AreEqual(0, (int)UniversalString.StringMode.PlainText);
            Assert.AreEqual(0, (int)UniversalSound.SoundMode.AudioClip);
            Assert.AreEqual(0, (int)UniversalAssetBase.AssetMode.Direct);
        }
    }
}