using Application.UseCases.Comands;
using Bogus;
using FluentAssertions;
using Infastructure.Persistanse;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IntegrationTests;

namespace Tests.IntegrationTests.AuthControllerTests
{
    public class LoginUserTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeUsersGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task LoginUser_Success_WhenUserExist()
        {
            var faker = new Faker();

            var RegisterRequestBody = new UserRegisterCommand
            (
                faker.Person.UserName,
                faker.Person.FullName,
                faker.Person.Email,
                faker.Internet.Password()
            );

            var RegisterRequest = new HttpRequestMessage(new HttpMethod("PUT"), "/api/auth")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(RegisterRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            var RegisterResponse = await _httpClient.SendAsync(RegisterRequest);

            var LoginRequestBody = new UserLoginCommand(
                RegisterRequestBody.Login,
                RegisterRequestBody.Password
            );

            var LoginRequest = new HttpRequestMessage(new HttpMethod("POST"), $"/api/auth/")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(LoginRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var response = await _httpClient.SendAsync(LoginRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();

            var userContent = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            userContent.IsLoggedIn.Should().Be(true);
            userContent.JwtToken.Should().NotBeNullOrEmpty();
            userContent.RefreshToken.Should().NotBeNullOrEmpty();
            userContent.User.Should().NotBeNull();
            userContent.User.login.Should().Be(RegisterRequestBody.Login);
            userContent.User.email.Should().Be(RegisterRequestBody.Email);
            userContent.User.name.Should().Be(RegisterRequestBody.Name);
        }

        [Fact]
        public async Task LoginUser_Fail_WhenInfalidLoginOrPassword()
        {
            var LoginRequest = new HttpRequestMessage(new HttpMethod("POST"), $"/api/auth/")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new UserLoginCommand("invalidLogin", "invalidPassword")),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act
            var response = await _httpClient.SendAsync(LoginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
