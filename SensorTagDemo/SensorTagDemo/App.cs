using Robotics.Mobile.Core.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SensorTagDemo
{
    public class App : Application
    {
        static IAdapter Adapter;

        public App()
        {
            // The root page of your application
            MainPage = new DemoStartPage(Adapter);
        }

        public static void SetAdapter(IAdapter adapter)
        {
            Adapter = adapter;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
