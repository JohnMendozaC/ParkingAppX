using ParkingAppX.Domain.Enums;

namespace ParkingAppX.Domain.Entity
{
    public class Car : Vehicle
    {
        public Car(string plate) : base(plate)
        {
        }

        public double CalculateAmountCar(int hours) => CalculateAmountDependentDayOrHour(
                (double)Prices.CAR_hour, (double)Prices.CAR_day, hours);
    }
}
