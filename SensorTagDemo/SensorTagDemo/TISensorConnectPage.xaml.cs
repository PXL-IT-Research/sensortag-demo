using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Robotics.Mobile.Core.Bluetooth.LE;
using SensorTagLib;

namespace SensorTagDemo
{
    public partial class TISensorConnectPage : ContentPage
    {
        private IAdapter _adapter;

        public TISensorConnectPage(IAdapter adapter)
        {
            InitializeComponent();
            _adapter = adapter;

            //_adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
        }

        public async void ConnectButton_Click(object sender, EventArgs args)
        {
            Debug.WriteLine("Start Scanning");
            IDevice device = await SensorTagFactory.FindSensorTag(_adapter);

            var sensortag = await SensorTagFactory.ConnectSensorTag(device, _adapter);

            StatusLabel.Text = "Found: " + device.Name + ", ID: " + device.ID;
            Debug.WriteLine("Found: " + device.Name);
            Debug.WriteLine("Device ID: " + device.ID);
        }
    }
}
