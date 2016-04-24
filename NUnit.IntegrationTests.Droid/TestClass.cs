using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorTagLib;
using Robotics.Mobile.Core.Bluetooth.LE;

namespace NUnit.IntegrationTests.SenorTagLib
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public async Task TestMethod()
        {
            // Arrange
            var adapter = new Robotics.Mobile.Core.Bluetooth.LE.Adapter();

            // Act
            IDevice device = await SensorTagFactory.FindSensorTag(adapter);
            var sensortag = new SensorTag(device);

            // Assert
            Assert.IsNotNull(device);
        }
    }
}
