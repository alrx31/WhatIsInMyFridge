using FluentAssertions;
using System.Net;

namespace Tests.IntegrationTests.UserControllerTests
{
    public class GetUserByIdTests : ControllerTests
    {
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
