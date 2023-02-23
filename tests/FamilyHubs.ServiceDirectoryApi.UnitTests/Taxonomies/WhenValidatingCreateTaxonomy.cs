using FamilyHubs.ServiceDirectory.Api.Commands.CreateTaxonomy;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Taxonomies;

public class WhenValidatingCreateTaxonomy
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new CreateTaxonomyCommandValidator();
        var testModel = new CreateTaxonomyCommand(new TaxonomyDto("Id", "Name", TaxonomyType.ServiceCategory, null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var validator = new CreateTaxonomyCommandValidator();
        var testModel = new CreateTaxonomyCommand(new TaxonomyDto("", "Name", TaxonomyType.ServiceCategory, null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Taxonomy.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var validator = new CreateTaxonomyCommandValidator();
        var testModel = new CreateTaxonomyCommand(new TaxonomyDto("Id", "", TaxonomyType.ServiceCategory, null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Taxonomy.Name").Should().BeTrue();
    }
}
