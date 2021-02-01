using System;
namespace ParkingAppX.Domain.Exceptions
{
    [Serializable]
    public class CanNotEnterVehicleException : Exception
    {
        public CanNotEnterVehicleException() : base() { }
        public CanNotEnterVehicleException(string message) : base(message) { }
        public CanNotEnterVehicleException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected CanNotEnterVehicleException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
