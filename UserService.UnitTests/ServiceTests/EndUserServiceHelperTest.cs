
// Homework to write unit tests for end user service helper
using AutoFixture;
using Shouldly;
using UserService.Application.Helpers;
using UserService.Domain;


namespace UserService.UnitTests.ServiceTests
{
    public class EndUserServiceHelperTest
    {
        private readonly Fixture _fixture = new();

        [Theory]
        [InlineData("Mohamad")]
        [InlineData("Aby")]
        [InlineData("Joe")]
        public void Update_Name_Successfully(string newName)
        {
            // Arrange
            var testModel = CreateTestModel();
            var updatedFields = new Dictionary<string, object>
            {
                {"FirstName",newName },
                {"LastName",newName }
            };

            // Assert
            EndUserServiceHelper.ApplyPatch(testModel, updatedFields);

            // Act
            testModel.FirstName.ShouldBe(newName);
            testModel.LastName.ShouldBe(newName);
            
        }

        [Fact]
        public void Validate_Patch_Fields_With_Valid_Fields_Should_Not_Throw_Exception()
        {
            var validFields = new Dictionary<string, object>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" }
            };

            // Act & Assert
            Should.NotThrow(() => EndUserServiceHelper.ValidatePatchFields(validFields));
        }

        [Fact]
        public void Validate_Patch_Fields_With_Invalid_Field_Should_Throw_ArgumentException()
        {
            // Arrange
            var invalidFields = new Dictionary<string, object>
            {
                { "InvalidField", "SomeValue" }
            };

            // Act & Assert
            var exception = Should.Throw<ArgumentException>(() =>
                EndUserServiceHelper.ValidatePatchFields(invalidFields)
            );

            exception.Message.ShouldContain("Field ‘InvalidField’ is not a valid field for User.");
        }

        private User CreateTestModel()
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "test",
                LastName = "test",
                Email = "test@test.com",
                PhoneNumber = "00000000000",
                Address = _fixture.Create<Address>()

            };
        }
    }
}
