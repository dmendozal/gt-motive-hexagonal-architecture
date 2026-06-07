namespace GtMotive.Estimate.Microservice.Domain.Common.Errors
{
    /// <summary>
    /// Contains domain error messages.
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// Indicates that a person already has an active rental and cannot rent another vehicle until the current rental is returned.
        /// </summary>
        public const string PersonAlreadyHasActiveRental = "Person already has an active rental.";

        /// <summary>
        /// Indicates that the person document id is required to create a person or rent a vehicle.
        /// </summary>
        public const string PersonIdRequired = "Person id is required.";

        /// <summary>
        /// Indicates that the person document id is required to create a person or rent a vehicle.
        /// </summary>
        public const string VehicleTooOld = "Vehicle manufacturing date exceeds 5 years.";

        /// <summary>
        /// Indicates that the vehicle id is required to create a vehicle or rent a vehicle.
        /// </summary>
        public const string VehicleIdRequired = "Vehicle id is required.";

        /// <summary>
        /// Indicates that the vehicle VIN is required to create a vehicle or rent a vehicle.
        /// </summary>
        public const string VehicleVinRequired = "Vehicle VIN is required.";

        /// <summary>
        /// Indicates that the vehicle brand is required to create a vehicle or rent a vehicle.
        /// </summary>
        public const string VehicleBrandRequired = "Vehicle brand is required.";

        /// <summary>
        /// Indicates that the vehicle model is required to create a vehicle or rent a vehicle.
        /// </summary>
        public const string VehicleModelRequired = "Vehicle model is required.";

        /// <summary>
        /// Indicates that the vehicle already exists.
        /// </summary>
        public const string DuplicateVehicle = "Vehicle already exists.";

        /// <summary>
        /// Indicates that the vehicle was not found.
        /// </summary>
        public const string VehicleNotFound = "Vehicle was not found.";

        /// <summary>
        /// Indicates that the vehicle is not available for rent.
        /// </summary>
        public const string VehicleAlreadyRented = "Vehicle is not available for rent.";

        /// <summary>
        /// Indicates that the rental id is required.
        /// </summary>
        public const string RentalIdRequired = "Rental id is required.";

        /// <summary>
        /// Indicates that the person document id is required.
        /// </summary>
        public const string PersonDocumentIdRequired = "Person document id is required.";

        /// <summary>
        /// Indicates that the person name is required.
        /// </summary>
        public const string PersonNameRequired = "Person name is required.";

        /// <summary>
        /// Indicates that the rental was not found.
        /// </summary>
        public const string RentalNotFound = "Rental was not found.";

        /// <summary>
        /// Indicates that the rental has already been returned.
        /// </summary>
        public const string RentalAlreadyReturned = "Rental has already been returned.";

        /// <summary>
        /// Indicates that the rental does not belong to the specified person.
        /// </summary>
        public const string RentalDoesNotBelongToPerson = "Rental does not belong to the specified person.";
    }
}
