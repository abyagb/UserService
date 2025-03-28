using AutoFixture;
using Moq;
using Shouldly;
using UserService.Application.DTOs;
using UserService.Application.Validators;
using UserService.Repository.Interfaces;

namespace UserService.UnitTests.ServiceTests
{
    public class EndUserValidatorTest
    {

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly EndUserValidator _validator;
        private Fixture _fixture = new();


        public EndUserValidatorTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _validator = new EndUserValidator(_mockUserRepository.Object);

        }

        [Fact]
        public async Task Should_Return_False_With_Message_When_Email_Already_Exists()
        {
            // Arrange
            var userDto = CreateTestUserDto();
            _mockUserRepository.Setup(x => x.CheckIfEmailExists(userDto.Email)).ReturnsAsync(true);

            // Act
            var (isValid, message) = await _validator.Validate(userDto);

            // Assert
            isValid.ShouldBeFalse();
            message.ShouldBe("Email already exists in the system");
        }

        [Fact]
        public async Task Should_Return_True_With_Empty_Message_When_Email_Doesnt_Exists()
        {
            // Arrange
            var userDto = CreateTestUserDto();
            _mockUserRepository.Setup(x => x.CheckIfEmailExists(userDto.Email)).ReturnsAsync(false);

            // Act
            var (isValid, message) = await _validator.Validate(userDto);

            // Assert
            isValid.ShouldBeTrue();
            message.ShouldBeEmpty();
        }
        
        [Fact]
        public async Task Should_Return_False_With_Message_When_Phone_Number_Already_Exists()
        {
            // Arrange
            var userDto = CreateTestUserDto();
            _mockUserRepository.Setup(x => x.CheckIfPhoneNumberExists(userDto.PhoneNumber)).ReturnsAsync(true);

            // Act
            var (isValid, message) = await _validator.Validate(userDto);

            // Assert
            isValid.ShouldBeFalse();
            message.ShouldBe("Phone number already exists in the system");
        }

        [Fact]
        public async Task Should_Return_True_With_Empty_Message_When_Phone_Number_Doesnt_Exists()
        {
            // Arrange
            var userDto = CreateTestUserDto();
            _mockUserRepository.Setup(x => x.CheckIfPhoneNumberExists(userDto.PhoneNumber)).ReturnsAsync(false);

            // Act
            var (isValid, message) = await _validator.Validate(userDto);

            // Assert
            isValid.ShouldBeTrue();
            message.ShouldBeEmpty();
        }

        private UserDto CreateTestUserDto()
        {
            return _fixture.Create<UserDto>();
        }
    }
}
