using Robotics.Mobile.Core.Bluetooth.LE;
using System;
using System.Collections.Generic;
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

        public BleButtonService(IAdapter adapter, IService service)
            : base(adapter, service)
        {}

        Delegate _buttonValueChanged;

        public event EventHandler<SensorButtonEventArgs> ButtonValueChanged
        {
            add
            {
                if (_buttonValueChanged != null)
                {
                    _buttonValueChanged = Delegate.Combine(_buttonValueChanged, value);
                }
                else
                {
                    _buttonValueChanged = value;
                    RegisterForValueChangeEvents(ButtonCharacteristicUuid);
                }
            }
            remove
            {
                if (_buttonValueChanged != null)
                {
                    _buttonValueChanged = Delegate.Remove(_buttonValueChanged, value);
                    if (_buttonValueChanged == null)
                    {
                        UnregisterForValueChangeEvents(ButtonCharacteristicUuid);
                    }
                }
            }
        }

        private void OnButtonValueChanged(SensorButtonEventArgs args)
        {
            if (_buttonValueChanged != null)
            {
                ((EventHandler<SensorButtonEventArgs>)_buttonValueChanged)(this, args);
            }
        }

        //public async Task<bool> ConnectAsync(string deviceContainerId)
        //{
        //    return await this.ConnectAsync(ButtonServiceUuid, deviceContainerId);
        //}

        //protected override void OnCharacteristicValueChanged(GattCharacteristic sender, GattValueChangedEventArgs eventArgs)
        //{
        //    if (sender.Uuid == ButtonCharacteristicUuid)
        //    {
        //        if (_buttonValueChanged != null)
        //        {
        //            uint dataLength = eventArgs.CharacteristicValue.Length;
        //            using (DataReader reader = DataReader.FromBuffer(eventArgs.CharacteristicValue))
        //            {
        //                if (dataLength == 1)
        //                {
        //                    byte bits = reader.ReadByte();

        //                    OnButtonValueChanged(new SensorButtonEventArgs(bits, eventArgs.Timestamp));
        //                }
        //            }
        //        }
        //    }
        //}

        public void UnregisterForValueChangeEvents(Guid guid)
        {
            foreach (var characteristic in GetSafeCharacteristics())
            {
                if (characteristic.ID == guid)
                {
                    UnregisterCharacteristic(characteristic);
                    break;
                }
            }
            this._requestedCharacteristics.Remove(guid);
        }

        ICharacteristic[] GetSafeCharacteristics()
        {
            ICharacteristic[] temp;
            if (_characteristics == null)
            {
                return new ICharacteristic[0];
            }
            lock (_characteristics)
            {
                temp = _characteristics.ToArray();
            }
            return temp;
        }

        private void UnregisterCharacteristic(ICharacteristic characteristic)
        {
            CharacteristicPropertyType properties = characteristic.Properties;
            if ((properties & CharacteristicPropertyType.Notify) != 0)
            {
                // stop notifying.
                characteristic.ValueUpdated -= OnCharacteristicValueChanged;
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
