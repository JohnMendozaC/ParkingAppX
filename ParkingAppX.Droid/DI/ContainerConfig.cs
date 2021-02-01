using System;
using System.Reflection;
using Autofac;
using ParkingAppX.Domain.Repository;
using ParkingAppX.Domain.services;
using ParkingAppX.Infrastructure.Repositories;

namespace ParkingAppX.Droid.DI
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ReceiptRepositoryRealm>().As<ReceiptRepository>();

            builder.RegisterType<ReceiptService>().As<IReceiptService>();

            /*builder.RegisterAssemblyTypes(Assembly.Load(nameof(Infrastructure)))
                .Where(t => t.Namespace.Contains("Repositories"))*/

            return builder.Build();
        }
    }
}
