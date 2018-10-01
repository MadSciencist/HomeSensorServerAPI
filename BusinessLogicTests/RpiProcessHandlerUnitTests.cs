using NUnit.Framework;
using RpiProcesses.FFmpeg;
using RpiProcesses.Rtsp;
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

        [Test]
        public void ResolutionDictionaryReturnsInvalidValue()
        {
            var expectedResult = "640x480";
            var result = Resolution.GetResolution(EResolution.Res720x480);

            Assert.AreNotEqual(expectedResult, result);
        }

        [Test]
        public void RtspUrlCredentialsInjection()
        {
            var url = @"rtsp://192.168.0.80:554/axis-media/media.amp";
            var login = "root";
            var password = "password";

            var expectedResult = @"rtsp://root:password@192.168.0.80:554/axis-media/media.amp";

            var result = RtspHelper.InjectCredentialsToUrl(url, login, password);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void RtspUrlCredentialsInjectionNoCredentials()
        {
            var url = @"rtsp://192.168.0.80:554/axis-media/media.amp";
            var login = "";
            var password = "";

            var expectedResult = @"rtsp://192.168.0.80:554/axis-media/media.amp";

            var result = RtspHelper.InjectCredentialsToUrl(url, login, password);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
