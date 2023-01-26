using FamilyHubs.ServiceDirectory.Api.Commands.UpdateTaxonomy;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Taxonomies;

public class WhenValidatingUpdateTaxonomy
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new UpdateTaxonomyCommandValidator();
        var testModel = new UpdateTaxonomyCommand("Id", new TaxonomyDto("Id", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenHasNoId()
    {
        //Arrange
        var validator = new UpdateTaxonomyCommandValidator();
        var testModel = new UpdateTaxonomyCommand("", new TaxonomyDto("Id", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var validator = new UpdateTaxonomyCommandValidator();
        var testModel = new UpdateTaxonomyCommand("Id", new TaxonomyDto("", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Taxonomy.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var validator = new UpdateTaxonomyCommandValidator();
        var testModel = new UpdateTaxonomyCommand("Id", new TaxonomyDto("Id", "", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Taxonomy.Name").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoVocabulary()
    {
        //Arrange
        var validator = new UpdateTaxonomyCommandValidator();
        var testModel = new UpdateTaxonomyCommand("Id", new TaxonomyDto("Id", "Name", "", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Taxonomy.Vocabulary").Should().BeTrue();
    }
}
