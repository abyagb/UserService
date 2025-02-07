using Shouldly;
using UserService.Api.Validators;
using UserService.Api.ViewModels;
using AutoFixture;

namespace UserService.Tests
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static IEnumerable<object[]> UserData()
        {
            var fixture = new Fixture();
            var user = fixture.Build<CreateUserViewModel>()
                              .With(x => x.Email, "test@gmail.com")
                              .With(x => x.FirstName, "testOne")
                              .With(x => x.LastName, "testTwo")
                              .With(x => x.PhoneNumber, "00000000000")
                              .Create();

            yield return new object[] { user };
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_FirstName_Is_Empty(CreateUserViewModel userView)
        {

            userView.FirstName = "";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }
        

        [Theory]
        [MemberData(nameof(UserData))]
        
        public void Should_Return_False_When_FirstName_Less_than_Three_Character(CreateUserViewModel userView)
        {
            userView.FirstName = "ts";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]  
        public void Should_Return_False_When_FirstName_Greater_Than_Fifty(CreateUserViewModel userView)
        {
            userView.FirstName = "";
            for(int i=0;i<54;i++)
            {
                userView.FirstName += "t";
            }
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse(); 
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_True_When_FirstName_Is_Between_Three_And_fifty_Charachters(CreateUserViewModel userView)
        {
            userView.FirstName = "test";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_FirstName_Contains_Special_Characters_Or_Space(CreateUserViewModel userView)
        {
            userView.FirstName = " dkd dl";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_LastName_Is_Empty(CreateUserViewModel userView)
        {

            userView.LastName = "";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_LastName_Less_than_Three_Character(CreateUserViewModel userView)
        {
            userView.LastName = "ts";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_LastName_Greater_Than_Fifty(CreateUserViewModel userView)
        {
            userView.LastName = "";
            for (int i = 0; i < 54; i++)
            {
                userView.FirstName += "t";
            }
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_True_When_LastName_Is_Between_Three_And_fifty_Charachters(CreateUserViewModel userView)
        {
            userView.LastName = "tesfft";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_LastName_Contains_Special_Characters_Or_Space(CreateUserViewModel userView)
        {
            userView.LastName = " dkd dl";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_Email_Is_With_Invalid_Format(CreateUserViewModel userView)
        {
            userView.Email = "test";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_True_When_Email_Is_With_Valid_Format(CreateUserViewModel viewModel)
        {
            viewModel.Email = "test@gmail.com";
            var result = _validator.Validate(viewModel);
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_PhoneNumber_Is_Less_Than_Eleven_Characters(CreateUserViewModel userView)
        {
            userView.PhoneNumber = "123";
             var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_False_When_PhoneNumber_Is_Greater_Than_Eleven_Characters(CreateUserViewModel userView)
        {
            userView.PhoneNumber = "";
            for(int i=0;i<15;i++)
            {
                userView.PhoneNumber += "1";
            }

            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeFalse();
        }

        [Theory]
        [MemberData(nameof(UserData))]
        public void Should_Return_True_When_PhoneNumber_Is_Equal_To_Eleven_Characters(CreateUserViewModel userView)
        {
            userView.PhoneNumber = "00000000000";
            var result = _validator.Validate(userView);
            result.IsValid.ShouldBeTrue();
        }
    }
}