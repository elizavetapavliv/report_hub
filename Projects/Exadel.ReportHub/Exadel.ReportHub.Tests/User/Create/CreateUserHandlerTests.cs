using AutoMapper;
using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Host.Mapping.Profiles;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using Moq;

namespace Exadel.ReportHub.Tests.User.Create;

[TestFixture]
public class CreateUserHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private IMapper _mapper;
    private CreateUserHandler _handler;
    private CreateUserRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>());
        _mapper = configuration.CreateMapper();
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new CreateUserHandler(_userRepositoryMock.Object, _mapper);
        _validator = new CreateUserRequestValidator(_userRepositoryMock.Object);
    }

    [Test]
    public async Task CreateUser_ValidRequest_ReturnsUserDTO()
    {
        // Arrange
        var createUserDto = new CreateUserDTO()
        {
            Email = "test@email.com",
            FullName = "Test Name",
            Password = "Test023@"
        };

        // Act
        var createUserRequest = new CreateUserRequest(createUserDto);
        var result = await _handler.Handle(createUserRequest, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.Not.Null, "Result value should not be null");
        Assert.That(result.IsError, Is.False, "Result should be without an error");
        Assert.That(result.Value, Is.InstanceOf<UserDTO>(), "Returned object should be an instance of UserDTO");
        Assert.That(result.Value.Email, Is.EqualTo(createUserDto.Email), "Email should be correct");
        Assert.That(result.Value.FullName, Is.EqualTo(createUserDto.FullName), "FullName should be correct");
    }

    [Test]
    public async Task CreateUser_WithEmptyRequest_ShouldReturnValidationError()
    {
        // Arrange
        var createUserDto = new CreateUserDTO()
        {
            Email = string.Empty,
            FullName = string.Empty,
            Password = string.Empty
        };

        // Act
        var createUserRequest = new CreateUserRequest(createUserDto);
        var result = await _validator.ValidateAsync(createUserRequest, CancellationToken.None);

        // Assert
        Assert.That(result.IsValid, Is.False, "Result should contains errors");
    }
}
