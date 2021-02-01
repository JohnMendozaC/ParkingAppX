using System;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Exceptions;
using ParkingAppX.Domain.Utils;

namespace ParkingAppX.Domain.Aggregate
{
    public class Receipt 
    {
        #region Properties

        public long EntryDate { get; private set; }

        public Vehicle Vehicle { get;  private set; }

        public double Amount { get; private set; }

        private long departureDate;

        public long DepartureDate   
        {
            get { return departureDate; }

            set {
                departureDate = value;
                CalculateAmount();
            }
        }

        #endregion

        #region Constructor

        public Receipt(long entryDate,Vehicle vehicle, bool validatePlate)
        {
            Vehicle = vehicle;
            EntryDate = entryDate;
            ValidateDay(entryDate, vehicle, validatePlate);
        }

        #endregion

        #region Logic

        private void ValidateDay(long entryDate, Vehicle vehicle, bool validatePlate)
        {
            if (vehicle.PlateIsInitA() && validatePlate)
            {
                DateTime dateValue = new DateTime(entryDate);
                var day = dateValue.DayOfWeek;
                if (day != DayOfWeek.Sunday && day != DayOfWeek.Monday)
                {
                    throw new CanNotEnterVehicleException();
                }
            }
        }

        private void CalculateAmount()
        {
            int hours = ConvertDate.GetCheckInAndCheckOutTimes(EntryDate, departureDate);
            if (typeof(Car).IsInstanceOfType(Vehicle))
            {
                Amount = (Vehicle as Car).CalculateAmountCar(hours);
            }
            else if (typeof(Motorcycle).IsInstanceOfType(Vehicle))
            {
                Amount = (Vehicle as Motorcycle).CalculateAmountMotorcycle(hours);
            }
            else
            {
                throw new CalculateAmountException();
            }
        }

        public string GetEntryDateString() => EntryDate.ConvertLongToTime();

        #endregion
    }
}
