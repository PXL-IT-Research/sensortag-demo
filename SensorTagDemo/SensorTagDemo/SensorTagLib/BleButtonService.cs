using Robotics.Mobile.Core.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagLib
{
    /// <summary>
    /// This class provides access to the SensorTag button (key) data.  
    /// </summary>
    public class BleButtonService : BleGenericGattService
    {
        public static Guid ButtonServiceUuid = Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb");
        public static Guid ButtonCharacteristicUuid = Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb");

        private ICharacteristic _characteristic;

        public BleButtonService(IAdapter adapter, IService service)
            : base(adapter, service)
        {
            _service.CharacteristicsDiscovered += (object sender, EventArgs e) =>
            {
                _characteristic = _service.Characteristics.First(); // only one for buttons
                Debug.WriteLine("Characteristic discovered: " + _characteristic.Name);
            };

            _service.DiscoverCharacteristics();

            if (_characteristic != null)
            {
                if (_characteristic.CanUpdate)
                {
                    _characteristic.ValueUpdated += (object sender, CharacteristicReadEventArgs e) =>
                    {
                        Debug.WriteLine("Update: " + e.Characteristic.Value);

                        //Todo: make human readable and throw event up
                    };

                    _characteristic.StartUpdates();
                }
            }
        }
    }

    public class SensorButtonEventArgs : EventArgs
    {
        private byte bits;

        public SensorButtonEventArgs(byte bits, DateTimeOffset timestamp)
        {
            this.bits = bits;
            Timestamp = timestamp;
        }

        public bool LeftButtonDown
        {
            get { return (bits & 0x2) == 0x2; }
        }

        public bool RightButtonDown
        {
            get { return (bits & 0x1) == 0x1; }
        }

        public DateTimeOffset Timestamp
        {
            get;
            private set;
        }
    }
}
