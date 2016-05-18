using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Robotics.Mobile.Core.Bluetooth.LE;
using SensorTagLib;
using Syncfusion.SfGauge.XForms;

namespace SensorTagDemo
{
    public partial class TISensorConnectPage : ContentPage
    {
        private IAdapter _adapter;
        private SensorTag sensorTag;

        public TISensorConnectPage(IAdapter adapter)
        {
            InitializeComponent();

            InitializeGauge();

            _adapter = adapter;
        }

        private void InitializeGauge()
        {
            SfCircularGauge gauge = new SfCircularGauge();
            Header header = new Header();
            header.Text = "Temperature";
            header.TextSize = 20;
            header.Position = new Point(0.5, 0.7);
            header.ForegroundColor = Color.Gray;
            gauge.Headers.Add(header);

            var scale = new Syncfusion.SfGauge.XForms.Scale();
            scale.LabelColor = Color.Gray;
            scale.LabelOffset = 0.1;
            scale.StartValue = -40;
            scale.EndValue = 60;
            scale.Interval = 10;
            scale.StartAngle = 135;
            scale.SweepAngle = 270;
            scale.RimThickness = 5;
            scale.RimColor = Color.Teal;
            scale.MinorTicksPerInterval = 0;
            gauge.Scales.Add(scale);

            //var range = new Range();
            //range.StartValue = 0;
            //range.EndValue = 80;
            //range.Color = Color.FromHex("#FF777777");
            //range.Thickness = 10;
            //scale.Ranges.Add(range);

            var needlePointer = new NeedlePointer();
            needlePointer.Value = 60;
            needlePointer.Color = Color.White;
            needlePointer.KnobColor = Color.White;
            needlePointer.Thickness = 3;
            needlePointer.KnobRadius = 20;
            needlePointer.LengthFactor = 0.8;
            scale.Pointers.Add(needlePointer);

            TheStack.Children.Add(gauge);
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
