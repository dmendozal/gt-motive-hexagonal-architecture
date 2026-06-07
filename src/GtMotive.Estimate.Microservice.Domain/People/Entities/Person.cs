using System;
using GtMotive.Estimate.Microservice.Domain.Common.Errors;
using GtMotive.Estimate.Microservice.Domain.People.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.People.Entities
{
    /// <summary>
    /// Represents a person who can rent vehicles.
    /// </summary>
    public sealed class Person
    {
        private Person(PersonId id, DocumentId documentId, string name)
        {
            Id = id;
            DocumentId = documentId;
            Name = name;
        }

        /// <summary>
        /// Gets the person identifier.
        /// </summary>
        public PersonId Id { get; }

        /// <summary>
        /// Gets the unique document identifier.
        /// </summary>
        public DocumentId DocumentId { get; }

        /// <summary>
        /// Gets the person name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a person.
        /// </summary>
        /// <param name="id">The person identifier.</param>
        /// <param name="documentId">The unique document identifier.</param>
        /// <param name="name">The person name.</param>
        /// <returns>The person.</returns>
        public static Person Create(Guid id, string documentId, string name)
        {
            var personId = PersonId.Create(id);
            var personDocumentId = ValueObjects.DocumentId.Create(documentId);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException(Errors.PersonNameRequired);
            }

            return new Person(personId, personDocumentId, name);
        }
    }
}
