using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain.Exceptions;
using GtMotive.Estimate.Microservice.Domain.People.Entities;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain
{
    public sealed class PersonTests
    {
        [Fact]
        public void CreateWhenPersonIsValidReturnsPerson()
        {
            var person = Person.Create(Guid.NewGuid(), "12345678A", "Jane Doe");

            person.DocumentId.Value.Should().Be("12345678A");
        }

        [Fact]
        public void CreateWhenDocumentIdIsMissingReturnsFailure()
        {
            Action act = static () => Person.Create(Guid.NewGuid(), string.Empty, "Jane Doe");

            act.Should().Throw<PersonDocumentIdRequiredException>();
        }
    }
}
