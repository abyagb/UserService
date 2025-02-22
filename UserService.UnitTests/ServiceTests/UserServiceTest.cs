using AutoMapper;
using Moq;
using Shouldly;
using AutoFixture;
using UserService.Application;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.UnitTests.ServiceTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IEndUserService _endUserService;
        private readonly Fixture _autoFixture = new();

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _endUserService = new EndUserService(_mockUserRepository.Object, _mockMapper.Object);
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

        //[Fact]
        //public async Task Return_Empty_When_No_Users()
        //{
        //    var users = await _endUserService.GetAllAsync();
        //    users.Count().ShouldBe(3);
        //}


        //[Fact]
        //public async Task Fetches_UserById_When_User_Exists()
        //{
        //    var existingUserId = _users[0].UserId;
        //    var user = await _endUserService.GetUserByIdAsync(existingUserId);
        //    user.ShouldNotBeNull();
        //    user.FirstName.ShouldBe(_users[0].FirstName);
        //    user.LastName.ShouldBe(_users[0].LastName);
        //}

        //[Fact]
        //public async Task Throws_An_Exception_When_User_Doesnt_Exist()
        //{
        //    var nonExistentUserId = Guid.NewGuid();
        //    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
        //        async () => await _endUserService.GetUserByIdAsync(nonExistentUserId)
        //    );
        //    exception.ShouldNotBeNull();
        //}


        //[Fact]
        //public async Task Should_CreateUser_WhenUserDoesntAlreadyExists()
        //{
        //    var user = _fixture.Create<User>();
        //    user.Email = "newUser@gmail.com";
        //    user.PhoneNumber = "01234567890";
        //    var userDto = _mapper.Map<UserDto>(user);
        //    await _endUserService.CreateAsync(userDto);
        //    var addedUser = _users.Find(u => u.Email == userDto.Email && u.PhoneNumber == userDto.PhoneNumber);
        //    addedUser.ShouldNotBeNull();
        //    addedUser.Email.ShouldBe(userDto.Email);
        //    addedUser.PhoneNumber.ShouldBe(userDto.PhoneNumber);
        //}

        //[Fact]
        //public async Task Creates_User_Successfully()
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