using AutoFixture;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.UpsertContact;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.ApiProject
{
    public class UpsertContactCommandTests
    {
        private readonly Fixture _fixture;
        private readonly ILogger<UpsertContactCommandHandler> _logger;
        private readonly Mock<IContactService> _mockContactService;
        private readonly Mock<IMapper> _mockMapper;
        private ContactDto _validContactDto;

        public UpsertContactCommandTests()
        {
            _fixture = new Fixture();
            _logger = Mock.Of<ILogger<UpsertContactCommandHandler>>();
            _mockContactService= new Mock<IContactService>();
            _mockMapper = new Mock<IMapper>();

            _validContactDto = new ContactDto
            {
                Id = "SomeId",
                Name= "Test",
                Email = "test@email.com",
                Telephone = "0208 333 1234",
                TextPhone = "07083 331234",
                Title= "Test",
                Url = "validUrl.com"
            };
        }

        [Fact]
        public async Task Handle_UpsertContactCommand_UpsertCalled_ReturnsSuccessfulResult()
        {
            //  Arrange
            var contact = _fixture.Create<Contact>();
            _mockMapper.Setup(x=>x.Map<Contact>(_validContactDto)).Returns(contact);

            var command = new UpsertContactCommand(_validContactDto);
            var commandHandler = new UpsertContactCommandHandler(_mockContactService.Object, _mockMapper.Object, _logger);

            //  Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //  Assert
            Assert.True(result.Succeeded);
            _mockContactService.Verify(x=>x.Upsert(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UpsertContactCommand_ValidationFails()
        {
            //  Arrange
            var longString = string.Join(string.Empty, _fixture.CreateMany<char>(51));
            var invalidContactDto = new ContactDto
            {
                Name = longString,
                Email = "ImNotAnEmail",
                Telephone = longString,
                TextPhone = longString,
                Title = longString,
                Url = "ImNotAUrl"
            };

            var command = new UpsertContactCommand(invalidContactDto);
            var commandHandler = new UpsertContactCommandHandler(_mockContactService.Object, _mockMapper.Object, _logger);

            //  Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            //  Assert
            Assert.False(result.Succeeded);
            Assert.Contains("The length of 'Title' must be 50 characters or fewer. You entered 51 characters.", result.Errors);
            Assert.Contains("The length of 'Name' must be 50 characters or fewer. You entered 51 characters.", result.Errors);
            Assert.Contains("The length of 'Telephone' must be 50 characters or fewer. You entered 51 characters.", result.Errors);
            Assert.Contains("The length of 'Text Phone' must be 50 characters or fewer. You entered 51 characters.", result.Errors);
            Assert.Contains("'Email' is not a valid email address.", result.Errors);
        }
    }
}
