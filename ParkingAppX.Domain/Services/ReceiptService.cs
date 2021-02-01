using System;
using System.Collections.Generic;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Enums;
using ParkingAppX.Domain.Exceptions;
using ParkingAppX.Domain.Repository;

namespace ParkingAppX.Domain.services
{
    public class ReceiptService : IReceiptService
    {

        private ReceiptRepository ReceiptRepository { get; set; }

        public ReceiptService(ReceiptRepository NewReceiptRepository)
        {
            ReceiptRepository = NewReceiptRepository;
        }

        private bool IsSpaceForVehicle(Vehicle Veh)
        {
            if (typeof(Car).IsInstanceOfType(Veh))
            {
                return (ReceiptRepository.GetQuantityOfVehicles((int)VehicleType.CAR) < (int)Parking.MAX_CANT_CAR);
            }
            else if (typeof(Motorcycle).IsInstanceOfType(Veh))
            {
                return (ReceiptRepository.GetQuantityOfVehicles((int)VehicleType.MOTORCYCLE) < (int)Parking.MAX_CANT_MOTORCYCLE);
            }
            else
            {
                throw new CalculateAmountException();
            }
        }

        public string EnterVehicle(long entryDate, Vehicle veh)
        {
            Receipt receipt = new Receipt(entryDate, veh, true);

            if (IsSpaceForVehicle(veh))
            {
                if (ReceiptRepository.EnterVehicle(receipt) > 0)
                {
                    return "¡Se guardo el vehiculo con exito!";
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new MaximunCantVehicleException();
            }

        }

        public double TakeOutVehicle(long departureDate, Receipt receipt)
        {
            if (ReceiptRepository.TakeOutVehicle(receipt) > 0)
            {
                receipt.DepartureDate = departureDate;
                return receipt.Amount;
            }
            else
            {
                throw new CalculateAmountException();
            }
        }

        public List<Receipt> GetVehicles() => ReceiptRepository.GetVehicles();

    }
}
