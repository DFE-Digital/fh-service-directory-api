using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.api.Commands.UpdateOpenReferralTaxonomy;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Taxonomies;

public class WhenValidatingUpdateTaxonomy
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new UpdateOpenReferralTaxonomyCommandValidator();
        var testModel = new UpdateOpenReferralTaxonomyCommand("Id", new OpenReferralTaxonomyDto("Id", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenHasNoId()
    {
        //Arrange
        var validator = new UpdateOpenReferralTaxonomyCommandValidator();
        var testModel = new UpdateOpenReferralTaxonomyCommand("", new OpenReferralTaxonomyDto("Id", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var validator = new UpdateOpenReferralTaxonomyCommandValidator();
        var testModel = new UpdateOpenReferralTaxonomyCommand("Id", new OpenReferralTaxonomyDto("", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralTaxonomy.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var validator = new UpdateOpenReferralTaxonomyCommandValidator();
        var testModel = new UpdateOpenReferralTaxonomyCommand("Id", new OpenReferralTaxonomyDto("Id", "", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralTaxonomy.Name").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoVocabulary()
    {
        //Arrange
        var validator = new UpdateOpenReferralTaxonomyCommandValidator();
        var testModel = new UpdateOpenReferralTaxonomyCommand("Id", new OpenReferralTaxonomyDto("Id", "Name", "", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralTaxonomy.Vocabulary").Should().BeTrue();
    }
}
