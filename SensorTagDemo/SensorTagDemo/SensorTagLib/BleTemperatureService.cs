using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robotics.Mobile.Core.Bluetooth.LE;
using System.Diagnostics;

namespace SensorTagLib
{
    public class BleTemperatureService : BleGenericGattService
    {
        public static Guid IRTemperatureServiceUuid = Guid.Parse("f000aa00-0451-4000-b000-000000000000");
        static Guid IRTemperatureCharacteristicUuid = Guid.Parse("f000aa01-0451-4000-b000-000000000000");
        static Guid IRTemperatureCharacteristicConfigUuid = Guid.Parse("f000aa02-0451-4000-b000-000000000000");
        static Guid IRTemperatureCharacteristicPeriodUuid = Guid.Parse("f000aa03-0451-4000-b000-000000000000");

        private ICharacteristic _temperatureChar;
        private ICharacteristic _temperatureCharConfig;
        private ICharacteristic _temperatureCharPeriod;

        public delegate void TemperatureStatusChangedHandler(object sender, TemperatureEventArgs args);
        public event TemperatureStatusChangedHandler TemperatureStatusChanged;

        /// <summary>
        /// This class provides access to the SensorTag temperature data.  
        /// </summary>
        public BleTemperatureService(IAdapter adapter, IService service)
            : base(adapter, service)
        {
            _service.CharacteristicsDiscovered += (object sender, EventArgs e) =>
            {
                foreach (var characteristic in _service.Characteristics)
                {
                    if (characteristic.ID == IRTemperatureCharacteristicUuid)
                    {
                        Debug.WriteLine("Characteristic discovered: " + characteristic.Name);
                        _temperatureChar = characteristic;
                    }
                    if (characteristic.ID == IRTemperatureCharacteristicConfigUuid)
                    {
                        Debug.WriteLine("Characteristic discovered: " + characteristic.Name);
                        _temperatureCharConfig = characteristic;
                    }
                    if (characteristic.ID == IRTemperatureCharacteristicPeriodUuid)
                    {
                        Debug.WriteLine("Characteristic discovered: " + characteristic.Name);
                        _temperatureCharPeriod = characteristic;
                    }
                }
            };

            _service.DiscoverCharacteristics();

            if (_temperatureChar != null)
            {
                if (_temperatureChar.CanUpdate)
                {
                    _temperatureChar.ValueUpdated += (object sender, CharacteristicReadEventArgs e) =>
                    {
                        double ambient, infrared;
                        CalculateTemperature(out ambient, out infrared);
                    };                
                }
            }
        }

        public void StartSensing()
        {
            _temperatureCharConfig.Write(new byte[] { 0x01 }); // Turn ON
            _temperatureChar.StartUpdates();
        }

        // copied from https://github.com/conceptdev/xamarin-forms-samples/tree/master/BluetoothTISensor
        //private void CalculateTemperature(out double ambient, out double ir)
        //{
        //    var sensorData = _temperatureChar.Value;
        //    // Temperature sensorTMP006 - works
        //    var ambientTemperature = BitConverter.ToUInt16(sensorData, 2) / 128.0;
        //    double Tdie = ambientTemperature + 273.15;

        //    // http://sensortag.codeplex.com/SourceControl/latest#SensorTagLibrary/SensorTagLibrary/Source/Sensors/IRTemperatureSensor.cs
        //    double Vobj2 = BitConverter.ToInt16(sensorData, 0);
        //    Vobj2 *= 0.00000015625;

        //    double S0 = 5.593E-14;
        //    double a1 = 1.75E-3;
        //    double a2 = -1.678E-5;
        //    double b0 = -2.94E-5;
        //    double b1 = -5.7E-7;
        //    double b2 = 4.63E-9;
        //    double c2 = 13.4;
        //    double Tref = 298.15;
        //    double S = S0 * (1 + a1 * (Tdie - Tref) + a2 * Math.Pow((Tdie - Tref), 2));
        //    double Vos = b0 + b1 * (Tdie - Tref) + b2 * Math.Pow((Tdie - Tref), 2);
        //    double fObj = (Vobj2 - Vos) + c2 * Math.Pow((Vobj2 - Vos), 2);
        //    double tObj = Math.Pow(Math.Pow(Tdie, 4) + (fObj / S), .25);

        //    tObj -= 273.15;

        //    Debug.WriteLine("ambient: " + Math.Round(ambientTemperature, 1) + "\nIR: " + Math.Round(tObj, 1) + " C");
        //    ambient = ambientTemperature;
        //    ir = tObj;
        //}

        private void CalculateTemperature(out double ambient, out double ir)
        {
            var data = _temperatureChar.Value;

            short objTemp = (short)((short)data[0] + (short)(data[1] << 8));
            short dieTemp = (short)((short)data[2] + (short)(data[3] << 8));

            const double SCALE_LSB = 0.03125;
            int it;

            it = (int)((objTemp) >> 2);
            ir = (double)it * SCALE_LSB;

            it = (int)((dieTemp) >> 2);
            ambient = (double)it * SCALE_LSB;

            Debug.WriteLine("ambient: " + ambient + "\nIR: " + ir + " C");
            TemperatureStatusChanged(this, new TemperatureEventArgs(ir, DateTime.Now));
        }

    }

    public class TemperatureEventArgs : EventArgs
    {
        public TemperatureEventArgs(double infrared, DateTimeOffset timestamp)
        {
            Infrared = infrared;
            Timestamp = timestamp;
        }

        public DateTimeOffset Timestamp
        {
            get;
            private set;
        }

        public double Infrared
        {
            get;
            private set;
        }
    }
}
