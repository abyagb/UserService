using Shouldly;
using UserService.Api.Validators;
using UserService.Api.ViewModels;

namespace UserService.UnitTests
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator _validator = new();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void Should_Return_False_When_Name_Is_Null_Or_Empty(string name)
        {
            // Arrange
            var testModel = CreateTestModel();
            testModel.FirstName = name;
            testModel.LastName = name;

            // Act
            var validationResult = _validator.Validate(testModel);

            // Assert
            validationResult.IsValid.ShouldBeFalse();
            validationResult.Errors.ShouldContain(e => e.PropertyName == "FirstName");
            validationResult.Errors.ShouldContain(e => e.PropertyName == "LastName");
        }

        [Theory]
        [InlineData("Yay", true)] // 3 characters
        [InlineData("Yayy", true)] // 4 characters
        [InlineData("Ya", false)] // 2 characters
        [InlineData("ThisIsATestName", true)] // 15 characters
        [InlineData("tzGGqvdelcJMDLbsXNwxsayTMCuvRizxiOmnXQIpMcxgjirlk", true)] // 49 characters
        [InlineData("tzGGqvdelcJMDLbsXNwxsayTMCuvRizxiOmnXQIpMcxgjirlkT", true)] // 50 characters
        [InlineData("tzGGqvdelcJMDLbsXNwxsayTMCuvRizxiOmnXQIpMcxgjirlkTt", false)] // 51 characters
        [InlineData("LmvLHvCWBiyamViDiySSOkZQIjxMubFLZwVsIGNzVvVuSKoxBtyyBvnFLHNw", false)] // 60 characters
        public void Should_Correct_Validation_Result_For_Name(string name, bool isValid)
        {
            // Arrange
            var testModel = CreateTestModel();
            testModel.FirstName = name;
            testModel.LastName = name;


            // Act
            var validationResult = _validator.Validate(testModel);


            // Assert
            validationResult.IsValid.ShouldBe(isValid);
            if (isValid is false)
            {
                validationResult.Errors.ShouldContain(e => e.PropertyName == "FirstName");
                validationResult.Errors.ShouldContain(e => e.PropertyName == "LastName");
            }
        }

        [Theory]
        [InlineData("test1")]
        [InlineData("!test!")]
        [InlineData("test@")]
        [InlineData("  test#")]
        [InlineData("t e s t $")]
        [InlineData("& .... %")]
        [InlineData("     ^")]
        [InlineData("*    -_  ")]
        public void Should_Return_False_When_Name_Contains_Invalid_Characters(string name)
        {
            // Arrange
            var testModel = CreateTestModel();
            testModel.FirstName = name;
            testModel.LastName = name;

            // Act
            var validationResult = _validator.Validate(testModel);

            // Assert
            validationResult.IsValid.ShouldBeFalse();
            validationResult.Errors.ShouldContain(e => e.PropertyName == "FirstName");
            validationResult.Errors.ShouldContain(e => e.PropertyName == "LastName");
        }

        [Theory]
        [InlineData("testEmail", false)]
        [InlineData("testEmail@", false)]
        [InlineData("testEmail@com", false)]
        [InlineData("www.testEmail.com", false)]
        [InlineData("test@test.com", true)]
        public void Should_Return_Correct_Validation_Result_For_Email(string email, bool isValid)
        {
            // Arrange
            var testModel = CreateTestModel();
            testModel.Email = email;

            // Act
            var validationResult = _validator.Validate(testModel);

            // Assert
            validationResult.IsValid.ShouldBe(isValid);
            if (isValid is false)
            {
                validationResult.Errors.ShouldContain(e => e.PropertyName == "Email");
            }
        }

        [Theory]
        [InlineData("08012345678")]  //11 digits
        [InlineData("07123456789")]  //11 digits
        [InlineData("09098765432")]  //11 digits
        public void Should_Have_No_Validation_Error_For_Valid_PhoneNumber(string phoneNumber)
        {
            // Arrange
            var testModel = CreateTestModel();
            testModel.PhoneNumber = phoneNumber;

            // Act
            var validationResult = _validator.Validate(testModel);

            // Assert
            validationResult.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234567890")]   // 10 digits
        [InlineData("123456789012")] // 12 digits
        [InlineData("abcdefghijk")]
        [InlineData("123-456-78901")]
        [InlineData("071 2345 6789")]
        public void Should_Have_Validation_Error_For_Invalid_PhoneNumber(string phoneNumber)
        {
            // Arrange
            var testModel = CreateTestModel();
            testModel.PhoneNumber = phoneNumber;

            // Act
            var validationResult = _validator.Validate(testModel);

            // Assert
            validationResult.IsValid.ShouldBeFalse();
            validationResult.Errors.ShouldContain(e => e.PropertyName == "PhoneNumber");
        }

        private static CreateUserViewModel CreateTestModel()
        {
            return new CreateUserViewModel
            {
                FirstName = "test",
                LastName = "test",
                Email = "test@test.com",
                PhoneNumber = "00000000000"
            };
        }
    }
}


