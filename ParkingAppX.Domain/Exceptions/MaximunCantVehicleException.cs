using System;
namespace ParkingAppX.Domain.Exceptions
{
    [Serializable]
    public class MaximunCantVehicleException : Exception
    {
        public MaximunCantVehicleException() : base() { }
        public MaximunCantVehicleException(string message) : base(message) { }
        public MaximunCantVehicleException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected MaximunCantVehicleException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
