using System;
namespace ParkingAppX.Domain.Exceptions
{
 
    [Serializable]
    public class CalculateAmountException : Exception
    {
        public CalculateAmountException() : base() { }
        public CalculateAmountException(string message) : base(message) { }
        public CalculateAmountException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected CalculateAmountException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
