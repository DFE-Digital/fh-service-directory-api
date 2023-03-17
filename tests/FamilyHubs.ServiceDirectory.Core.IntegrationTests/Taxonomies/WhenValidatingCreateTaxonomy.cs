using FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.CreateTaxonomy;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Taxonomies;

public class WhenValidatingCreateTaxonomy
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new CreateTaxonomyCommandValidator();
        var testModel = new CreateTaxonomyCommand(new TaxonomyDto
        {
            Name = "Name",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        });

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var validator = new CreateTaxonomyCommandValidator();
        var testModel = new CreateTaxonomyCommand(new TaxonomyDto
        {
            Name = "",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        });

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Taxonomy.Name").Should().BeTrue();
    }
}
