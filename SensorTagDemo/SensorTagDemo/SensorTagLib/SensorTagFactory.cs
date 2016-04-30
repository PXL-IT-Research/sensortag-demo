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
    /// Scans for and retrieves the sensortag
    /// </summary>
    public class SensorTagFactory
    {
        public static Task<SensorTag> FindSensorTag(IAdapter adapter)
        {
            TaskCompletionSource<SensorTag> tcs = new TaskCompletionSource<SensorTag>();
            IDevice sensortagDevice = null;

            adapter.DeviceDiscovered += (sender, e) =>
            {
                if (e.Device.Name != null)
                {
                    if (e.Device.Name.ToUpper().Contains("SENSORTAG"))
                    {
                        sensortagDevice = e.Device;
                        // this event triggers multiple times, so set the result only once
                        var sensorTag = new SensorTag(sensortagDevice, adapter);
                        tcs.TrySetResult(sensorTag);
                    }
                }
            };

            adapter.ScanTimeoutElapsed += (sender, e) =>
            {
                adapter.StopScanningForDevices();
                if (sensortagDevice == null)
                {
                    tcs.SetException(new DeviceNotFoundException("SenorTag not found!"));
                }
            };

            if (adapter.IsScanning) // stop with previous scans
            {
                adapter.StopScanningForDevices();
                sensortagDevice = null;
            }

            adapter.StartScanningForDevices(Guid.Empty);
           
            return tcs.Task;
        }

        public static Task<SensorTag> ConnectSensorTag(IDevice device, IAdapter adapter)
        {
            TaskCompletionSource<SensorTag> tcs = new TaskCompletionSource<SensorTag>();
            Guid ButtonServiceUuid = Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb");

            adapter.DeviceConnected += (sender, e) =>
            {
                device = e.Device;
                var sensorTag = new SensorTag(device, adapter);

                // when services are discovered
                device.ServicesDiscovered += (object se, EventArgs ea) => {
                    Debug.WriteLine("device.ServicesDiscovered");
                    foreach (var service in device.Services)
                    {
                        Debug.WriteLine("Service: " + service.ID);
                        if (service.ID == ButtonServiceUuid)
                        {
                            Debug.WriteLine("THIS THING HAS A BUTTON!");
                        }
                        
                    }
                    tcs.SetResult(sensorTag);
                };
                
                // start looking for services
                device.DiscoverServices();
            };

            adapter.ConnectToDevice(device);

            return tcs.Task;
        }
    }
}
