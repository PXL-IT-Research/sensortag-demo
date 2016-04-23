using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagLib.UnitTests
{
    [TestFixture]
    public class SensorTagTests
    {
        [Test]
        public void SensorTag_Create()
        {
            // TODO: Add your test code here
            var sensor = new SensorTag();
            Assert.NotNull(sensor);
        }
    }
}
