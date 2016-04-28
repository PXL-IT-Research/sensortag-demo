using Robotics.Mobile.Core.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagLib
{
    /// <summary>
    /// Following in the spirit of https://github.com/clovett/SensorTag-for-Windows
    /// This class combines all of the GATT services provided by SensorTag into one helper class.
    /// See http://processors.wiki.ti.com/index.php/CC2650_SensorTag_User's_Guide#IR_Temperature_Sensor
    /// for details on GATT services.
    /// Under the covers we will use https://github.com/xamarin/Monkey.Robotics
    /// </summary>
    public class SensorTag
    {
        private IDevice _device;

        public SensorTag(IDevice device)
        {
            this._device = device;
            Name = _device.Name;
            ID = _device.ID;
        }

        public string Name { get; set; }
        public Guid ID { get; set; }

    }
}
