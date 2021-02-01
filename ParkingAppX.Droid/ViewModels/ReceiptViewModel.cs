using System;
using System.Collections.Generic;
using Android.Content;
using AndroidX.Lifecycle;
using Autofac;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Exceptions;
using ParkingAppX.Domain.Repository;
using ParkingAppX.Domain.services;
using ParkingAppX.Droid.DI;
using ParkingAppX.Droid.ViewModels.Response;
using Realms.Exceptions;

namespace ParkingAppX.Droid.ViewModels
{
    public class ReceiptViewModel : ViewModel
    {

        public ReceiptService ReceiptService { get; private set; }
        public Context Context { get; set; }

        public ReceiptViewModel()
        {
            var container = ContainerConfig.Configure();
            using var scope = container.BeginLifetimeScope();
            var receiptRepository = scope.Resolve<ReceiptRepository>();
            ReceiptService = new ReceiptService(receiptRepository);
        }


        public ResourceData<string> EnterVehicle(long entryDate, Vehicle vehicle)
        {
            var resourceData = new ResourceData<string>
            {
                _status = (int)StatusData.LOADING
            };
            try
            {
                resourceData._status = (int)StatusData.SUCCESS;
                resourceData._message = ReceiptService.EnterVehicle(entryDate, vehicle);
            }
            catch (Exception e)
            {
                resourceData._status = (int)StatusData.ERROR;
                resourceData._message = e.GetType() switch
                {
                    Type t when typeof(RealmDuplicatePrimaryKeyValueException).IsInstanceOfType(t) => Context.GetString(Resource.String.vehicle_already_in_the_parking_lot),
                    Type t when typeof(MaximunCantVehicleException).IsInstanceOfType(t) => Context.GetString(Resource.String.there_is_no_space_to_store_the_vehicle),
                    Type t when typeof(CanNotEnterVehicleException).IsInstanceOfType(t) => Context.GetString(Resource.String.you_cannot_enter_the_vehicle_since_it_is_not_sunday_or_monday),
                    _ => Context.GetString(Resource.String.something_unexpected_happened)
                };
            }

            return resourceData;
        }


        public ResourceData<double> TakeOutVehicle(long departureDate, Receipt receipt)
        {
            var resourceData = new ResourceData<double>
            {
                _status = (int)StatusData.LOADING
            };
            try
            {
                resourceData._status = (int)StatusData.SUCCESS;
                resourceData._data = ReceiptService.TakeOutVehicle(departureDate, receipt);
            }
            catch (Exception e)
            {
                resourceData._status = (int)StatusData.ERROR;
                resourceData._message = e.GetType() switch
                {
                    Type t when typeof(CalculateAmountException).IsInstanceOfType(t) => Context.GetString(Resource.String.the_amount_could_not_be_obtained),
                    _ => Context.GetString(Resource.String.something_unexpected_happened)
                };
            }

            return resourceData;
        }

        public ResourceData<List<Receipt>> GetVehiclesAsync()
        {
            var resourceData = new ResourceData<List<Receipt>>
            {
                _status = (int)StatusData.LOADING
            };
            try
            {
                resourceData._data = ReceiptService.GetVehicles();
                resourceData._status = (int)StatusData.SUCCESS;
            }
            catch
            {
                resourceData._status = (int)StatusData.ERROR;
                resourceData._message = Context.GetString(Resource.String.something_unexpected_happened);
            }

            return resourceData;
        }
    }
}
