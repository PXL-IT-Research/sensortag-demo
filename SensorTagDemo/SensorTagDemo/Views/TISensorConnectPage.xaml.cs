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
        }

        public async void ConnectButton_Click(object sender, EventArgs args)
        {
            Debug.WriteLine("Start Scanning");

            ConnectButton.IsEnabled = false;

            SensorTag stag = await SensorTagFactory.FindSensorTag(_adapter);
            bool status = await stag.ConnectAsync();

            StatusLabel.Text = "Found: " + stag.Name + ", \nID: " + stag.ID;
            StatusLabel.BackgroundColor = Color.Lime;
            
            ConnectButton.IsEnabled = true;

            Debug.WriteLine("Found: " + stag.Name);
            Debug.WriteLine("Device ID: " + stag.ID);
        }
    }
}
