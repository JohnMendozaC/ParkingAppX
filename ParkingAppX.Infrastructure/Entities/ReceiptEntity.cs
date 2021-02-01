using Realms;

namespace ParkingAppX.Infrastructure.Entities
{
    public class ReceiptEntity : RealmObject
    {
        [PrimaryKey]
        public string Plate { get; set; }

        public long EntryDate { get; set; }

        public int Type { get; set; }

        public long DepartureDate { get; set; }

        public int CylinderCapacity { get; set; }
    }
}
