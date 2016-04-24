using System;
using System.Collections.Generic;

namespace SensorTagLib
{
    public class DeviceNotFoundException : Exception
    {
        public DeviceNotFoundException(string msg) : base(msg)
        { }
    }
}