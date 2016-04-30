using Robotics.Mobile.Core.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SensorTagDemo
{
    public partial class DemoStartPage : TabbedPage
    {
        private IAdapter _adapter;

        public DemoStartPage(IAdapter adapter)
        {
            InitializeComponent();

            _adapter = adapter;

            Children.Add(new TISensorConnectPage(adapter));
        }

        public IAdapter Adapter
        {
            get
            {
                return _adapter;
            }
            private set { }
        }
    }
}
