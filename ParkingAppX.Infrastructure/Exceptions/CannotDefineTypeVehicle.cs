using System;
namespace ParkingAppX.Infrastructure.Exceptions
{
    [Serializable]
    public class CannotDefineTypeVehicle : Exception
    {
        public CannotDefineTypeVehicle() : base() { }
        public CannotDefineTypeVehicle(string message) : base(message) { }
        public CannotDefineTypeVehicle(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected CannotDefineTypeVehicle(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
