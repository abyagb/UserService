using AutoMapper;
using Moq;
using Shouldly;
using AutoFixture;
using UserService.Application;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain;
using UserService.Repository.Interfaces;
using UserService.Application.Validators;
using Microsoft.Extensions.Logging;

namespace UserService.UnitTests.ServiceTests
{
    public class EndUserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEndUserValidator> _mockEndUserValidator;
        private readonly Mock<ILogger<EndUserService>> _mockLogger;
        private readonly IEndUserService _endUserService;
        private readonly Fixture _autoFixture = new();
        public EndUserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockEndUserValidator = new Mock<IEndUserValidator>();
            _mockLogger = new Mock<ILogger<EndUserService>>();
            _endUserService = new EndUserService(_mockUserRepository.Object, _mockMapper.Object, _mockEndUserValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Fetches_All_Users_Successfully()
        {
            // Arrange
            var testUsers = CreateTestUsers();
           _mockUserRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(testUsers);
           _mockMapper.Setup(x => x.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>())).Returns(testUsers.Select(x => new UserDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address
            }));

            // Act
            var result = await _endUserService.GetAllAsync();

            // Assert
            result.Count().ShouldBe(testUsers.Count);
        }

        [Fact]
        public async Task Fetches_UserById_When_User_Exists()
        {
            // Arrange
            var testUser = _autoFixture.Create<User>();
            _mockUserRepository.Setup(x => x.GetUserByIdAsync(testUser.UserId)).ReturnsAsync(testUser);
            _mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(
            new UserDto
            {
                FirstName = testUser.FirstName,
                LastName = testUser.LastName,
                Email = testUser.Email,
                PhoneNumber = testUser.PhoneNumber,
                Address = testUser.Address
            });

            // Act
            var result = await _endUserService.GetUserByIdAsync(testUser.UserId);

            // Assert
            result.ShouldNotBeNull();
            result.FirstName.ShouldBe(testUser.FirstName);
            result.LastName.ShouldBe(testUser.LastName);
            result.Email.ShouldBe(testUser.Email);
            result.Address.ShouldBe(testUser.Address);
        }

        //[Fact]
        //public async Task Throws_An_Exception_When_User_Doesnt_Exist()
        //{
        //    var nonExistentUserId = Guid.NewGuid();
        //    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
        //        async () => await _endUserService.GetUserByIdAsync(nonExistentUserId)
        //    );
        //    exception.ShouldNotBeNull();
        //}
        [Fact]
        public async Task Creates_User_Successfully()
        {
            // Arrange
            var testUserDto = _autoFixture.Create<UserDto>();
            _mockEndUserValidator.Setup(x => x.Validate(testUserDto)).ReturnsAsync((true,string.Empty));
            List<User> testUsers=new List<User>();
            _mockUserRepository.Setup(x => x.CreateAsync(It.IsAny<User>()))
                .Callback<User>(user =>
                {
                    user.UserId=Guid.NewGuid();
                    testUsers.Add(user);
                });
            _mockMapper.Setup(x => x.Map<User>(It.IsAny<UserDto>())).Returns(
           new User
           {
               FirstName = testUserDto.FirstName,
               LastName = testUserDto.LastName,
               Email = testUserDto.Email,
               PhoneNumber = testUserDto.PhoneNumber,
               Address = testUserDto.Address
           });

            // Act
            await _endUserService.CreateAsync(testUserDto);


            // Assert
            testUsers.ShouldNotBeNull();
            testUsers.Count.ShouldBe(1);
            testUsers.Any(x => x.Email == testUserDto.Email).ShouldBeTrue();
        }

        [Fact]
        public async Task Deletes_User_Successfully()
        {
            // Arrange
            var testUserDto = _autoFixture.Create<UserDto>();
            _mockEndUserValidator.Setup(x => x.Validate(testUserDto)).ReturnsAsync((true, string.Empty));
            List<User> testUsers = new List<User>();
            _mockUserRepository.Setup(x => x.CreateAsync(It.IsAny<User>()))
                .Callback<User>(user =>
                {
                    user.UserId = Guid.NewGuid();
                    testUsers.Add(user);
                });
            _mockUserRepository.Setup(x => x.DeleteAsync(It.IsAny<User>()))
               .Callback<User>(user =>
               {
                   testUsers.Remove(user);
               });
            
            _mockMapper.Setup(x => x.Map<User>(It.IsAny<UserDto>())).Returns(
           new User
           {
               FirstName = testUserDto.FirstName,
               LastName = testUserDto.LastName,
               Email = testUserDto.Email,
               PhoneNumber = testUserDto.PhoneNumber,
               Address = testUserDto.Address
           });

            // Act
            await _endUserService.CreateAsync(testUserDto);
            _mockUserRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(testUsers[0]);


            // Assert
            testUsers.ShouldNotBeNull();
            testUsers.Count.ShouldBe(1);
            testUsers.Any(x => x.Email == testUserDto.Email).ShouldBeTrue();

            // Act
            await _endUserService.DeleteAsync(testUsers[0].UserId);
            testUsers.Any(x => x.Email == testUserDto.Email).ShouldBeFalse();


        }

        
        // Home work write test for editing a user

        //[Fact]
        //public async Task Creates_User_Successfully()
        //{
        //    // Arrange
        //    var testUserDto = _autoFixture.Create<UserDto>();

        //    // Act

        //    // Assert



        //    await _endUserService.CreateAsync(userDto);
        //    var addedUser = _users.Find(u => u.Email == userDto.Email && u.PhoneNumber == userDto.PhoneNumber);
        //    addedUser.ShouldNotBeNull();
        //    addedUser.Email.ShouldBe(userDto.Email);
        //    addedUser.PhoneNumber.ShouldBe(userDto.PhoneNumber);
        //}

        //[Fact]
        //public async Task Deletes_User_Successfully()
        //{
        //    var id = _users[0].UserId;
        //    await _endUserService.DeleteAsync(id);
        //    var result = _users.FirstOrDefault(x => x.UserId.Equals(id));
        //    result.ShouldBeNull();
        //}

        //[Fact]
        //public async Task Deletes_User_Successfully()
        //{
        //    var nonExistentUserId = Guid.NewGuid();
        //    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
        //        async () => await _endUserService.DeleteAsync(nonExistentUserId)
        //    );
        //    exception.ShouldNotBeNull();
        //}

        private List<User> CreateTestUsers()
        {
            return _autoFixture.CreateMany<User>(3).ToList();
        }
    }
}