using System;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Exceptions;
using Xunit;

namespace ParkingAppX.Domain.Test
{
    public class ReceiptTest
    {
        [Fact]
        public void Receipt_createReceiptWithVehicle()
        {
            //Arrange
            long entryVehicle = 637474176000000000; // 28/01/2021 8 AM
            Vehicle vehicle = new Motorcycle("BJO90F", 150);

            //Act
            Receipt expected = new Receipt(entryVehicle, vehicle, true);

            //Assert
            Assert.NotNull(expected);
        }

        [Fact]
        public void Receipt_createReceiptWithCarOut()
        {
            //Arrange
            long entryVehicle = 637474176000000000; // 28/01/2021 8 AM
            Vehicle vehicle = new Car("BJO90F");

            //Act
            Receipt expected = new Receipt(entryVehicle, vehicle, true)
            {
                DepartureDate = 637475148000000000 // 28/01/2021 8 AM + 27 hours
            };

            //Assert
            Assert.Equal(11000.0, expected.Amount);
        }

        [Fact]
        public void DepartureDate_MotorcycleOutHigherCC500_CorrectValue()
        {
            //Arrange
            long entryVehicle = 637474176000000000; // 28/01/2021 8 AM
            Vehicle vehicle = new Motorcycle("BJO90F", 650);

            //Act
            Receipt expected = new Receipt(entryVehicle, vehicle, false)
            {
                DepartureDate = 637474536000000000 // 28/01/2021 8 AM + 10 hours
            };

            //Assert
            Assert.Equal(6000.0, expected.Amount);
        }

        [Fact]
        public void DepartureDate_MotorcycleOutHigherCC150_CorrectValue()
        {
            //Arrange
            long entryVehicle = 637474176000000000; // 28/01/2021 8 AM
            Vehicle vehicle = new Motorcycle("BJO90F", 150);

            //Act
            Receipt expected = new Receipt(entryVehicle, vehicle, false)
            {
                DepartureDate = 637474536000000000 // 28/01/2021 8 AM + 10 hours
            };

            //Assert
            Assert.Equal(4000.0, expected.Amount);
        }

        [Fact]
        public void Receipt_createReceiptWithMotorcyclePlateInitANotCanEntry()
        {
            //Arrange
            long entryVehicle = 637474176000000000; // 28/01/2021 8 AM
            Vehicle vehicle = new Motorcycle("AJO90F", 650);
            try
            {
                //Act
                Receipt expected = new Receipt(entryVehicle, vehicle, false);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.IsType<CanNotEnterVehicleException>(ex);
            }
        }

        [Fact]
        public void Receipt_createReceiptWithMotorcyclePlateInitACanEntryMon()
        {
            //Arrange
            long entryVehicle = 637471944000000000; // 25/01/2021 6 PM
            Vehicle vehicle = new Motorcycle("AJO95F", 150);

            //Act
            Receipt expected = new Receipt(entryVehicle, vehicle, true);

            //Assert
            Assert.NotNull(expected);
        }

        [Fact]
        public void Receipt_createReceiptWithMotorcyclePlateInitACanEntrySun()
        {
            //Arrange
            long entryVehicle = 637471080000000000; // 24/01/2021 6 PM
            Vehicle vehicle = new Motorcycle("AJO95F", 150);

            //Act
            Receipt expected = new Receipt(entryVehicle, vehicle, true);

            //Assert
            Assert.NotNull(expected);
        }
    }
}
