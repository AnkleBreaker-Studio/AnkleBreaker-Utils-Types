using NUnit.Framework;
using AnkleBreaker.Utils.UniversalTypes;

namespace AnkleBreaker.Utils.UniversalTypes.Tests
{
    public class UniversalTypesTests
    {
        // ─── UniversalString ────────────────────────────────────────

        [Test]
        public void UniversalString_Default_IsNotLocalized()
        {
            var us = new UniversalString();
            Assert.IsFalse(us.IsLocalized);
        }

        [Test]
        public void UniversalString_Default_IsEmpty()
        {
            var us = new UniversalString();
            Assert.IsTrue(us.IsEmpty);
        }

        [Test]
        public void UniversalString_Default_ToStringReturnsEmpty()
        {            var us = new UniversalString();
            Assert.AreEqual("", us.ToString());
        }

        [Test]
        public void UniversalString_ImplicitConversion_ReturnsToString()
        {
            var us = new UniversalString();
            string result = us;
            Assert.AreEqual("", result);
        }

        [Test]
        public void UniversalString_EqualityOperator_WithString()
        {
            var us = new UniversalString();
            Assert.IsTrue(us == "");
            Assert.IsFalse(us != "");
            Assert.IsTrue("" == us);
        }

        [Test]
        public void UniversalString_PlainText_IsAccessible()
        {
            var us = new UniversalString();
            Assert.AreEqual("", us.PlainText);
        }

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
        public void UniversalSound_DefaultAudioClip_IsNull()
        {
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
        public void UniversalSprite_DefaultMode_IsDirect()
        {
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

#if AB_ADDRESSABLES
        [Test]
        public void UniversalSprite_AddressableMode_Exists()
        {            var mode = UniversalAssetBase.AssetMode.Addressable;
            Assert.AreEqual(1, (int)mode);
        }
#endif

        // ─── Enum Stability ────────────────────────────────────────

        [Test]
        public void EnumValues_AreStable()
        {
            Assert.AreEqual(0, (int)UniversalSound.SoundMode.AudioClip);
            Assert.AreEqual(0, (int)UniversalAssetBase.AssetMode.Direct);
        }
    }
}