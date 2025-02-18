using AutoFixture;
using AutoMapper;
using Moq;
using UserService.Application;
using UserService.Application.DTOs;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.UnitTests.ServiceTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EndUserService _endUserService;
    private readonly Fixture _autoFixture = new();
    
    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _endUserService = new EndUserService(_mockUserRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_Exception_If_User_Already_Exists()
    {
        // Arrange
        var testUserDto = _autoFixture.Create<UserDto>();
        _mockMapper.Setup(x => x.Map<User>(It.IsAny<UserDto>())).Returns(_autoFixture.Create<User>());
        _mockUserRepository.Setup(x => x.CheckIfUserExists(It.IsAny<string>())).ReturnsAsync(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _endUserService.CreateAsync(testUserDto));
    }
    
    [Fact]
    public async Task CreateAsync_With_Valid_User_Should_Create_User()
    {
        // Arrange
        var testUserDto = _autoFixture.Create<UserDto>();
        _mockMapper.Setup(x => x.Map<User>(It.IsAny<UserDto>())).Returns(_autoFixture.Create<User>());
        _mockUserRepository.Setup(x => x.CheckIfUserExists(It.IsAny<string>())).ReturnsAsync(true);
        
        // Act
        await _endUserService.CreateAsync(testUserDto);
        
        // Assert
        _mockUserRepository.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
    }
}