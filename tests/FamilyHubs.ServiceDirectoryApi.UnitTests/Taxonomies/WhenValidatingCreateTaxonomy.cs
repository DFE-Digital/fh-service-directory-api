using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.api.Commands.CreateOpenReferralTaxonomy;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Taxonomies;

public class WhenValidatingCreateTaxonomy
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new CreateOpenReferralTaxonomyCommandValidator();
        var testModel = new CreateOpenReferralTaxonomyCommand(new OpenReferralTaxonomyDto("Id", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var validator = new CreateOpenReferralTaxonomyCommandValidator();
        var testModel = new CreateOpenReferralTaxonomyCommand(new OpenReferralTaxonomyDto("", "Name", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralTaxonomy.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var validator = new CreateOpenReferralTaxonomyCommandValidator();
        var testModel = new CreateOpenReferralTaxonomyCommand(new OpenReferralTaxonomyDto("Id", "", "Vocabulary", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralTaxonomy.Name").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoVocabulary()
    {
        //Arrange
        var validator = new CreateOpenReferralTaxonomyCommandValidator();
        var testModel = new CreateOpenReferralTaxonomyCommand(new OpenReferralTaxonomyDto("Id", "Name", "", null));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralTaxonomy.Vocabulary").Should().BeTrue();
    }
}
