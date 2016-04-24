using Robotics.Mobile.Core.Bluetooth.LE;
using System;
using System.Collections.Generic;
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
        public static Task<IDevice> FindSensorTag(IAdapter adapter)
        {
            TaskCompletionSource<IDevice> tcs = new TaskCompletionSource<IDevice>();
            IDevice sensortagDevice = null;

            adapter.DeviceDiscovered += (sender, e) =>
            {
                if (e.Device.Name.ToUpper().Contains("SENSORTAG"))
                {
                    sensortagDevice = e.Device;
                    // this event triggers multiple times, so set the result only once
                    tcs.TrySetResult(sensortagDevice);
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
    }
}
