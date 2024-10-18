using FluentAssertions;
using Infastructure.Persistanse;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IntegrationTests;

namespace Tests.IntegrationTests.UserControllerTests
{
    public class DeleteUserTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeUsersGenerator.InitializeData();
            dataContext.AddRange(_fakeUsersGenerator.Users);
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task DeleteUser_Success_shouldDeleteUser()
        {
            var requestBody = 1;

            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/user/{1}")
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
        }

        [Fact]
        public async Task DeleteUser_Fail_UserNotFound()
        {
            var requestBody = 1;

            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/user/{999}")
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
        public async Task DeleteUser_Fail_NotAllowed()
        {
            var requestBody = 2;

            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/user/{1}")
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
