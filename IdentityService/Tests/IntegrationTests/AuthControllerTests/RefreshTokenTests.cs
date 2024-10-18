using Application.DTO;
using Application.UseCases.Comands;
using Bogus;
using Infastructure.Persistanse;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using System.Net;

namespace Tests.IntegrationTests.AuthControllerTests
{
    public class RefreshTokenTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeUsersGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task RefreshToken_Success_ShouldCreateNewToken()
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

            loginResponse.EnsureSuccessStatusCode();
            loginUser.Should().NotBeNull();
            loginUser.JwtToken.Should().NotBeNull();
            loginUser.RefreshToken.Should().NotBeNull();

            var refreshTokenRequestBody = new RefreshTokenCommand
            (
                loginUser.JwtToken
            );

            var refreshTokenRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/auth/token")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(refreshTokenRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            _httpClient.DefaultRequestHeaders.Add("Cookie", $"refreshToken={loginUser.RefreshToken}");

            var refreshTokenResponse = await _httpClient.SendAsync(refreshTokenRequest);

            refreshTokenResponse.EnsureSuccessStatusCode();

            var refreshTokenJson = await refreshTokenResponse.Content.ReadAsStringAsync();
            var refreshedTokenUser = JsonSerializer.Deserialize<LoginResponse>(refreshTokenJson, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Assert

            refreshedTokenUser.Should().NotBeNull();
            refreshedTokenUser.JwtToken.Should().NotBeNull();
            refreshedTokenUser.RefreshToken.Should().NotBeNull();
            refreshedTokenUser.RefreshToken.Should().NotBe(loginUser.RefreshToken);
        }

        [Fact]
        public async Task RefreshToken_Fail_InvalidToken()
        {
            var faker = new Faker();

            var refreshTokenRequestBody = new RefreshTokenCommand
            (
                faker.Random.String(16)
            );

            var refreshTokenRequest = new HttpRequestMessage(new HttpMethod("POST"), "/api/auth/token")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(refreshTokenRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            var refreshTokenResponse = await _httpClient.SendAsync(refreshTokenRequest);

            // Assert

            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
