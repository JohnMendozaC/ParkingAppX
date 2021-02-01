using System;
using Android.App;
using Android.Runtime;
using Autofac;
using Autofac.Core;
using ParkingAppX.Domain.Repository;
using ParkingAppX.Droid.DI;

namespace ParkingAppX.Droid
{
    public class App : Application
    {
        public override void OnCreate()
        {
            base.OnCreate();

           /* var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var repository = scope.Resolve<ReceiptRepository>();
                repository.GetVehicles();
            } */

        }
    }
}
