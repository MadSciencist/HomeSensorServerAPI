using NUnit.Framework;
using RpiProcessHandler.FFmpeg;
using System;

namespace BusinessLogicTests
{
    public class RpiProcessHandlerUnitTests
    {

        [Test]
        public void ResolutionDictionaryReturnsProperValue()
        {
            var expectedResult = "640x480";
            var result = Resolution.GetResolution(EResolution.Res640x480);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
