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
        }
    }
}
