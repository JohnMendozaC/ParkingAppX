using System.Collections.Generic;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Domain.Entity;

namespace ParkingAppX.Domain.services
{
    public interface IReceiptService
    {
        string EnterVehicle(long entryDate, Vehicle veh);
        List<Receipt> GetVehicles();
        double TakeOutVehicle(long departureDate, Receipt receipt);
    }
}