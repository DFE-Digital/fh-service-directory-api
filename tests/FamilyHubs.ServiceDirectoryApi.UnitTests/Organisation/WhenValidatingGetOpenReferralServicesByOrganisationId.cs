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
        var testModel = new GetServicesByOrganisationIdCommand(Random.Shared.Next());

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
