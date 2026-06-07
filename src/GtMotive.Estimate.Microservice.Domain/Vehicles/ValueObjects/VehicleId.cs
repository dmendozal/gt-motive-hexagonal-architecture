using System;
using GtMotive.Estimate.Microservice.Domain.Common.Errors;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    /// <summary>
    /// Vehicle identity value object.
    /// </summary>
    public sealed class VehicleId
    {
        private VehicleId(Guid value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the vehicle identifier value.
        /// </summary>
        public Guid Value { get; }

        /// <summary>
        /// Creates a vehicle identifier.
        /// </summary>
        /// <param name="value">The identifier value.</param>
        /// <returns>The vehicle identifier.</returns>
        public static VehicleId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException(Errors.VehicleIdRequired);
            }

            return new VehicleId(value);
        }
    }
}
