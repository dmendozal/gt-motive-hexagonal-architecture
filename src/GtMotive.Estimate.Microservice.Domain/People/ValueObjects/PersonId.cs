using System;
using GtMotive.Estimate.Microservice.Domain.Common.Errors;

namespace GtMotive.Estimate.Microservice.Domain.People.ValueObjects
{
    /// <summary>
    /// Person identity value object.
    /// </summary>
    public sealed class PersonId
    {
        private PersonId(Guid value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the person identifier value.
        /// </summary>
        public Guid Value { get; }

        /// <summary>
        /// Creates a person identifier.
        /// </summary>
        /// <param name="value">The identifier value.</param>
        /// <returns>The person identifier.</returns>
        public static PersonId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException(Errors.PersonIdRequired);
            }

            return new PersonId(value);
        }
    }
}
