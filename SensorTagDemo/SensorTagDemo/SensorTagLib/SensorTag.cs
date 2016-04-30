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
    /// Following in the spirit of https://github.com/clovett/SensorTag-for-Windows
    /// This class combines all of the GATT services provided by SensorTag into one helper class.
    /// See http://processors.wiki.ti.com/index.php/CC2650_SensorTag_User's_Guide#IR_Temperature_Sensor
    /// for details on GATT services.
    /// Under the covers we will use https://github.com/xamarin/Monkey.Robotics
    /// </summary>
    public class SensorTag
    {
        private IDevice _device;
        private IAdapter _adapter;

        private BleButtonService _buttonService;

        public SensorTag(IDevice device, IAdapter adapter)
        {
            this._device = device;
            this._adapter = adapter;
            Name = _device.Name;
            ID = _device.ID;
        }

        public string Name { get; set; }
        public Guid ID { get; set; }

        /// <summary>
        /// Connect or reconnect to the device.
        /// </summary>
        /// <returns></returns>
        public Task<bool> ConnectAsync()
        {
            bool status = false;
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            
            _adapter.DeviceConnected += (sender, e) =>
            {
                _device = e.Device;
                
                // when services are discovered
                _device.ServicesDiscovered += (object se, EventArgs ea) => {
                    foreach (var service in _device.Services)
                    {
                        if (service.ID == BleButtonService.ButtonServiceUuid)
                        {
                            Debug.WriteLine("Found BleButtonService");
                            _buttonService = new BleButtonService();
                        }

                    }
                };

                // start looking for services
                _device.DiscoverServices();
            };

            _adapter.ConnectToDevice(_device);
            tcs.SetResult(status);
            
            return tcs.Task;
        }

    }
}
