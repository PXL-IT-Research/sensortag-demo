using GalaSoft.MvvmLight;
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
    public class SensorTag : ObservableObject
    {
        private IDevice _device;
        private IAdapter _adapter;

        private BleButtonService _buttonService;
        private BleTemperatureService _temperatureService;
        
        public SensorTag(IDevice device, IAdapter adapter)
        {
            this._device = device;
            this._adapter = adapter;
            Name = _device.Name;
            ID = _device.ID;
        }

        public string Name { get; set; }
        public Guid ID { get; set; }

        private ButtonStatus _buttonStatus;
        public ButtonStatus ButtonStatus
        {
            get
            {
                return _buttonStatus;
            }
            set
            {
                Set<ButtonStatus>(ref _buttonStatus, value);
            }
        }

        private double _infraredTemperature;
        public double InfraredTemperature
        {
            get
            {
                return _infraredTemperature;
            }
            set
            {
                Set<double>(ref _infraredTemperature, value);
            }
        }

        public void StartTemperatureSensing()
        {
            _temperatureService.StartSensing();
        }


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

                DiscoverServicesAsync();
                tcs.SetResult(status);
            };

            _adapter.ConnectToDevice(_device);
           
            
            return tcs.Task;
        }

        private void DiscoverServicesAsync()
        {
            //bool status = false;
            //TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            // when services are discovered
            _device.ServicesDiscovered += (object se, EventArgs ea) =>
            {
                foreach (var service in _device.Services)
                {
                    if (service.ID == BleButtonService.ButtonServiceUuid)
                    {
                        Debug.WriteLine("Found BleButtonService");
                        _buttonService = new BleButtonService(_adapter, service);
                        _buttonService.ButtonStatusChanged += ButtonService_ButtonStatusChanged;

                    }

                    if (service.ID == BleTemperatureService.IRTemperatureServiceUuid)
                    {
                        Debug.WriteLine("Found BleTemperatureService");
                        _temperatureService = new BleTemperatureService(_adapter, service);
                        _temperatureService.TemperatureStatusChanged += TemperatureService_TemperatureStatusChanged;
                    }
                }
            };

            // start looking for services
            _device.DiscoverServices();
        }

        private void TemperatureService_TemperatureStatusChanged(object sender, TemperatureEventArgs args)
        {
            InfraredTemperature = args.Infrared;
        }

        private void ButtonService_ButtonStatusChanged(object sender, SensorButtonEventArgs args)
        {
            ButtonStatus = args.Status;
        }
    }
}
