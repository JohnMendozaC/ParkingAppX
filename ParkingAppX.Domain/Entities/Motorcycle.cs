using System;
using ParkingAppX.Domain.Enums;

namespace ParkingAppX.Domain.Entity
{
    public class Motorcycle : Vehicle
    {

        public int CylinderCapacity { get; private set; }

        public Motorcycle(string plate, int cylinderCapacity) : base(plate)
        {
            this.CylinderCapacity = cylinderCapacity;
        }

        public double CalculateAmountMotorcycle(int hours)
        {
            double amount = CalculateAmountDependentDayOrHour(
                (double)Prices.MOTORCYCLE_hour, (double)Prices.MOTORCYCLE_day, hours);

            if(CylinderCapacity > (int)Parking.MAX_CYLINDER_MOTORCYCLE)
              amount += (double)Prices.MOTORCYCLE_additionalAmount;
         
            return amount;
        }
    }
}
