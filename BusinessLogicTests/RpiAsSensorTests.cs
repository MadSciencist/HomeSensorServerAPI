using NUnit.Framework;
using RpiAsSensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicTests
{
    public class RpiAsSensorTests
    {
        [Test]
        public void TemperatureReaderParserReturnProperValue()
        {
            var stdOut = "temp=44.5'C";
            var expectedResult = "44.5";

            var reader = new TemperatureReader();
            var result = reader.ParseTemperature(stdOut);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
