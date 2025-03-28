
// Homework to do Create unit test for edit user validator
//home work to do : find out what test coverage is and how to use Coverlet(a test coverage tool) to inspect my code to see how much is being covered by tests
using Shouldly;

using UserService.Validators;
using UserService.ViewModels;

namespace UserService.UnitTests
{
    public class EditUserValidatorTests
    {
        private readonly EditUserValidator _validator = new();

        [Theory]
        [InlineData(null,true)]
        [InlineData("", false)] // Empty
        [InlineData("YY", false)] // 2 charachters
        [InlineData("YYY", true)] // 3 characters
        [InlineData("YYYY", true)] // 4 characters
        [InlineData("tzGGqvdelcJMDLbsXNwxsayTMCuvRizxiOmnXQIpMcxgjirlk", true)] // 49 characters
        [InlineData("tzGGqvdelcJMDLbsXNwxsayTMCuvRizxiOmnXQIpMcxgjirlkT", true)] // 50 characters
        [InlineData("tzGGqvdelcJMDLbsXNwxsayTMCuvRizxiOmnXQIpMcxgjirlkTt", false)] // 51 characters
        [InlineData("LmvLHvCWBiyamViDiySSOkZQIjxMubFLZwVsIGNzVvVuSKoxBtyyBvnFLHNw", false)] // 60 characters
        public void Should_Return_Correct_Validation_For_Name(string? name, bool isValid)
        {
            // Arrange
            var testModel=CreateTestModel();
            testModel.LastName = name;
            testModel.FirstName = name;

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
        [InlineData(null, true)]
        [InlineData("testEmail", false)]
        [InlineData("testEmail@", false)]
        [InlineData("testEmail@com", false)]
        [InlineData("www.testEmail.com", false)]
        [InlineData("test@test.com", true)]

        public void Should_Return_Correct_Validation_For_Email(string? email, bool isValid)
        {
            // Arrange
            var testModel=CreateTestModel();
            testModel.Email = email;

            // Act
            var validationResult= _validator.Validate(testModel);

            // Assert
            validationResult.IsValid.ShouldBe(isValid);
            if (isValid is false)
            {
                validationResult.Errors.ShouldContain(e => e.PropertyName == "Email");
            }
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("0",  false)] // 1 character
        [InlineData("000", false)] // 2 charachters
        [InlineData("YY", false)]
        [InlineData("YYY", false)]
        [InlineData("00Y", false)]
        [InlineData("0000",false)] // 4 characters
        [InlineData("00000000000", true)] // 11 characters
        public void Should_Return_Correct_Validation_For_PhoneNumber(string? phoneNumber, bool isValid)
        {
            // Arrange
            var testModel= CreateTestModel();
            testModel.PhoneNumber = phoneNumber;

            // Act
            var validationResult= _validator.Validate(testModel);

            // Assert
            validationResult.IsValid.ShouldBe(isValid);

            if (isValid is false)
            {
                validationResult.Errors.ShouldContain(e => e.PropertyName == "PhoneNumber");
            }

        }
        private static EditUserViewModel CreateTestModel()
        {
            return new EditUserViewModel
            {
                FirstName = "test",
                LastName = "test",
                Email = "test@test.com",
                PhoneNumber = "00000000000"
            };
        }
    }

}
