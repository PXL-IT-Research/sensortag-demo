using Robotics.Mobile.Core.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagLib
{
    public abstract class BleGenericGattService
    {
        protected IService _service;
        protected List<ICharacteristic> _characteristics;
        protected IDevice _connectedDevice;
        protected IAdapter _adapter;
        protected bool _connected;
        protected bool _disconnecting;
        protected HashSet<Guid> _requestedCharacteristics = new HashSet<Guid>();

        public BleGenericGattService(IAdapter adapter, IService service)
        {
            _adapter = adapter;
            _service = service;
        }

        protected void RegisterForValueChangeEvents(Guid guid)
        {
            _requestedCharacteristics.Add(guid);

            if (_service == null || !this._connected)
            {
                // wait till we are connected...
                return;
            }
            try
            {
                ICharacteristic characteristic = _service.Characteristics
                    .Where((c) => c.ID == guid)
                    .FirstOrDefault();

                if (characteristic == null)
                {
                    //TODO: throw this event
                    // OnError("Characteristic " + guid + " not available");
                    Debug.WriteLine("Characteristic " + guid + " not available");
                    return;
                }

                CharacteristicPropertyType properties = characteristic.Properties;
                
                Debug.WriteLine("Characteristic {0} supports: ", guid.ToString("b"));
                if ((properties & CharacteristicPropertyType.Broadcast) != 0)
                {
                    Debug.WriteLine("    Broadcast");
                }
                if ((properties & CharacteristicPropertyType.Read) != 0)
                {
                    Debug.WriteLine("    Read");
                }
                if ((properties & CharacteristicPropertyType.WriteWithoutResponse) != 0)
                {
                    Debug.WriteLine("    WriteWithoutResponse");
                }
                //if ((properties & CharacteristicPropertyType.) != 0)
                //{
                //    Debug.WriteLine("    Write");
                //}
                if ((properties & CharacteristicPropertyType.Notify) != 0)
                {
                    Debug.WriteLine("    Notify");
                }
                if ((properties & CharacteristicPropertyType.Indicate) != 0)
                {
                    Debug.WriteLine("    Indicate");
                }
                if ((properties & CharacteristicPropertyType.AuthenticatedSignedWrites) != 0)
                {
                    Debug.WriteLine("    AuthenticatedSignedWrites");
                }
                if ((properties & CharacteristicPropertyType.ExtendedProperties) != 0)
                {
                    Debug.WriteLine("    ExtendedProperties");
                }
                //if ((properties & CharacteristicPropertyType.ReliableWrites) != 0)
                //{
                //    Debug.WriteLine("    ReliableWrites");
                //}
                //if ((properties & CharacteristicPropertyType.WritableAuxiliaries) != 0)
                //{
                //    Debug.WriteLine("    WritableAuxiliaries");
                //}

                if ((properties & CharacteristicPropertyType.Notify) != 0)
                {
                    try
                    {
                        characteristic.ValueUpdated -= OnCharacteristicValueChanged;
                    }
                    catch { }

                    //QueueAsyncEnableNotification(characteristic);

                }
                else
                {
                    Debug.WriteLine("Registering for notification on characteristic that doesn't support notification: " + guid);
                    //OnError("Registering for notification on characteristic that doesn't support notification: " + guid);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unhandled exception registering for notifications. " + ex.Message);
                //OnError("Unhandled exception registering for notifications. " + ex.Message);
            }
        }

        protected virtual void OnCharacteristicValueChanged(object sender,
                                                            CharacteristicReadEventArgs args)
        {
            // handled by subclasses
        }

       
    }
}
