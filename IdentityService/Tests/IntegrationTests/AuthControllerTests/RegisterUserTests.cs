using Application.DTO;
using Application.UseCases.Comands;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infastructure.Persistanse;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IntegrationTests;

namespace Tests.IntegrationTests.AuthControllerTests
{
    public class RegisterUserTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeUsersGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task RegisterUser_Success_ShouldRegisterUser()
        {
            var faker = new Faker();

            var requestBody = new UserRegisterCommand
            (
                faker.Person.UserName,
                faker.Internet.Password(),
                faker.Person.Email,
                faker.Person.FullName
            );

            var request = new HttpRequestMessage(new HttpMethod("PUT"), "/api/auth")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act

            var response = await _httpClient.SendAsync(request);

            //Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginRequestBody = new UserLoginCommand(
                requestBody.Login,
                requestBody.Password
            );

            var user = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("POST"), $"/api/auth/")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(loginRequestBody),
                    Encoding.UTF8,
                    "application/json")
            });

            var userContent = await user.Content.ReadAsStringAsync();

            var userDTO = JsonSerializer.Deserialize<LoginResponse>(userContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            userDTO.IsLoggedIn.Should().Be(true);
            userDTO.JwtToken.Should().NotBeNullOrEmpty();
            userDTO.RefreshToken.Should().NotBeNullOrEmpty();
            userDTO.User.Should().NotBeNull();
            userDTO.User.login.Should().Be(requestBody.Login);
            userDTO.User.email.Should().Be(requestBody.Email);
            userDTO.User.name.Should().Be(requestBody.Name);
        }

        [Fact]
        public async Task RegisterUser_Fail_WhenLoginIsNotAvaible()
        {
            var faker = new Faker();


            var requestBody = new UserRegisterCommand
            (
                faker.Person.UserName,
                faker.Internet.Password(),
                faker.Person.Email,
                faker.Person.FullName
            );

            var request1 = new HttpRequestMessage(new HttpMethod("PUT"), "/api/auth")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            var response1 = await _httpClient.SendAsync(request1);

            var existingUser = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), $"/api/user/{1}"));

            var request = new HttpRequestMessage(new HttpMethod("PUT"), "/api/auth")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
    }
}
