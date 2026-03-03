using NUnit.Framework;
using AnkleBreaker.Utils.UniversalTypes;

namespace AnkleBreaker.Utils.UniversalTypes.Tests
{
    public class UniversalTypesTests
    {
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
            var us = new UniversalString();
            Assert.AreEqual("", us.GetRawValue());
        }

        [Test]
        public void UniversalString_ImplicitConversion_ReturnsRawValue()
        {
            var us = new UniversalString();
            string result = us;
            Assert.AreEqual("", result);
        }

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
    }
}