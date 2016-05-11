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

        public delegate void ButtonStatusChangedHandler(object sender, SensorButtonEventArgs args);
        public event ButtonStatusChangedHandler ButtonStatusChanged;

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
                        var status = Decode(e.Characteristic.Value);
                        Debug.WriteLine("Update: " + e.Characteristic.Value);
                        Debug.WriteLine("Decoded: " + status);
                        if (ButtonStatusChanged != null)
                            ButtonStatusChanged(this, new SensorButtonEventArgs(status, DateTime.Now));
                    };

                    _characteristic.StartUpdates();
                }
            }
        }

        private ButtonStatus Decode(byte[] value)
        {
            // Smart Keys: Bit 2 - side key, Bit 1 - right key, Bit 0 – left key - works
            var b = ((int)value[0]) % 4;
            ButtonStatus output = ButtonStatus.NoButtonDown;
            switch (b)
            {
                case 1:
                    output = ButtonStatus.RightButtonDown;
                    break;
                case 2:
                    output = ButtonStatus.LeftButtonDown;
                    break;
                case 3:
                    output = ButtonStatus.BothButtonsDown;
                    break;
                default:
                    output = ButtonStatus.NoButtonDown;
                    break;
            };
            return output;
        }
    }

    public class SensorButtonEventArgs : EventArgs
    {
        public SensorButtonEventArgs(ButtonStatus status, DateTimeOffset timestamp)
        {
            Status = status;
            Timestamp = timestamp;
        }

        public DateTimeOffset Timestamp
        {
            get;
            private set;
        }

        public ButtonStatus Status
        {
            get;
            private set;
        }
    }

    public enum ButtonStatus
    {
        LeftButtonDown,
        RightButtonDown,
        BothButtonsDown,
        NoButtonDown
    }
}
