using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SensorTagDemo
{
    /// <summary>
    /// Makes the error go away:
    /// https://forums.xamarin.com/discussion/41950/initializecomponent-does-not-exist-in-the-current-context
    /// </summary>
    public partial class DummyPage : ContentPage
    {
        public DummyPage()
        {
            InitializeComponent();
        }
    }
}
