using FluentAssertions;
using Infastructure.Persistanse;
using System.Net;
using Tests.IdentityServiceTests.IntegrationTests;

namespace Tests.IdentityServiceTests.IntegrationTests.UserControllerTests
{
    public class GetUserByIdTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeUsersGenerator.InitializeData();
            dataContext.AddRange(_fakeUsersGenerator.Users);
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task GetUserById_Success_shouldReturnUser()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/user/{1}");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task GetUserById_Fail_shouldReturnNotFound()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/user/{999}");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
