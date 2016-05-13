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
        private SensorTag sensorTag;

        public TISensorConnectPage(IAdapter adapter)
        {
            InitializeComponent();
            _adapter = adapter;
        }

        public async void ConnectButton_Click(object sender, EventArgs args)
        {
            Debug.WriteLine("Start Scanning");

            ConnectButton.IsEnabled = false;

            sensorTag = await SensorTagFactory.FindSensorTag(_adapter);
            bool status = await sensorTag.ConnectAsync();

            StatusLabel.Text = "Found: " + sensorTag.Name + ", \nID: " + sensorTag.ID;
            StatusLabel.BackgroundColor = Color.Lime;
            
            ConnectButton.IsEnabled = true;

            Debug.WriteLine("Found: " + sensorTag.Name);
            Debug.WriteLine("Device ID: " + sensorTag.ID);

            BindingContext = sensorTag;
        }

        public void TemperatureButton_Click(object sender, EventArgs args)
        {
            sensorTag.StartTemperatureSensing();
        }
    }
}
