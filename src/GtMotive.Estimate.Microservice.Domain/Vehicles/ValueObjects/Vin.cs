using GtMotive.Estimate.Microservice.Domain.Common.Errors;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    /// <summary>
    /// Vehicle VIN value object.
    /// </summary>
    public sealed class Vin
    {
        private Vin(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the VIN value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a VIN.
        /// </summary>
        /// <param name="value">The VIN value.</param>
        /// <returns>The VIN.</returns>
        public static Vin Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException(Errors.VehicleVinRequired);
            }

            return new Vin(value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }
}
