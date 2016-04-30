using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagLib
{
    /// <summary>
    /// This class provides access to the SensorTag button (key) data.  
    /// </summary>
    public class BleButtonService : BleGenericGattService
    {
        public static Guid ButtonServiceUuid = Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb");
        public static Guid ButtonCharacteristicUuid = Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb");

        public BleButtonService()
        {
        }


    }
}
