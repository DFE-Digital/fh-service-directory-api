using FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingGetServicesByOrganisationId
{
    [Fact]
    public void ThenShouldNotErrorForGetServicesByOrganisationIdWhenModelIsValid()
    {
        //Arrange
        var validator = new GetServicesByOrganisationIdCommandValidator();
        var testModel = new GetServicesByOrganisationIdCommand("56e62852-1b0b-40e5-ac97-54a67ea957dc");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
