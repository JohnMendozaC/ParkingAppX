using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAppX.Infrastructure.Entities;
using Realms;

namespace ParkingAppX.Infrastructure.Daos
{
    public class ReceiptDao
    {

        Realm Realm { get; set; }

        public ReceiptDao(Realm Realm)
        {
            this.Realm = Realm;
        }

        public int GetCountVehicle(int type)
        {
            var receipts = Realm.All<ReceiptEntity>().Where(d => d.Type == type);
            return receipts.Count();
        }

        public long InsertReceipt(ReceiptEntity receiptEntity)
        {
            Realm.Write(() =>
            {
                Realm.Add(receiptEntity);
            });
            return 1;
        }

        public int DeleteReceipt(string plateD)
        {
            var receiptEntity = Realm.All<ReceiptEntity>().First(b => b.Plate == plateD);
            // Delete an object with a transaction
            using (var trans = Realm.BeginWrite())
            {
                Realm.Remove(receiptEntity);
                trans.Commit();
            }
            return 1;
        }

        public List<ReceiptEntity> GetVehicles() => Realm.All<ReceiptEntity>().ToList();
    }
}
