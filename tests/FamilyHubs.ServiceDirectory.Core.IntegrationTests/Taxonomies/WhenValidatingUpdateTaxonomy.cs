using FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.UpdateTaxonomy;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Taxonomies;

public class WhenValidatingUpdateTaxonomy
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var id = Random.Shared.Next();
        var validator = new UpdateTaxonomyCommandValidator();
        var testModel = new UpdateTaxonomyCommand(id, new TaxonomyDto
        {
            Id = id,
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
        var validator = new UpdateTaxonomyCommandValidator();
        var testModel = new UpdateTaxonomyCommand(0, new TaxonomyDto
        {
            Id = 0,
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
