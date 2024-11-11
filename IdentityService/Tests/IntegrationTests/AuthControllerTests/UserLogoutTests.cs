using Application.DTO;
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
    public class UserLogoutTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeUsersGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task LogoutUser_Success_ShouldLogoutUser()
        {
            var faker = new Faker();
            var registerUserRequestBody = new RegisterDTO
            {
                email = faker.Person.Email,
                login = faker.Person.UserName,
                password = faker.Internet.Password(),
                name = faker.Person.FullName
            };

            var registerUserRequest = new HttpRequestMessage(new HttpMethod("PUT"), "/api/auth")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(registerUserRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            var responseReg = await _httpClient.SendAsync(registerUserRequest);

            var loginUserRequestBody = new UserLoginCommand
            (
                registerUserRequestBody.login,
                registerUserRequestBody.password
            );

            var loginUserRequest = new HttpRequestMessage(new HttpMethod("POST"), $"/api/auth/")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(loginUserRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            var loginResponse = await _httpClient.SendAsync(loginUserRequest);

            var json = await loginResponse.Content.ReadAsStringAsync();

            var loginUser = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var logoutRequest = new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/auth/{loginUser.User.id}");

            // Act

            var response = await _httpClient.SendAsync(logoutRequest);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}
