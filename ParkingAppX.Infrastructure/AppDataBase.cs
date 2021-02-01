using ParkingAppX.Infrastructure.Entities;
using Realms;

namespace ParkingAppX.Infrastructure
{
    public class AppDataBase
    {
        public RealmConfiguration Config { get; private set; }

        public AppDataBase()
        {
            Config = new RealmConfiguration("ParkingApp.realm")
            {
                ObjectClasses = new[] { typeof(ReceiptEntity) }
            };
        }

        public  Realm GetInstanceSync() => Realm.GetInstance(Config);
    }
}
