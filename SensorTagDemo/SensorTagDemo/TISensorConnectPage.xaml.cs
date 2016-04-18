using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Robotics.Mobile.Core.Bluetooth.LE;

namespace SensorTagDemo
{
    public partial class TISensorConnectPage : ContentPage
    {
        private IAdapter _adapter;

        public TISensorConnectPage(IAdapter adapter)
        {
            InitializeComponent();
            _adapter = adapter;

            _adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
        }

        private void Adapter_DeviceDiscovered(object sender, DeviceDiscoveredEventArgs e)
        {
            Debug.WriteLine("Discovered: " + e.Device);
        }

        public void ConnectButton_Click(object sender, EventArgs args)
        {
            Debug.WriteLine("Start Scanning");
            _adapter.StartScanningForDevices(Guid.Empty);
        }
    }
}
