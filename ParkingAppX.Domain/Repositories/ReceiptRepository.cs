using System.Collections.Generic;
using ParkingAppX.Domain.Aggregate;

namespace ParkingAppX.Domain.Repository
{
    public interface ReceiptRepository
    {
        int GetQuantityOfVehicles(int typeVehicle);

        long EnterVehicle(Receipt receipt);

        int TakeOutVehicle(Receipt receipt);

        List<Receipt> GetVehicles();
    }
}
