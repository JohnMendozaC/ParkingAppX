using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace ParkingAppX.Droid.Uitest
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            // TODO: If the Android app being tested is included in the solution then open
            // the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            //app = ConfigureApp.Android.ApkFile("/Users/john.mendoza/Projects/ParkingAppX/ParkingAppX.Droid/bin/Debug/com.companyname.parking_appx-Signed.apk").StartApp();
            app = ConfigureApp.Android.ApkFile("/Users/john.mendoza/Projects/ParkingAppX/ParkingAppX.Droid/bin/Release/com.companyname.parking_appx.apk").StartApp();
            /*app = ConfigureApp.Android.ApkFile(Directory.GetCurrentDirectory() + "/ParkingAppX.Droid/bin/Debug/com.companyname.parking_appx.apk").StartApp();
            ConfigureApp.Android.ApkFile(Directory.GetCurrentDirectory() + "/../../../com.companyname.parking_appx.apk").StartApp();*/
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }
    }
}
