using Application.DTO;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infastructure.Persistanse;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IdentityServiceTests.IntegrationTests;

namespace Tests.IdentityServiceTests.IntegrationTests.UserControllerTests
{
    public class UpdateUserTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeUsersGenerator.InitializeData();
            dataContext.AddRange(_fakeUsersGenerator.Users);
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task UpdateUser_Success_ShouldUpdateUser()
        {
            var faker = new Faker();

            var requestBody = new RegisterDTO
            {
                login = faker.Person.UserName,
                password = faker.Internet.Password(),
                email = faker.Person.Email,
                name = faker.Person.FullName,
            };

            var userId = 1;

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/user/{userId}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            var response = await _httpClient.SendAsync(request);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), $"/api/user/{userId}"));

            var userContent = await user.Content.ReadAsStringAsync();

            var userDTO = JsonSerializer.Deserialize<User>(userContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            userDTO.login.Should().Be(requestBody.login);
            userDTO.email.Should().Be(requestBody.email);
            userDTO.name.Should().Be(requestBody.name);

        }

        [Fact]
        public async Task UpdateUser_Fail_UserNotFound()
        {
            var faker = new Faker();

            var requestBody = new RegisterDTO
            {
                login = faker.Person.UserName,
                password = faker.Internet.Password(),
                email = faker.Person.Email,
                name = faker.Person.FullName,
            };

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/user/{999}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            var response = await _httpClient.SendAsync(request);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateUser_Fail_WhenInvalidEmail()
        {
            var faker = new Faker();

            var requestBody = new RegisterDTO
            {
                login = faker.Person.UserName,
                password = faker.Internet.Password(),
                email = "invalidEmail",
                name = faker.Person.FullName,
            };

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/user/{1}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            var response = await _httpClient.SendAsync(request);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
