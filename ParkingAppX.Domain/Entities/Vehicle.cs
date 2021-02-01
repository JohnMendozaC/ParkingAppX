using System;
using ParkingAppX.Domain.Enums;

namespace ParkingAppX.Domain.Entity
{
    public abstract class Vehicle
    {
        public string Plate { get; private set; }

        protected Vehicle(string plate)
        {
            Plate = plate;
        }

        public bool PlateIsInitA() => Plate[0].ToString().Equals("A", StringComparison.OrdinalIgnoreCase);

        protected double CalculateAmountDependentDayOrHour(double priceHour, double priceDay, int hours)
        {
            return hours switch
            {
                int i when i < ((int)Time.MAX_HOUR) => hours * priceHour,
                int i when i < ((int)Time.DAY_HOUR) => priceDay,
                _ => CalculateAmountDefault(priceHour, priceDay, hours)
            };

        }

        private double CalculateAmountDefault(double priceHour, double priceDay, int hours)
        {
            float result = hours / (float)Time.DAY_HOUR;
            var days = (int)result;
            var newHours = (result - days) * (int)Time.DAY_HOUR;
            return (days * priceDay) + (newHours * priceHour);
        }

    }
}
