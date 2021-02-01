using System.Collections.Generic;
using System.Linq;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Enums;
using ParkingAppX.Infrastructure.Entities;
using ParkingAppX.Infrastructure.Exceptions;

namespace ParkingAppX.Infrastructure.Anticorruption
{
    public static class ReceiptDto
    {
        public static ReceiptEntity ToReceiptEntity(this Receipt receipt)
        {
            int cc = 0;
            int typeVehicle = receipt.Vehicle switch
            {
                Vehicle v when typeof(Car).IsInstanceOfType(v) => (int)VehicleType.CAR,
                Vehicle v when typeof(Motorcycle).IsInstanceOfType(v) => (int)VehicleType.MOTORCYCLE,
                _ => throw new CannotDefineTypeVehicle("")
            };

            if (typeVehicle == (int)VehicleType.MOTORCYCLE)
            {
                cc = (receipt.Vehicle as Motorcycle).CylinderCapacity;
            }

            return new ReceiptEntity
            {
                Plate = receipt.Vehicle.Plate,
                EntryDate = receipt.EntryDate,
                Type = typeVehicle,
                CylinderCapacity = cc
            };
        }

        public static List<Receipt> ToDomainModel(this List<ReceiptEntity> list)
        {
            return list.Select(
                recEntity =>
                {
                    Vehicle vehicle;
                    if (recEntity.Type == (int)VehicleType.CAR)
                    {
                        vehicle = new Car(recEntity.Plate);
                    }
                    else if (recEntity.Type == (int)VehicleType.MOTORCYCLE)
                    {
                        vehicle = new Motorcycle(recEntity.Plate, recEntity.CylinderCapacity);
                    }
                    else
                    {
                        throw new CannotDefineTypeVehicle();
                    }

                    return new Receipt(recEntity.EntryDate, vehicle, false);
                }).ToList();
        }
    }
}
