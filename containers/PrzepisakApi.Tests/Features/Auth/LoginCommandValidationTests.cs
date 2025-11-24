using FluentValidation.TestHelper;
using miejsce.api.src.Features.Auth.Application.Login;
using PrzepisakApi.api.src.Features.Auth.Application.Login;

namespace PrzepisakApi.Tests.Features.Auth
{
    public class LoginCommandValidationTests
    {
        private readonly LoginCommandValidation _validator;

        public LoginCommandValidationTests()
        {
            _validator = new LoginCommandValidation();
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Empty()
        {
            var command = new LoginCommand { Username = "", Password = "password" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Username).WithErrorMessage("Podaj nazwę użytkownika");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            var command = new LoginCommand { Username = "user", Password = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("Podaj hasło");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Data_Is_Valid()
        {
            var command = new LoginCommand { Username = "user", Password = "password" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}