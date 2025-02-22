using AutoMapper;
using Moq;
using Shouldly;
using AutoFixture;
using UserService.Application;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.Mappings;
using UserService.Domain;
using UserService.Repository.Interfaces;

namespace UserService.UnitTests.ServiceTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IEndUserService _endUserService;
        private readonly List<User> _users;
        private readonly Fixture _fixture;

        public UserServiceTest()
        {
            _fixture = new Fixture();
            _users = CreateUsers();
            _userRepositoryMock = ConfigureMockRepository();
            _mapper = ConfigureMapper();
            _endUserService = new EndUserService(_userRepositoryMock.Object, _mapper);
        }

        private List<User> CreateUsers()
        {
            return _fixture.CreateMany<User>(3).ToList();
        }
        private IMapper ConfigureMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });
            return mapperConfig.CreateMapper();
        }

        private Mock<IUserRepository> ConfigureMockRepository()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_users);
            mockRepo.Setup(repo => repo.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => _users.FirstOrDefault(user => user.UserId.Equals(id)));
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask)
                .Callback((User user) =>
                {
                    user.UserId = Guid.NewGuid();
                    _users.Add(user);
                });
            mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask)
                .Callback((User user) => _users.Remove(user));
            mockRepo.Setup(repo => repo.CheckIfUserExists(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string email, string phoneNumber) =>
                {
                    return _users.Any(x => x.Email.Equals(email)) || _users.Any(x => x.PhoneNumber.Equals(phoneNumber));
                });
            return mockRepo;
        }

        [Fact]
        public async Task Should_GetAllUsers()
        {
            var users = await _endUserService.GetAllAsync();
            users.Count().ShouldBe(3);
        }

        [Fact]
        public async Task Should_GetUserById_WhenUserExists()
        {
            var existingUserId = _users[0].UserId;
            var user = await _endUserService.GetUserByIdAsync(existingUserId);
            user.ShouldNotBeNull();
            user.FirstName.ShouldBe(_users[0].FirstName);
            user.LastName.ShouldBe(_users[0].LastName);
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_WhenUserDoesntExists()
        {
            var nonExistentUserId = Guid.NewGuid();
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _endUserService.GetUserByIdAsync(nonExistentUserId)
            );
            exception.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_CreateUser_WhenUserDoesntAlreadyExists()
        {
            var user = _fixture.Create<User>();
            user.Email = "newUser@gmail.com";
            user.PhoneNumber = "01234567890";
            var userDto = _mapper.Map<UserDto>(user);
            await _endUserService.CreateAsync(userDto);
            var addedUser = _users.Find(u => u.Email == userDto.Email && u.PhoneNumber == userDto.PhoneNumber);
            addedUser.ShouldNotBeNull();
            addedUser.Email.ShouldBe(userDto.Email);
            addedUser.PhoneNumber.ShouldBe(userDto.PhoneNumber);
        }

        [Fact]
        public async Task Should_DeleteUser_WhenUserExists()
        {
            var id = _users[0].UserId;
            await _endUserService.DeleteAsync(id);
            var result = _users.FirstOrDefault(x => x.UserId.Equals(id));
            result.ShouldBeNull();
        }

        [Fact]
        public async Task Should_ThrowNotFoundException_WhenDeletingANonExistingUser()
        {
            var nonExistentUserId = Guid.NewGuid();
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                async () => await _endUserService.DeleteAsync(nonExistentUserId)
            );
            exception.ShouldNotBeNull();
        }
    }
}