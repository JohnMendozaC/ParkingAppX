using System.Collections.Generic;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Domain.Repository;
using ParkingAppX.Infrastructure.Anticorruption;
using ParkingAppX.Infrastructure.Daos;

namespace ParkingAppX.Infrastructure.Repositories
{
    public class ReceiptRepositoryRealm : ReceiptRepository
    {
        ReceiptDao ReceiptDao { get; set; }

        public ReceiptRepositoryRealm()
        {
            InstanceDB();
        }

        public void InstanceDB()
        {
            AppDataBase db = new AppDataBase();
            Realms.Realm instance = db.GetInstanceSync();
            ReceiptDao = new ReceiptDao(instance);
        }

        public long EnterVehicle(Receipt receipt)
        {
            ReceiptDao.InsertReceipt(receipt.ToReceiptEntity());
            return 1;
        }

        public int GetQuantityOfVehicles(int typeVehicle)
        {
            return ReceiptDao.GetCountVehicle(typeVehicle);
        }

        public List<Receipt> GetVehicles()
        {
            return ReceiptDao.GetVehicles().ToDomainModel();
        }

        public int TakeOutVehicle(Receipt receipt)
        {
            ReceiptDao.DeleteReceipt(receipt.Vehicle.Plate);
            return 1;
        }
    }
}
